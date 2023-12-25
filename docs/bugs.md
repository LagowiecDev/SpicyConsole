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

## Reporting Bugs

If you encounter any issues or bugs while using SpicyConsole, we appreciate your help in improving the package. Follow these steps to report bugs on our [GitHub repository](https://github.com/LagowiecDev/SpicyConsole):

1. **Check Existing Issues:**
   - Before reporting a new bug, check the [existing issues](https://github.com/LagowiecDev/SpicyConsole/issues) on GitHub to see if the problem has already been reported.

2. **Create a GitHub Account:**
   - If you don't already have one, create a GitHub account.

3. **Open a New Issue:**
   - Navigate to the [Issues](https://github.com/LagowiecDev/SpicyConsole/issues) section of the GitHub repository.
   - Click on the "New Issue" button.

4. **Provide Detailed Information:**
   - In the issue description, provide detailed information about the bug, including steps to reproduce it.
   - Specify the version of SpicyConsole you are using.

5. **Include Screenshots or Code Snippets:**
   - If applicable, include screenshots or code snippets that help illustrate the issue.

6. **Tag the Bug:**
   - Use appropriate labels to tag the issue, such as "bug" and any other relevant labels.

7. **Submit the Issue:**
   - Click on the "Submit new issue" button to create the bug report.

### Example Bug Report:
```md title="Bug Report Example"
Bug Description:
[Describe the bug in detail]

Steps to Reproduce:

1. [Step 1]
2. [Step 2]
3. [Step 3]

Expected Behavior:
[Describe what you expected to happen]

Actual Behavior:
[Describe what actually happened]

Version:
[Specify the version of SpicyConsole you are using]

Screenshots:
[Include any relevant screenshots]

Additional Information:
[Any other information that might be helpful]
```

Your bug report will help us identify and resolve issues more efficiently. We appreciate your contribution to making SpicyConsole better for everyone!