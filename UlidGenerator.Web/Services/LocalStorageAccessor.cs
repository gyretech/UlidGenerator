using Microsoft.JSInterop;

namespace UlidGenerator.Web.Services;

public sealed class LocalStorageAccessor(IJSRuntime jSRuntime) : IAsyncDisposable
{
    private Lazy<IJSObjectReference> _accessorJsRef = new();

    private async Task WaitForReference()
    {
        if(_accessorJsRef.IsValueCreated is false)
        {
            _accessorJsRef = new(await jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/storage.js"));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if(_accessorJsRef.IsValueCreated)
        {
            await _accessorJsRef.Value.DisposeAsync();
        }
    }

    public async Task<T> GetValueAsync<T>(string key)
    {
        await WaitForReference();

        return await _accessorJsRef.Value.InvokeAsync<T>("get", key);
    }

    public async Task SetValueAsync<T>(string key, T value)
    {
        await WaitForReference();

        await _accessorJsRef.Value.InvokeVoidAsync("set", key, value);
    }

    public async Task RemoveItemAsync(string key)
    {
        await WaitForReference();

        await _accessorJsRef.Value.InvokeVoidAsync("remove", key);
    }

    public async Task ClearAsync()
    {
        await WaitForReference();

        await _accessorJsRef.Value.InvokeVoidAsync("clear");
    }
}
