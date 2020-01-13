using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.Gamepad
{
    /// <summary>
    /// Provides gamepad API access.
    /// </summary>
    public class GamepadList
    {
        private readonly IJSRuntime JSRuntime;

        private readonly List<Gamepad> _Gamepads = new List<Gamepad>();

        private bool ScriptLoaded = false;

        private readonly SemaphoreSlim Syncer = new SemaphoreSlim(1, 1);

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
            await EnsureScriptAsync();
            if (!ScriptLoaded) return _Gamepads;

            var latestGamePads = await JSRuntime.InvokeAsync<string[][]>("Toolbelt.Blazor.Gamepad.getGamepads");
            var toAddPads = latestGamePads.Where(p1 => !_Gamepads.Any(p2 => p1[0] == p2.Id && p1[1] == p2.Index.ToString())).ToArray();
            var toRemovePads = _Gamepads.Where(p1 => !latestGamePads.Any(p2 => p1.Id == p2[0] && p1.Index.ToString() == p2[1])).ToArray();

            foreach (var item in toAddPads)
            {
                _Gamepads.Add(new Gamepad(JSRuntime, item[0], int.Parse(item[1]), true));
            }
            foreach (var pad in toRemovePads)
            {
                pad._Connected = false;
                _Gamepads.Remove(pad);
            }

            return _Gamepads;
        }

        private async ValueTask EnsureScriptAsync()
        {
            if (ScriptLoaded) return;

            await Syncer.WaitAsync();
            try
            {
                if (ScriptLoaded) return;
                const string scriptPath = "_content/Toolbelt.Blazor.Gamepad/script.min.js";
                await JSRuntime.InvokeVoidAsync("eval", "new Promise(r=>((d,t,s)=>(h=>h.querySelector(t+`[src=\"${s}\"]`)?r():(e=>(e.src=s,e.onload=r,h.appendChild(e)))(d.createElement(t)))(d.head))(document,'script','" + scriptPath + "'))");
                ScriptLoaded = true;
            }
            catch (Exception) { }
            finally { Syncer.Release(); }
        }
    }
}
