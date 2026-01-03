using Microsoft.JSInterop;

namespace Toolbelt.Blazor.Gamepad;

/// <summary>
/// Provides gamepad API access.
/// </summary>
public class GamepadList : IAsyncDisposable
{
    private readonly IJSRuntime JSRuntime;

    private IJSObjectReference? _JSModule;

    private readonly List<Gamepad> _Gamepads = new();

    private readonly SemaphoreSlim Syncer = new(1, 1);

    /// <summary>
    /// Initialize a new instance of the GamepadList class.
    /// </summary>
    internal GamepadList(IJSRuntime jSRuntime)
    {
        this.JSRuntime = jSRuntime;
    }

    /// <summary>
    /// Get the list of activated gamepad objects.
    /// </summary>
    public async ValueTask<IReadOnlyList<Gamepad>> GetGamepadsAsync()
    {
        var jsmodule = await this.GetJSModuleAsync();
        if (jsmodule == null) return this._Gamepads;

        var latestGamePads = await jsmodule.InvokeAsync<string[][]>("getGamepads");
        var toAddPads = latestGamePads.Where(p1 => !this._Gamepads.Any(p2 => p1[0] == p2.Id && p1[1] == p2.Index.ToString())).ToArray();
        var toRemovePads = this._Gamepads.Where(p1 => !latestGamePads.Any(p2 => p1.Id == p2[0] && p1.Index.ToString() == p2[1])).ToArray();

        foreach (var item in toAddPads)
        {
            this._Gamepads.Add(new Gamepad(jsmodule, item[0], int.Parse(item[1]), true));
        }
        foreach (var pad in toRemovePads)
        {
            pad._Connected = false;
            this._Gamepads.Remove(pad);
        }

        return this._Gamepads;
    }

    private static bool CacheBustingEnabled() => Environment.GetEnvironmentVariable("TOOLBELT_BLAZOR_GAMEPAD_JSCACHEBUSTING") != "0";

    private async ValueTask<IJSObjectReference?> GetJSModuleAsync()
    {
        if (this._JSModule is not null) return this._JSModule;

        await this.Syncer.WaitAsync();
        try
        {
            if (this._JSModule is null)
            {
#if NET10_0_OR_GREATER
                var cacheBustingQueryAsync = CacheBustingEnabled() ?
                    this.JSRuntime.GetValueAsync<bool>("navigator.onLine").AsTask().ContinueWith(static task => task.Result ? "?v=" + VersionInfo.VersionText : "") :
                    Task.FromResult("");

                this._JSModule = await cacheBustingQueryAsync
                    .ContinueWith(task => this.JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Toolbelt.Blazor.Gamepad/script.min.js" + task.Result).AsTask())
                    .Unwrap();
#else
                var isOnLine = await this.JSRuntime.InvokeAsync<bool>("Toolbelt.Blazor.getProperty", "navigator.onLine");
                var scriptPath = $"./_content/Toolbelt.Blazor.Gamepad/script.min.js";
                if (isOnLine) scriptPath += $"?v={VersionInfo.VersionText}";

                this._JSModule = await this.JSRuntime.InvokeAsync<IJSObjectReference>("import", scriptPath);
#endif
            }
        }
        finally { this.Syncer.Release(); }

        return this._JSModule;
    }

    public async ValueTask DisposeAsync()
    {
        if (this._JSModule is not null)
        {
            try { await this._JSModule.DisposeAsync(); }
            catch (JSDisconnectedException) { }
        }
    }
}
