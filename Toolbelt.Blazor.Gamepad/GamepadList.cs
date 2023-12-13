using System.Reflection;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.Gamepad;

/// <summary>
/// Provides gamepad API access.
/// </summary>
public class GamepadList : IAsyncDisposable
{
    private readonly IJSRuntime JSRuntime;

    private IJSObjectReference? JSModule;

    private JSInvoker? JSInvoker;

    private readonly GamepadOptions Options;

    private readonly List<Gamepad> _Gamepads = new List<Gamepad>();

    private readonly SemaphoreSlim Syncer = new SemaphoreSlim(1, 1);

    /// <summary>
    /// Initialize a new instance of the GamepadList class.
    /// </summary>
    internal GamepadList(IJSRuntime jSRuntime, GamepadOptions options)
    {
        this.JSRuntime = jSRuntime;
        this.Options = options;
    }

    /// <summary>
    /// Get the list of activated gamepad objects.
    /// </summary>
    public async ValueTask<IReadOnlyList<Gamepad>> GetGamepadsAsync()
    {
        var jsInvoker = await this.GetJSInvokerAsync();
        if (jsInvoker == null) return this._Gamepads;

        var latestGamePads = await jsInvoker.InvokeAsync<string[][]>("Toolbelt.Blazor.Gamepad.getGamepads");
        var toAddPads = latestGamePads.Where(p1 => !this._Gamepads.Any(p2 => p1[0] == p2.Id && p1[1] == p2.Index.ToString())).ToArray();
        var toRemovePads = this._Gamepads.Where(p1 => !latestGamePads.Any(p2 => p1.Id == p2[0] && p1.Index.ToString() == p2[1])).ToArray();

        foreach (var item in toAddPads)
        {
            this._Gamepads.Add(new Gamepad(jsInvoker, item[0], int.Parse(item[1]), true));
        }
        foreach (var pad in toRemovePads)
        {
            pad._Connected = false;
            this._Gamepads.Remove(pad);
        }

        return this._Gamepads;
    }

    private async ValueTask<JSInvoker?> GetJSInvokerAsync()
    {
        if (this.JSInvoker != null) return this.JSInvoker;

        await this.Syncer.WaitAsync();
        try
        {
            if (this.JSInvoker != null) return this.JSInvoker;

            var version = this.GetVersionText();
            if (!this.Options.DisableClientScriptAutoInjection)
            {
                var scriptPath = $"./_content/Toolbelt.Blazor.Gamepad/script.module.min.js?v={version}";
                this.JSModule = await this.JSRuntime.InvokeAsync<IJSObjectReference>("import", scriptPath);
                this.JSInvoker = new JSInvoker(this.JSRuntime, this.JSModule);
            }
            else
            {
                try { await this.JSRuntime.InvokeVoidAsync("Toolbelt.Blazor.Gamepad.ready"); } catch { }
                this.JSInvoker = new JSInvoker(this.JSRuntime, null);
            }
        }
        catch (Exception) { }
        finally { this.Syncer.Release(); }

        return this.JSInvoker;
    }

    private string GetVersionText()
    {
        var assembly = this.GetType().Assembly;
        var version = assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? assembly.GetName().Version?.ToString() ?? "0.0.0";
        return version;
    }

    public async ValueTask DisposeAsync()
    {
        if (this.JSModule != null) await this.JSModule.DisposeAsync();
    }
}
