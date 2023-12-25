# SpicyConsole Documentation

## Introduction

Welcome to the documentation for the SpicyConsole Unity package! SpicyConsole is a versatile console system that allows you to easily create and manage console commands in your Unity projects.

## Installation

### Installing SpicyConsole from UnityPackage

1. Go to the [Releases](https://github.com/LagowiecDev/SpicyConsole/releases) section of the [SpicyConsole GitHub repository](https://github.com/LagowiecDev/SpicyConsole).

2. Download the latest UnityPackage file available.

3. Open your Unity project.

4. In the Unity Editor, go to `Assets` > `Import Package` > `Custom Package...`.

5. Select the downloaded UnityPackage file.

6. In the Import Unity Package window, review the contents and click `Import` to add the SpicyConsole assets to your project.

## Quick Start

### Creating a Command

To create a console command, use the `SpicyCommand` attribute on a method in any script. The following example demonstrates how to create a simple command named "command_custom_name":

```csharp
using SpicyConsole.Setup;

public class YourClass : MonoBehaviour
{
    [SpicyCommand("command_custom_name")]
    private void Example(string x)
    {
        Debug.Log(x);
    }
}
```

Now, when you type "command_custom_name" in the console during runtime, the `Example` method will be executed, logging the provided parameter.

### Note: Script Instance in Scene

For SpicyConsole to function correctly, ensure that your custom script, such as `YourClass`, is attached to a GameObject in your scene during runtime. This custom script instance is crucial for initializing and managing the SpicyConsole system.

## Command Syntax

The `SpicyCommand` attribute takes a string parameter, which represents the name of the command. The associated method should have the following characteristics:

- It must be a non-static method.
- It can have parameters, which will be filled by the console command arguments.

## Additional Features

SpicyConsole offers additional features to enhance your console experience. Explore the documentation to learn about:

- Parameter types and parsing.
- Command aliases.
- Command descriptions and help messages.
- Customizing the console appearance.

## Conclusion

With SpicyConsole, managing and creating console commands in your Unity project becomes a breeze. Experiment with the provided features and make your game development experience even more enjoyable!

For detailed information on each feature, refer to the specific sections in this documentation. If you encounter any issues or have questions, feel free to reach out to the SpicyConsole community or the package developer. Happy coding!