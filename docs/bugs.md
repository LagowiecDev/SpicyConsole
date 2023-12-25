# Bugs & Issues

## Known Issue: CommandList Asset Reference

### Problem Description

There is a known bug in the current version of SpicyConsole where the field for the CommandList asset may not correctly reference the asset after initial import or update.

### Workaround

If you encounter this issue, follow these steps to manually set the correct reference for the CommandList asset:

1. In the Unity Editor, navigate to the project window.

2. Locate the `CommandsList.asset` file in the following directory:

    ```
    Assets/Settings/ThirdParty/LagowiecDev/SpicyConsole/CommandsList.asset
    ```

3. Select the `CommandsList.asset` file.

4. In the Inspector window, find the field that may not be correctly referencing the asset. It might be labeled as "Command List" or similar.

5. Click the small circle button or drag the `CommandsList.asset` file onto the field to set the correct reference.

### Note

Ensure that the `CommandsList.asset` file is not moved or renamed after setting the reference, as it may lead to the field losing its connection.

### Future Updates

We are actively working on resolving this bug, and a fix will be included in the upcoming updates of SpicyConsole. Keep an eye on my [GitHub repository](https://github.com/LagowiecDev/SpicyConsole) for the latest releases and bug fixes.

If you encounter any other issues or need further assistance, feel free to reach out to the SpicyConsole community or the package developer. Your feedback is valuable in improving the SpicyConsole experience for everyone.
