namespace Toolbelt.Blazor.Gamepad {
    const searchParam = document.currentScript?.getAttribute('src')?.split('?')[1] || '';
    var r = import('./script.module.min.js?' + searchParam).then(m => {
        Object.assign(Gamepad, m.Toolbelt.Blazor.Gamepad);
    });
    export function ready(): Promise<void> { return r; }
}