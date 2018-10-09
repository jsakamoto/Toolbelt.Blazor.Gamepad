# Blazor Gamepad [![NuGet Package](https://img.shields.io/nuget/v/Toolbelt.Blazor.Gamepad.svg)](https://www.nuget.org/packages/Toolbelt.Blazor.Gamepad/)

## Summary



## How to install and use?

### 1. Installation and Registration

**Step.1** Install the library via NuGet package, like this.

```shell
> dotnet add package Toolbelt.Blazor.Gamepad
```

**Step.2** Register "GamepadList" service into the DI container, at `ConfigureService` method in the `Startup` class of your Blazor application.

```csharp
using Toolbelt.Blazor.Extensions.DependencyInjection; // <- Add this line, and...
...
public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddGamepadList(); // <- Add this line.
    ...
```
### 2. Usage in your Blazor component (.cshtml)

**Step.1** Inject the `GamepadList` service into the component.

```html
@inject Toolbelt.Blazor.Gamepad.GamepadList GamepadList @* <- Add this. *@
...
```

**Step.2** Invoke `GetGamepadsAsync()` async method to retreive gamepad list, and find a active gamepad object.

After you find it, you can reference gamepad axes and buttons.

_**Note**:_ _`GetGamepadsAsync()` returns empty list until any gamepad devices are activated. To activate the gamepad, you should do any actions on the gamepad device while the browser's document has focus._

Sample .cshtml code is here:

```csharp
@page "/"
@using Toolbelt.Blazor.Gamepad
@inject GamepadList GamePadList

@if (this.Gamepad != null) {
  <p>Axes</p>
  <ul>
    @foreach (var ax in Axes) {
      <li>@ax.ToString("#,0.0")</li>
    }
  </ul>

  <p>Buttons</p>
  <ul>
    @foreach (var button in Buttons) {
      <li>@button.Pressed (@button.Value)</li>
    }
  </ul>
}

@functions {

  Gamepad Gamepad;

  protected override void OnInit() {
    WatchLoop();
  }

  async Task WatchLoop() {
    await Task.Delay(200);

    var gamepads = await GamePadList.GetGamepadsAsync();
    this.Gamepad = gamepads.FirstOrDefault();

    this.StateHasChanged();
    WatchLoop();
  }
}
```

## License

[Mozilla Public License Version 2.0](https://github.com/jsakamoto/Toolbelt.Blazor.Gamepad/blob/master/LICENSE)