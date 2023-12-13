using System.ComponentModel;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.Gamepad;

/// <summary>
/// An individual gamepad or other controller device.
/// </summary>
public class Gamepad
{
    private readonly JSInvoker JSInvoker;

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

    private readonly List<GamepadButton> _Buttons = new List<GamepadButton>();

    /// <summary>
    /// A list of GamepadButton objects representing the buttons present on the device.
    /// </summary>
    public IReadOnlyList<GamepadButton> Buttons => this.Refresh()._Buttons;

    private ValueTask<object> LastRefreshTask = default;

    private DotNetObjectReference<Gamepad> _ObjectRefOfThis;

    internal Gamepad(JSInvoker jsInvoker, string id, int index, bool connected)
    {
        this.JSInvoker = jsInvoker;
        this.Id = id;
        this.Index = index;
        this._Connected = connected;
    }

    private Gamepad Refresh()
    {
        if (this.LastRefreshTask.IsCompleted)
        {
            if (this._ObjectRefOfThis == null) this._ObjectRefOfThis = DotNetObjectReference.Create(this);
            this.LastRefreshTask = this.JSInvoker.InvokeAsync<object>("Toolbelt.Blazor.Gamepad.refresh", this._ObjectRefOfThis, this.Id, this.Index);
        }
        return this;
    }

    [JSInvokable(nameof(UpdateStatus)), EditorBrowsable(EditorBrowsableState.Never)]
    public void UpdateStatus(bool connected, double[] axes, bool[] buttonsPressed, double[] buttonsValue)
    {
        this._Connected = connected;
        this._Axes = axes;

        var buttonsCount = this._Buttons.Count;
        for (var i = 0; i < buttonsPressed.Length; i++)
        {
            var button = default(GamepadButton);
            if (i < buttonsCount)
            {
                button = this._Buttons[i];
            }
            else
            {
                button = new GamepadButton();
                this._Buttons.Add(button);
            }
            button.Pressed = buttonsPressed[i];
            button.Value = buttonsValue[i];
        }
        if (buttonsPressed.Length < buttonsCount)
        {
            this._Buttons.RemoveRange(buttonsPressed.Length, buttonsCount - buttonsPressed.Length);
        }
    }
}
