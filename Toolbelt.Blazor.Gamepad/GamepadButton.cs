using System.ComponentModel;

namespace Toolbelt.Blazor.Gamepad
{
    public class GamepadButton
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool _Pressed;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public double _Value;

        public bool Pressed => _Pressed;

        public double Value => _Value;
    }
}
