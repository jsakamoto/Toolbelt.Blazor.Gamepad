var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var Gamepad;
        (function (Gamepad) {
            let _gamepadListWrappers = [];
            function attach(gamepadListWrapper) {
                _gamepadListWrappers.push(gamepadListWrapper);
            }
            Gamepad.attach = attach;
            function getGamepads() {
                return Array.from(navigator.getGamepads())
                    .filter(g => g != null)
                    .map(g => ({ id: g.id, index: g.index }));
            }
            Gamepad.getGamepads = getGamepads;
            function refresh(id, index) {
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
            Gamepad.refresh = refresh;
        })(Gamepad = Blazor.Gamepad || (Blazor.Gamepad = {}));
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
//# sourceMappingURL=script.js.map