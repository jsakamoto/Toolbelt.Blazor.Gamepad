"use strict";
var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var Gamepad;
        (function (Gamepad) {
            var _a, _b;
            const searchParam = ((_b = (_a = document.currentScript) === null || _a === void 0 ? void 0 : _a.getAttribute('src')) === null || _b === void 0 ? void 0 : _b.split('?')[1]) || '';
            var r = import('./script.module.min.js?' + searchParam).then(m => {
                Object.assign(Gamepad, m.Toolbelt.Blazor.Gamepad);
            });
            function ready() { return r; }
            Gamepad.ready = ready;
        })(Gamepad = Blazor.Gamepad || (Blazor.Gamepad = {}));
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
