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

```cs title="YourClass.cs" linenums="1"

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

Now, when you type `command_custom_name ExampleMSG` in the console during runtime, the `Example()` method will be executed, logging `ExampleMSG` as the provided parameter.

### Note: Script Instance in Scene

For SpicyConsole to function correctly, ensure that your custom script, such as `YourClass`, is attached to a GameObject in your scene during runtime. This custom script instance is crucial for initializing and managing the SpicyConsole system.

## Command Syntax

The `SpicyCommand` attribute takes a string parameter, which represents the name of the command. The associated method should have the following characteristics:

- It must be a non-static method.
- It can have parameters, which will be filled by the console command arguments.

## Additional Features

SpicyConsole offers additional features to enhance your console experience. Explore the documentation to learn about:

### Commands List

#### Automatic Commands List Generation

SpicyConsole provides a convenient feature that automatically generates a list of all commands marked with the `SpicyCommand` attribute during every C# script compilation in Unity. This list is then stored as an asset in the following path: `Assets/Settings/ThirdParty/LagowiecDev/SpicyConsole/CommandsList.asset`
The generated `CommandsList.asset` file contains information about each command, including its name, associated method, and any additional attributes specified in the script.

##### How It Works

1. During the compilation of C# scripts in Unity, SpicyConsole scans for methods with the `SpicyCommand` attribute.

2. The information about these commands is collected and compiled into the `CommandsList.asset` file.

3. The asset file is automatically saved in the specified directory for easy access and reference.

##### Accessing the Commands List

You can access the generated command list asset in the Unity Editor by navigating to the following path: `Assets/Settings/ThirdParty/LagowiecDev/SpicyConsole/CommandsList.asset`

#### Manual Commands List Retrieval

SpicyConsole provides a manual option to retrieve a list of all commands marked with the `SpicyCommand` attribute directly from the Unity Editor's top bar.

Where you can find the function: `ThirdParty/LagowiecDev/SpicyConsole/Get All Commands`

1. In the Unity Editor, navigate to the top bar where you find options like "File," "Edit," and others.

2. Look for the "ThirdParty" menu.

3. Under "ThirdParty," find and click on "LagowiecDev."

4. Within the "LagowiecDev" menu, locate and click on "SpicyConsole."

5. From the dropdown menu, select "Get All Commands."

6. SpicyConsole will generate a list of all commands and display it in the Unity Console or another designated location.

This manual option is useful when you want to explicitly trigger the command list retrieval process. It can be particularly handy for scenarios where automatic compilation may not have occurred, or you want to refresh the command list on-demand.

Feel free to use this option to conveniently access and review the commands available in your SpicyConsole-enabled project!

### Other

- Parameter types and parsing.
- Command aliases.
- Command descriptions and help messages.
- Customizing the console appearance.

## Conclusion

With SpicyConsole, managing and creating console commands in your Unity project becomes a breeze. Experiment with the provided features and make your game development experience even more enjoyable!

For detailed information on each feature, refer to the specific sections in this documentation. If you encounter any issues or have questions, feel free to reach out to the SpicyConsole community or the package developer. Happy coding!