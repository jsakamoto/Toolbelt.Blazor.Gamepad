# Blazor Gamepad [![NuGet Package](https://img.shields.io/nuget/v/Toolbelt.Blazor.Gamepad.svg)](https://www.nuget.org/packages/Toolbelt.Blazor.Gamepad/)

## Summary

This is a class library that provides gamepad API access for your Blazor apps.

- _**Live Demo Site:**_ https://jsakamoto.github.io/Toolbelt.Blazor.Gamepad/

![demo movie](https://raw.githubusercontent.com/jsakamoto/Toolbelt.Blazor.Gamepad/master/.assets/movie-001.gif)

## Supported .NET versions

.NET version        | Blazor Gamepad version
--------------------|---------------------
.NET 8.0, 9.0, 10.0 | v.10.x
.NET 6.0, 7.0       | v.9.x
.NET 5.0 or earlier | v.8.x or earlier

All hosting models, such as Blazor WebAssembly and Blazor Server, are supported.

## How to install and use?

### 1. Installation and Registration

**Step.1** Install the library via NuGet package, like this.

```shell
> dotnet add package Toolbelt.Blazor.Gamepad
```

**Step.2** Register "GamepadList" service into the DI container.

```csharp
// Program.cs
...
using Toolbelt.Blazor.Extensions.DependencyInjection; // <- Add this line, and...
...
var builder = WebAssemblyHostBuilder.CreateDefault(args);
...
builder.Services.AddGamepadList(); // <- Add this line.
...
```

### 2. Usage in your Blazor component (.razor)

**Step.1** Inject the `GamepadList` service into the component.

```html
@inject Toolbelt.Blazor.Gamepad.GamepadList GamepadList @* <- Add this. *@
...
```

**Step.2** Invoke `GetGamepadsAsync()` async method to retreive gamepad list, and find a active gamepad object.

After you find it, you can reference gamepad axes and buttons.

_**Note**:_ _`GetGamepadsAsync()` returns empty list until any gamepad devices are activated. To activate the gamepad, you should do any actions on the gamepad device while the browser's document has focus._

Sample .razor code is here:

```csharp
@page "/"
@using Toolbelt.Blazor.Gamepad
@using System.Timers
@implements IDisposable
@inject GamepadList GamePadList

@if (this.Gamepad != null) {
  <p>Axes</p>
  <ul>
    @foreach (var ax in _gamepad.Axes) {
      <li>@ax.ToString("#,0.0")</li>
    }
  </ul>

  <p>Buttons</p>
  <ul>
    @foreach (var button in _gamepad.Buttons) {
      <li>@button.Pressed (@button.Value)</li>
    }
  </ul>
}

@code {

  private Gamepad? _gamepad;

  private readonly System.Timers.Timer _timer = new Timer(200) { Enabled = true };

  protected override void OnAfterRender(bool firstRender) {
    if (firstRender) {
      _timer.Elapsed += timer_Elapsed;
    }
  }

  private async void timer_Elapsed(object sender, EventArgs args) {
    var gamepads = await GamePadList.GetGamepadsAsync();
    _gamepad = gamepads.FirstOrDefault();
    if (_gamepad != null) 
      await this.InvokeAsync(() => this.StateHasChanged());
  }

  public void Dispose() {
    _timer.Elapsed -= timer_Elapsed;
    _timer.Dispose();
  }
}
```

## JavaScript file cache busting

This library includes and uses a JavaScript file to access the browser's Gamepad API. When you update this library to a newer version, the browser may use the cached previous version of the JavaScript file, leading to unexpected behavior. To prevent this issue, the library appends a version query string to the JavaScript file URL when loading it.

### .NET 8 and 9

A version query string will always be appended to this library's JavaScript file URL regardless of the Blazor hosting model you are using.

### .NET 10 or later

By default, a version query string will be appended to the JavaScript file URL that this library loads. If you want to disable appending a version query string to the JavaScript file URL, you can do so by setting the `TOOLBELT_BLAZOR_GAMEPAD_JSCACHEBUSTING` environment variable to `0`.

```csharp
// Program.cs
...
// 👇 Add this line to disable appending a version query string for this library's JavaScript file.
Environment.SetEnvironmentVariable("TOOLBELT_BLAZOR_GAMEPAD_JSCACHEBUSTING", "0");

var builder = WebApplication.CreateBuilder(args);
...
```

**However,** when you publish a .NET 10 Blazor WebAssembly app, a version query string will always be appended to the JavaScript file URL regardless of the `TOOLBELT_BLAZOR_GAMEPAD_JSCACHEBUSTING` environment variable setting. The reason is that published Blazor WebAssembly standalone apps don't include import map entries for JavaScript files from NuGet packages. If you want to avoid appending a version query string to the JavaScript file URL in published Blazor WebAssembly apps, you need to set the `ToolbeltBlazorGamepadJavaScriptCacheBusting` MSBuild property to `false` in the project file of the Blazor WebAssembly app, like this:

```xml
<PropertyGroup>
  <ToolbeltBlazorGamepadJavaScriptCacheBusting>false</ToolbeltBlazorGamepadJavaScriptCacheBusting>
</PropertyGroup>
```

### Why do we append a version query string to the JavaScript file URL regardless of whether the import map is available or not?

We know that .NET 9 or later allows us to use import maps to import JavaScript files with a fingerprint in their file names. Therefore, in .NET 9 or later Blazor apps, you may want to avoid appending a version query string to the JavaScript file URL that this library loads.

However, we recommend keeping the default behavior of appending a version query string to the JavaScript file URL. The reason is that published Blazor WebAssembly standalone apps don't include import map entries for JavaScript files from NuGet packages. This inconsistent behavior between development and production environments and hosting models may lead to unexpected issues that are hard to diagnose, particularly in AutoRender mode apps.

## Release Notes

The release notes is [here.](https://github.com/jsakamoto/Toolbelt.Blazor.Gamepad/blob/master/RELEASE-NOTES.txt)

## License

[Mozilla Public License Version 2.0](https://github.com/jsakamoto/Toolbelt.Blazor.Gamepad/blob/master/LICENSE)