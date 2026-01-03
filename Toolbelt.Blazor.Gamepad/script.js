const _getGamepads = () => navigator.getGamepads();
export const getGamepads = () => {
    return _getGamepads()
        .filter(g => g !== null)
        .map(g => [g.id, g.index.toString()]);
};
export const refresh = (gamepadObjRef, id, index) => {
    const gamepad = _getGamepads().filter(gamepad => gamepad?.id === id && gamepad.index === index)[0];
    gamepadObjRef.invokeMethodAsync("UpdateStatus", gamepad?.connected ?? false, gamepad?.axes ?? [], gamepad?.buttons.map(b => b.pressed) ?? [], gamepad?.buttons.map(b => b.value) ?? []);
};
