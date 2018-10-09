using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.Gamepad
{
    public class GamepadList
    {
        private bool _Attached = false;

        private List<Gamepad> _Gamepads = new List<Gamepad>();

        /// <summary>
        /// Initialize a new instance of the GamepadList class.
        /// </summary>
        internal GamepadList()
        {
        }

        /// <summary>
        /// Attach this GamepadList service instance to JavaScript DOM event handler.
        /// </summary>
        private async Task AttachAsync()
        {
            if (_Attached) return;
            await JSRuntime.Current.InvokeAsync<object>("Toolbelt.Blazor.Gamepad.attach", new DotNetObjectRef(this));
            _Attached = true;
        }

        public async Task<IReadOnlyList<Gamepad>> GetGamepadsAsync()
        {
            await AttachAsync();

            var latestGamePads = await JSRuntime.Current.InvokeAsync<Gamepad[]>("Toolbelt.Blazor.Gamepad.getGamepads");
            var toAddPads = latestGamePads.Where(p1 => !_Gamepads.Any(p2 => p1.Id == p2.Id && p1.Index == p2.Index)).ToArray();
            var toRemovePads = _Gamepads.Where(p1 => !latestGamePads.Any(p2 => p1.Id == p2.Id && p1.Index == p2.Index)).ToArray();

            _Gamepads.AddRange(toAddPads);
            foreach (var pad in _Gamepads) pad.Connected = true;
            foreach (var pad in toRemovePads) pad.Connected = false;// _GamePads.Remove(pad);

            return _Gamepads;
        }

        [JSInvokable(nameof(UpdateGamepad)), EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateGamepad(string id, int index, bool connected, double[] axes, GamepadButton[] buttons)
        {
            var gamePad = _Gamepads.Find(pad => pad.Id == id && pad.Index == index);
            gamePad?.UpdateStatus(connected, axes, buttons);
        }
    }
}
