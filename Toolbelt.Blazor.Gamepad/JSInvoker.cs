using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace Toolbelt.Blazor.Gamepad;

internal class JSInvoker : IAsyncDisposable
{
    private readonly IJSRuntime JSRuntime;

    private readonly IJSObjectReference? JSModule;

    public JSInvoker(IJSRuntime jSRuntime, IJSObjectReference? jsModule)
    {
        this.JSRuntime = jSRuntime;
        this.JSModule = jsModule;
    }

    public ValueTask<T> InvokeAsync<[DynamicallyAccessedMembers(PublicConstructors | PublicFields | PublicProperties)] T>(string identifier, params object[]? args)
    {
        if (this.JSModule != null)
            return this.JSModule.InvokeAsync<T>(identifier, args);
        else
            return this.JSRuntime.InvokeAsync<T>(identifier, args);
    }

    public async ValueTask DisposeAsync()
    {
        if (this.JSModule != null)
        {
            try { await this.JSModule.DisposeAsync(); } catch (JSDisconnectedException) { }
        }
    }
}
