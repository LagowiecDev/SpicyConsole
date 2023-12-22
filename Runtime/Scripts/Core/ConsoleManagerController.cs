using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using TMPro;
using SpicyConsole.Setup;

namespace SpicyConsole
{
    public class ConsoleManagerController : MonoBehaviour
    {

        public static ConsoleManagerController Instance;

        [SerializeField] private TMP_Text consoleOutput; // Make it public
        [SerializeField] private TMP_Text suggestionsText; // Reference to the TMP_Text for suggestions

        [SerializeField] private GameObject consoleObject;

        string[] splitedCommand;
        string[] parametersCommand;
        List<string> allLogs = new List<string>();

        bool isListening;

        string allParams;

        // A list to store all commands with SpicyCommand attribute

        private Dictionary<string,string[]> spicyCommands = new Dictionary<string,string[]>();

        void Start()
        {
            if (consoleOutput != null)
            {
                consoleOutput.text = string.Empty;
            }
            else
            {
                this.enabled = false;
                Debug.LogError("ConsoleOutput TMP_Text not assigned. Disabling ConsoleManagerController.");
            }

            if (suggestionsText != null)
            {
                suggestionsText.text = string.Empty;
            }
            else
            {
                Debug.LogError("SuggestionsText TMP_Text not assigned.");
            }

            // Populate the list of commands with SpicyCommand attribute on Start
            Console_Enable(true);
        }
        
        void Update()
        {
            if (isListening)
            {

                consoleOutput.text = string.Empty;

                foreach (string _log in allLogs)
                {
                    consoleOutput.text += _log + "\n";
                }
            }
        }

        [SpicyCommand("spicy_console_visible")]
        private void Console_Enable(bool isEnabled) {
            
            if (isEnabled)
            {
                GetAllSpicyCommands();

                if (!isListening)
                {
                    Application.logMessageReceived += DebugListener;
                    isListening = true;
                }          

                consoleObject.SetActive(true);
            } else {

                Application.logMessageReceived -= DebugListener;

                isListening = false;

                consoleObject.SetActive(false);
            }
        }

        private void DebugListener(string logString, string stackTrace, LogType type)
        {
            LogConverter(logString,"",type.ToString());
        }

        private void LogConverter(string logString, string stackTrace = "", string _type = "Log")
        {

            LogType type = (LogType) Enum.Parse(typeof(LogType),_type,true);

            switch (type)
            {
                case LogType.Error:
                    allLogs.Add($"<color=red>{logString}</color>");
                break;
                case LogType.Warning:
                    allLogs.Add($"<color=yellow>{logString}</color>");
                break;
                case LogType.Log:
                    allLogs.Add($"{logString}");
                break;
                case LogType.Assert:
                    allLogs.Add($"<b>{logString}/b>");
                break;
            }
        }

        // This method gets all methods with SpicyCommand attribute and populates the list
        private void GetAllSpicyCommands()
        {

            Assembly[] allAssembles = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly currentAssembly in allAssembles)
            {
                Type[] types = currentAssembly.GetTypes();
                foreach (Type type in types)
                {
                    MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                    foreach (MethodInfo method in methods)
                    {
                        SpicyCommandAttribute attribute = method.GetCustomAttribute<SpicyCommandAttribute>();
                        if (attribute != null)
                        {
                            List<string> parametersNamesAndType = new List<string>();
                            foreach (ParameterInfo parameter in method.GetParameters())
                            {
                                parametersNamesAndType.Add($"| {parameter.Name} [{parameter.ParameterType.Name}] |");
                            }
                            spicyCommands.Add(attribute.CustomName,parametersNamesAndType.ToArray());
                        }
                    }
                }
            }
        }

        // RANDOM IMPLEMATION
        public void GetAllSuggestions(string writed)
        {
            if (!string.IsNullOrEmpty(writed))
            {
                DisplayBoldSuggestions(writed);
            }
        }

        // This method displays suggestions in the suggestionsText TMP_Text with typed characters bold
        private void DisplayBoldSuggestions(string typedCharacters)
        {
            string[] _array_typeCharacters = typedCharacters.Split(' ');
            if (suggestionsText != null)
            {
                // Replace the typed characters with bolded characters in the suggestion
                string boldedSuggestion = SuggestCommand(_array_typeCharacters).Replace(_array_typeCharacters[0], $"<color=#FFFFFF><b>{_array_typeCharacters[0]}</b></color>");
                suggestionsText.text = boldedSuggestion;
            }
            else
            {
                Debug.LogError("SuggestionsText TMP_Text not assigned.");
            }
        }

        private string SuggestCommand(string[] typedCharacter)
        {
            // Filter commands starting with the typed character
            var matchingCommands = spicyCommands.Where(command => command.Key.StartsWith(typedCharacter[0], StringComparison.OrdinalIgnoreCase));

            string _commandParams = string.Empty;
            List<string> _commands = new List<string>();

            foreach (KeyValuePair<string, string[]> command in matchingCommands)
            {
                var typedParams = typedCharacter.Skip(1).ToArray();

                for (int i = 0; i < command.Value.Length; i++)
                {
                    if (i < typedParams.Length && typedParams[i] != null)
                    {
                        _commandParams += ($" <color=#FFFFFF><b>{command.Value[i]}</b></color>");
                    }
                    else
                    {
                        _commandParams += $" {command.Value[i]}";
                    }
                }

                _commands.Add($"{command.Key} {_commandParams}");
            }

            // Create a suggestion string with the typed character bold
            string suggestion = string.Join(", ", _commands);

            return suggestion;
        }

        // This method executes the command
        public void ExecuteCommand(string fullCommand)
        {
            splitedCommand = fullCommand.Split(' ');

            if (splitedCommand.Length < 1)
            {
                Debug.LogError("Invalid command format: " + fullCommand);
                return;
            }

            string commandName = splitedCommand[0];

            if (string.IsNullOrEmpty(commandName))
            {
                Debug.LogError("Command name is empty.");
                return;
            }

            Debug.Log($"Executing command: {fullCommand}");

            parametersCommand = splitedCommand.Skip(1).ToArray();

            Assembly[] allAssembles = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly currentAssembly in allAssembles)
            {

                Type[] types = currentAssembly.GetTypes();

                foreach (Type type in types)
                {

                    if (!spicyCommands.ContainsKey(commandName))
                    {
                        Debug.LogError($"Command '{commandName}' not found!");
                        return;
                    }

                    MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                    foreach (MethodInfo method in methods)
                    {
                        SpicyCommandAttribute attribute = method.GetCustomAttribute<SpicyCommandAttribute>();
                        if ((attribute != null) && (attribute.CustomName == commandName))
                        {
                            ParameterInfo[] methodParams = method.GetParameters();

                            allParams = string.Empty;

                            foreach (ParameterInfo parameter in methodParams)
                            {
                                allParams += $"'[{parameter.Name} {parameter.ParameterType.Name}]' ";
                            }

                            if (methodParams.Length == parametersCommand.Length)
                            {
                                object instance = FindObjectOfType(type);

                                if (instance == null)
                                {
                                    Debug.LogError($"Instance of type {type} not found.");
                                    return;
                                }

                                object[] convertedParameters = ConvertParameters(parametersCommand, methodParams);

                                if (convertedParameters != null)
                                {
                                    try
                                    {
                                        Debug.Log($"Invoking method: {method.Name}");
                                        foreach (object obj in convertedParameters)
                                        {
                                            Debug.Log($"{obj.GetType()} {obj.ToString()}");
                                        }
                                        method.Invoke(instance, convertedParameters);
                                        Debug.Log($"Command '{commandName}' executed successfully.");
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.LogError($"Error executing command '{commandName}': {ex.Message}");
                                    }
                                    return;
                                }
                                else
                                {
                                    Debug.LogError($"Error converting parameters for command '{commandName} {allParams}'.");
                                }
                            }
                            else
                            {
                                Debug.LogError($"Incorrect number of parameters for command '{commandName} {allParams}'.");
                            }
                        }
                    }
                }
            }
        }

        private object[] ConvertParameters(string[] parameters, ParameterInfo[] methodParams)
        {
            if (parameters.Length != methodParams.Length)
            {
                Debug.LogError($"Incorrect number of parameters. Expected {methodParams.Length}, but got {parameters.Length}.");
                return null;
            }

            object[] convertedParameters = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                Type paramType = methodParams[i].ParameterType;

                try
                {
                    if (parameters[i].StartsWith("{") && parameters[i].EndsWith("}"))
                    {
                        string _conv0 = parameters[i].Replace('}','{').Split('{')[1];
                        string[] _conv_array = _conv0.Split(':');

                        switch (_conv_array[0].ToLower())
                        {
                            case "array":
                                if (_conv_array[1].StartsWith("(") && _conv_array[1].EndsWith(")"))
                                {
                                    string _conv1 = _conv_array[1].Replace(')','(').Split('(')[1];
                                    string[] _array = _conv1.Split(',');

                                    convertedParameters[i] = _array;
                                }
                            break;
                            case "list":
                                if (_conv_array[1].StartsWith("(") && _conv_array[1].EndsWith(")"))
                                {
                                    string _conv1 = _conv_array[1].Replace(')','(').Split('(')[1];
                                    string[] _array = _conv1.Split(',');

                                    List<string> _list = new List<string>(_array);

                                    convertedParameters[i] = Convert.ChangeType(_list,paramType);
                                }
                            break;
                            case "dictionary":
                                if (_conv_array[1].StartsWith("(") && _conv_array[1].EndsWith(")"))
                                {
                                    string _conv1 = _conv_array[1].Replace(')','(').Split('(')[1];
                                    string[] _array = _conv1.Split(',');

                                    Dictionary<string,string> _dictionary = new Dictionary<string, string>();

                                    foreach (string _str in _array)
                                    {
                                        string _conv2 = _str.Replace('[',']').Split(']')[1];
                                        string[] _array2 = _conv2.Split(';');
                                        _dictionary.Add(_array2[0],_array2[1]);
                                    }

                                    convertedParameters[i] = Convert.ChangeType(_dictionary,paramType);
                                }
                            break;
                        }
                    } else {
                        convertedParameters[i] = Convert.ChangeType(parameters[i], paramType);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error converting parameter {i + 1} to type {paramType}: {ex.Message}");
                    return null;
                }
            }

            return convertedParameters;
        }
    }
}