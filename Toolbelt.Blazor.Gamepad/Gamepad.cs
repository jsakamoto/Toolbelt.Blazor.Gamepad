using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.Gamepad
{
    /// <summary>
    /// An individual gamepad or other controller device.
    /// </summary>
    public class Gamepad
    {
        private readonly IJSRuntime JSRuntime;

        /// <summary>
        /// A string containing identifying information about the controller.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// An integer that is auto-incremented to be unique for each device currently connected to the system.
        /// </summary>
        public int Index { get; }

        internal bool _Connected;

        /// <summary>
        /// A boolean indicating whether the gamepad is still connected to the system.
        /// </summary>
        public bool Connected => this.Refresh()._Connected;

        private double[] _Axes = new double[0];

        /// <summary>
        /// A list representing the controls with axes present on the device (e.g. analog thumb sticks).
        /// </summary>
        public IReadOnlyList<double> Axes => this.Refresh()._Axes;

        private List<GamepadButton> _Buttons = new List<GamepadButton>();

        /// <summary>
        /// A list of GamepadButton objects representing the buttons present on the device.
        /// </summary>
        public IReadOnlyList<GamepadButton> Buttons => this.Refresh()._Buttons;

        private Task LastRefreshTask = null;

        private DotNetObjectRef<Gamepad> _ObjectRefOfThis;

        internal Gamepad(IJSRuntime jSRuntime, string id, int index, bool connected)
        {
            JSRuntime = jSRuntime;
            Id = id;
            Index = index;
            _Connected = connected;
        }

        private Gamepad Refresh()
        {
            if ((LastRefreshTask?.IsCompleted ?? true) == true)
            {
                LastRefreshTask?.Dispose();
                if (_ObjectRefOfThis == null) _ObjectRefOfThis = DotNetObjectRef.Create(this);
                LastRefreshTask = JSRuntime.InvokeAsync<object>("Toolbelt.Blazor.Gamepad.refresh", _ObjectRefOfThis, this.Id, this.Index);
            }
            return this;
        }

        [JSInvokable(nameof(UpdateStatus)), EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateStatus(bool connected, double[] axes, bool[] buttonsPressed, double[] buttonsValue)
        {
            _Connected = connected;
            _Axes = axes;

            var buttonsCount = _Buttons.Count;
            for (var i = 0; i < buttonsPressed.Length; i++)
            {
                var button = default(GamepadButton);
                if (i < buttonsCount)
                {
                    button = _Buttons[i];
                }
                else
                {
                    button = new GamepadButton();
                    _Buttons.Add(button);
                }
                button.Pressed = buttonsPressed[i];
                button.Value = buttonsValue[i];
            }
            if (buttonsPressed.Length < buttonsCount)
            {
                _Buttons.RemoveRange(buttonsPressed.Length, buttonsCount - buttonsPressed.Length);
            }
        }
    }
}
