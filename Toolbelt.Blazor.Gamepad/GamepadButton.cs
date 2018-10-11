namespace Toolbelt.Blazor.Gamepad
{
    /// <summary>
    /// An individual button of a gamepad or other controller.
    /// </summary>
    public class GamepadButton
    {
        /// <summary>
        /// A boolean value indicating whether the button is currently pressed (true) or unpressed (false).
        /// </summary>
        public bool Pressed { get; internal set; }

        /// <summary>
        /// A double value used to represent the current state of analog buttons, such as the triggers on many modern gamepads. 
        /// <para>The values are normalized to the range 0.0 —1.0, with 0.0 representing a button that is not pressed, and 1.0 representing a button that is fully pressed.</para>
        /// </summary>
        public double Value { get; internal set; }

        internal GamepadButton()
        {
        }
    }
}
