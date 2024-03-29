"use strict";
var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var Gamepad;
        (function (Gamepad) {
            const searchParam = document.currentScript?.getAttribute('src')?.split('?')[1] || '';
            Gamepad.ready = import('./script.module.min.js?' + searchParam).then(m => {
                Object.assign(Gamepad, m.Toolbelt.Blazor.Gamepad);
            });
        })(Gamepad = Blazor.Gamepad || (Blazor.Gamepad = {}));
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
