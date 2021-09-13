export var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var Gamepad;
        (function (Gamepad) {
            function getGamepads() {
                return Array.from(navigator.getGamepads())
                    .filter(g => g != null)
                    .map(g => [g.id, g.index.toString()]);
            }
            Gamepad.getGamepads = getGamepads;
            function refresh(gamepadObjRef, id, index) {
                for (var gamepad of navigator.getGamepads()) {
                    if (gamepad != null && gamepad.id == id && gamepad.index == index) {
                        gamepadObjRef.invokeMethodAsync("UpdateStatus", gamepad.connected, gamepad.axes, gamepad.buttons.map(b => b.pressed), gamepad.buttons.map(b => b.value));
                        return null;
                    }
                }
                gamepadObjRef.invokeMethodAsync("UpdateStatus", false, [], [], []);
                return null;
            }
            Gamepad.refresh = refresh;
        })(Gamepad = Blazor.Gamepad || (Blazor.Gamepad = {}));
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
