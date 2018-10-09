using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.Gamepad
{
    public class Gamepad
    {
        public string Id { get; set; }

        public int Index { get; set; }

        public bool Connected { get; internal set; }

        private double[] _Axes = new double[0];

        public IReadOnlyList<double> Axes => this.Refresh()._Axes;

        private GamepadButton[] _Buttons = new GamepadButton[0];

        public IReadOnlyList<GamepadButton> Buttons => this.Refresh()._Buttons;

        private Task LastRefreshTask = null;

        private Gamepad Refresh()
        {
            if ((LastRefreshTask?.IsCompleted ?? true) == true)
            {
                LastRefreshTask = JSRuntime.Current.InvokeAsync<Gamepad[]>("Toolbelt.Blazor.Gamepad.refresh", this.Id, this.Index);
            }
            return this;
        }

        internal void UpdateStatus(bool connected, double[] axes, GamepadButton[] buttons)
        {
            Connected = connected;
            _Axes = axes;
            _Buttons = buttons;
        }
    }
}
