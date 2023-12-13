export var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var Gamepad;
        (function (Gamepad) {
            const _getGamepads = () => navigator.getGamepads();
            function getGamepads() {
                return _getGamepads()
                    .filter(g => g !== null)
                    .map(g => [g.id, g.index.toString()]);
            }
            Gamepad.getGamepads = getGamepads;
            function refresh(gamepadObjRef, id, index) {
                const gamepad = _getGamepads().filter(gamepad => gamepad?.id === id && gamepad.index === index)[0];
                gamepadObjRef.invokeMethodAsync("UpdateStatus", gamepad?.connected ?? false, gamepad?.axes ?? [], gamepad?.buttons.map(b => b.pressed) ?? [], gamepad?.buttons.map(b => b.value) ?? []);
            }
            Gamepad.refresh = refresh;
        })(Gamepad = Blazor.Gamepad || (Blazor.Gamepad = {}));
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
