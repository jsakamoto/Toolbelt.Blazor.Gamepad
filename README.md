# Blazor Gamepad [![NuGet Package](https://img.shields.io/nuget/v/Toolbelt.Blazor.Gamepad.svg)](https://www.nuget.org/packages/Toolbelt.Blazor.Gamepad/)

## Summary

This is a class library that provides gamepad API access for your Blazor apps.

- _**Live Demo Site:**_ https://jsakamoto.github.io/Toolbelt.Blazor.Gamepad/

![demo movie](https://raw.githubusercontent.com/jsakamoto/Toolbelt.Blazor.Gamepad/master/.assets/movie-001.gif)

## Requirements

"Blazor Gamepad" ver.9.x or later supports Blazor versions below.

- v.6.0, 7.0, 8.0 or later

> **Note:**  
> If you are using Blazor version 5.0 or earlier, please use "Blazor Gamepad" ver.8.x or earlier.

Both "Blazor WebAssembly App" and "Blazor Server App" are supoorted.

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

  protected override void OnInitialized() {
    _timer.Elapsed += timer_Elapsed;
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

## Configuration options

The calling of `AddGamepadList()` injects the reference of the helper JavaScript file (.js) - which are bundled with this package - into your page automatically.

If you don't want this behavior, you can disable the automatic injections. 
To do that, please call `AddGamepadList()` with configuration action like this:

```csharp
builder.Services.AddGamepadList(options =>
{
  // If you don't want automatic injection of js file, add below;
  options.DisableClientScriptAutoInjection = true;
});
```

You can inject the helper JavaScript file manually. The URL of that JavaScript file is below:

- `_content/Toolbelt.Blazor.Gamepad/script.min.js`

## Release Notes

The release notes is [here.](https://github.com/jsakamoto/Toolbelt.Blazor.Gamepad/blob/master/RELEASE-NOTES.txt)

## License

[Mozilla Public License Version 2.0](https://github.com/jsakamoto/Toolbelt.Blazor.Gamepad/blob/master/LICENSE)