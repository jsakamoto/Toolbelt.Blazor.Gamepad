# Blazor Gamepad [![NuGet Package](https://img.shields.io/nuget/v/Toolbelt.Blazor.Gamepad.svg)](https://www.nuget.org/packages/Toolbelt.Blazor.Gamepad/)

## Summary

This is a class library that provides gamepad API access for your Blazor apps.

## Requirements

"Blazor Gamepad" ver.7.x supports Blazor versions below.

- v.3.1
    - including previews and release candidates.
- v.3.2
    - including previews and release candidates.
- v.5.0
    - including previews and release candidates.

Both "Blazor WebAssembly App" (a.k.a."Client-side Blazor") and "Blazor Server App" (a.k.a."Server-side Blazor") are supoorted.

## How to install and use?

### 1. Installation and Registration

**Step.1** Install the library via NuGet package, like this.

```shell
> dotnet add package Toolbelt.Blazor.Gamepad
```

**Step.2** Register "GamepadList" service into the DI container.

If the project is a Blazor Server App or a Blazor WebAssembly App ver.3.1 Preview 4 or earlyer, add the code into the `ConfigureService` method in the `Startup` class of your Blazor application.

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

If the project is a Blazor WebAssembly App ver.3.2 Preview 1 or later (includes .NET 5), add the code into the `Main` method in the `Program` class of your Blazor application.

```csharp
// Program.cs

using Toolbelt.Blazor.Extensions.DependencyInjection; // <- Add this, and...
...
public class Program
{
  public static async Task Main(string[] args)
  {
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
    @foreach (var ax in this.Gamepad.Axes) {
      <li>@ax.ToString("#,0.0")</li>
    }
  </ul>

  <p>Buttons</p>
  <ul>
    @foreach (var button in this.Gamepad.Buttons) {
      <li>@button.Pressed (@button.Value)</li>
    }
  </ul>
}

@code {

  Gamepad Gamepad;

  Timer Timer = new Timer(200) { Enabled = true };

  protected override void OnInitialized() {
    Timer.Elapsed += Timer_Elapsed;
  }

  async void Timer_Elapsed(object sender, EventArgs args) {
    var gamepads = await GamePadList.GetGamepadsAsync();
    this.Gamepad = gamepads.FirstOrDefault();
    if (this.Gamepads.Any()) 
      await this.InvokeAsync(() => this.StateHasChanged());
  }

  public void Dispose() {
    Timer.Elapsed -= Timer_Elapsed;
    Timer.Dispose();
  }
}
```

## Release Notes

The release notes is [here.](https://github.com/jsakamoto/Toolbelt.Blazor.Gamepad/blob/master/RELEASE-NOTES.txt)

## License

[Mozilla Public License Version 2.0](https://github.com/jsakamoto/Toolbelt.Blazor.Gamepad/blob/master/LICENSE)