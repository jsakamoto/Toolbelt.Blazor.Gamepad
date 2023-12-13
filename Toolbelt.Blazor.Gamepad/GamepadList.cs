using System.Reflection;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.Gamepad;

/// <summary>
/// Provides gamepad API access.
/// </summary>
public class GamepadList : IAsyncDisposable
{
    private readonly IJSRuntime JSRuntime;

    private JSInvoker? JSInvoker;

    private readonly GamepadOptions Options;

    private readonly List<Gamepad> _Gamepads = new();

    private readonly SemaphoreSlim Syncer = new(1, 1);

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

            if (!this.Options.DisableClientScriptAutoInjection)
            {
                var isOnLine = await this.JSRuntime.InvokeAsync<bool>("Toolbelt.Blazor.getProperty", "navigator.onLine");
                var scriptPath = $"./_content/Toolbelt.Blazor.Gamepad/script.module.min.js";
                if (isOnLine) scriptPath += $"?v={this.GetVersionText()}";

                var module = await this.JSRuntime.InvokeAsync<IJSObjectReference>("import", scriptPath);
                this.JSInvoker = new JSInvoker(this.JSRuntime, module);
            }
            else
            {
                await this.JSRuntime.InvokeVoidAsync("Toolbelt.Blazor.getProperty", "Toolbelt.Blazor.SpeechRecognition.ready");
                this.JSInvoker = new JSInvoker(this.JSRuntime, null);
            }
        }
        catch (InvalidOperationException) { }
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
        if (this.JSInvoker != null) await this.JSInvoker.DisposeAsync();
    }
}
