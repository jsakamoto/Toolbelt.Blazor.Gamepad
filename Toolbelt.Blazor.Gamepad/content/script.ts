declare var DotNet: any;

namespace Toolbelt.Blazor.Gamepad {

    let _gamepadListWrappers: any[] = [];

    export function attach(gamepadListWrapper: any): void {
        _gamepadListWrappers.push(gamepadListWrapper);
    }

    export function getGamepads(): { id: string, index: number }[] {
        return Array.from(navigator.getGamepads())
            .filter(g => g != null)
            .map(g => ({ id: g.id, index: g.index }));
    }

    export function refresh(id: string, index: number): void {
        const gamepad = Array.from(navigator.getGamepads())
            .filter(g => g != null)
            .filter(g => g.id == id && g.index == index)
            .pop();
        const args = (typeof gamepad != "undefined") ? {
            connected: gamepad.connected,
            axes: gamepad.axes,
            buttons: gamepad.buttons.map(btn => ({ _pressed: btn.pressed, _value: btn.value }))
        } : { connected: false, axes: [], buttons: [] };
        _gamepadListWrappers.forEach(wrapper => wrapper.invokeMethodAsync("UpdateGamepad", id, index, args.connected, args.axes, args.buttons));
    }
}