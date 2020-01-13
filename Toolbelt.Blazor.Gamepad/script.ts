namespace Toolbelt.Blazor.Gamepad {

    export function getGamepads(): string[][] {
        return Array.from(navigator.getGamepads())
            .filter(g => g != null)
            .map(g => [g.id, g.index.toString()]);
    }

    export function refresh(gamepadObjRef: any, id: string, index: number): null {
        for (var gamepad of navigator.getGamepads()) {
            if (gamepad != null && gamepad.id == id && gamepad.index == index) {
                gamepadObjRef.invokeMethodAsync("UpdateStatus", gamepad.connected, gamepad.axes, gamepad.buttons.map(b => b.pressed), gamepad.buttons.map(b => b.value));
                return null;
            }
        }
        gamepadObjRef.invokeMethodAsync("UpdateStatus", false, [], [], []);
        return null;
    }
}