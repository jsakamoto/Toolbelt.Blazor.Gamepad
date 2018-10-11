using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.Gamepad
{
    /// <summary>
    /// Provides gamepad API access.
    /// </summary>
    public class GamepadList
    {
        private List<Gamepad> _Gamepads = new List<Gamepad>();

        /// <summary>
        /// Initialize a new instance of the GamepadList class.
        /// </summary>
        internal GamepadList()
        {
        }

        /// <summary>
        /// Get the list of activated gamepad objects.
        /// </summary>
        public async Task<IReadOnlyList<Gamepad>> GetGamepadsAsync()
        {
            var latestGamePads = await JSRuntime.Current.InvokeAsync<string[][]>("Toolbelt.Blazor.Gamepad.getGamepads");
            var toAddPads = latestGamePads.Where(p1 => !_Gamepads.Any(p2 => p1[0] == p2.Id && p1[1] == p2.Index.ToString())).ToArray();
            var toRemovePads = _Gamepads.Where(p1 => !latestGamePads.Any(p2 => p1.Id == p2[0] && p1.Index.ToString() == p2[1])).ToArray();

            foreach (var item in toAddPads)
            {
                _Gamepads.Add(new Gamepad(item[0], int.Parse(item[1]), true));
            }
            foreach (var pad in toRemovePads)
            {
                pad._Connected = false;
                _Gamepads.Remove(pad);
            }

            return _Gamepads;
        }
    }
}
