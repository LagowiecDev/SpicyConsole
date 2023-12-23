using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using TMPro;
using SpicyConsole.Setup;
using SpicyConsole.Formatting;
using SpicyConsole.Formatting.BinarySerializing;

namespace SpicyConsole
{
    [System.Serializable]

    public class ConsoleManagerController : MonoBehaviour
    {

        public static ConsoleManagerController Instance; // For Referencing ConsoleManagerController Instance in Scene :)

        [Header("Settings")]

        [SerializeField] private bool isLoggingEnabled; // Enables Logging | Console will show Unity Logger Messages
        [SerializeField] private bool isSuggestionEnabled; // Enables Suggestions | 
        
        [SerializeField] private bool areConsoleMessagesEnabled; // Enables SpicyConsoleMessages | Console will log for eg Pakcage Error or other similar stuff

        [Header("Components")]

        [SerializeField] private TMP_Text consoleOutputText; // Reference to TMP_Text for Console Logs;
        [SerializeField] private TMP_Text consoleSuggestionsText; // Reference to the TMP_Text for suggestions

        // Optional:?
        [SerializeField] private GameObject consoleObject;

        //

        public SpicyCommands file;

        private Dictionary<string,MethodInfo> commandsDictionary = new Dictionary<string, MethodInfo>();

        private void Awake() {
            if (IsEnabledAndExists(isLoggingEnabled,consoleOutputText))
            {
                consoleOutputText.text = string.Empty;
            }

            if (IsEnabledAndExists(isSuggestionEnabled,consoleSuggestionsText))
            {
                consoleSuggestionsText.text = string.Empty;
            }
        }

        private void Start() {
            GetAllCommands();
        }

        public void ExecuteString(string fullCommand)
        {

            if (fullCommand != string.Empty)
            {
                string[] commandParts = fullCommand.Split(' ');

                if (commandsDictionary.ContainsKey(commandParts[0]))
                {
                    MethodInfo wantedCommand = commandsDictionary[commandParts[0]];

                    var type = wantedCommand.DeclaringType;

                    UnityEngine.Object instance = FindObjectOfType(type);
                    if (instance != null)
                    {
                        string[] parametersCommand = commandParts.Skip(1).ToArray();

                        if (parametersCommand.Count() <= wantedCommand.GetParameters().Count())
                        {
                            commandsDictionary[commandParts[0]].Invoke(instance,ConvertStringParamsToParams(parametersCommand,wantedCommand.GetParameters()));
                        } else {
                            SpicyMessage(LogType.Warning,$"Writed Size of Paramters is {parametersCommand.Count()} but wanted size is {wantedCommand.GetParameters().Count()}");
                        }
                    } else {
                        SpicyMessage(LogType.Warning,$"Command {commandParts[0]} need instance of {type}");
                    }
                } else {
                    SpicyMessage(LogType.Warning,($"Command {commandParts[0]} not found!"));
                }
            }
        }

        // Getting Paramters

        private UnityEngine.Object[] ConvertStringParamsToParams(string[] paramStrings, ParameterInfo[] parameterInfo)
        {

            List<UnityEngine.Object> convertedParameters = new List<UnityEngine.Object>();

            for (int i = 0; i < paramStrings.Count(); i++)
            {
                ParameterInfo param = parameterInfo[i];
                string stringParam = paramStrings[i];

                try {
                    convertedParameters.Add((UnityEngine.Object) Convert.ChangeType(stringParam,param.ParameterType));
                } catch (Exception ex)
                {
                    //convertedParameters.Add((UnityEngine.Object) param.DefaultValue);
                    SpicyMessage(LogType.Warning,ex.Message);
                }
            }

            return convertedParameters.ToArray();
        }

        // Getting commands

        private void GetAllCommands()
        {

            // TODO: REMOVE IT
            /*
            // 
            Assembly[] assemblies = GetAllAssemblies();
            Type[] types = GetAllTypesFromAssemblies(assemblies);
            MethodInfo[] methodInfos = GetMethodInfosFromTypes(types);

            //
            MethodInfo[] methodInfosWithAttribute = GetCommandsFromAllMethods(methodInfos);

            commandsDictionary = ConvertCommandsArrayIntoCommandsDictionary(methodInfosWithAttribute);
            */

            /*
            theoretically can use this:
            commands = GetCommandsFromAllMethods(GetMethodInfosFromTypes(GetAllTypesFromAssemblies(GetAllAssemblies())));
            */

            //
            commandsDictionary = ConvertCommandsArrayIntoCommandsDictionary(ByteConverter.SerializedMethodInfoArrayToInfoMethodArray(file.methodInfos));
        }

        // TODO: REMOVE IT
        /*

        private Assembly[] GetAllAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        private Type[] GetAllTypesFromAssemblies(Assembly[] assemblies)
        {
            List<Type> typesToReturn = new List<Type>();

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    typesToReturn.Add(type);
                }
            }

            return typesToReturn.ToArray();
        }

        private MethodInfo[] GetMethodInfosFromTypes(Type[] types)
        {

            List<MethodInfo> methodInfosToReturn = new List<MethodInfo>();

            foreach (Type type in types)
            {
                foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    methodInfosToReturn.Add(methodInfo);
                }
            }

            return methodInfosToReturn.ToArray();
        }

        private MethodInfo[] GetCommandsFromAllMethods(MethodInfo[] methodInfos)
        {
            List<MethodInfo> methodInfosWithAttributes = new List<MethodInfo>();

            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (true)
                {
                    methodInfosWithAttributes.Add(methodInfo);
                }
            }

            return methodInfosWithAttributes.ToArray();
        }

        */

        private Dictionary<string,MethodInfo> ConvertCommandsArrayIntoCommandsDictionary(MethodInfo[] methodInfos)
        {
            Dictionary<string,MethodInfo> keyValuePairs = new Dictionary<string,MethodInfo>();

            foreach (MethodInfo methodInfo in methodInfos)
            {
                SpicyCommandAttribute attribute = methodInfo.GetCustomAttribute<SpicyCommandAttribute>();
                string keyName = attribute.CustomName;
                //Debug.Log($"Adding: Dictionary(K: {keyName} V: {methodInfo.Name})");
                keyValuePairs.Add(keyName,methodInfo);
            }

            return keyValuePairs;
        }

        // Other

        private bool IsEnabledAndExists(bool isEnabled, UnityEngine.Object objectToCheck = null)
        {
            if (isEnabled && objectToCheck != null)
                return true;

            SpicyMessage(LogType.Warning, $"SpicyConsoleRuntime: {"N/A"} is null");
            DisableComponent();
            return false;
        }
        
        private void DisableComponent()
        {
            this.enabled = false;
        }

        private void SpicyMessage(LogType logType, string message = "")
        {
            if (areConsoleMessagesEnabled)
            {
                switch (logType) {
                    case LogType.Log: Debug.Log(message); break;
                    case LogType.Error: Debug.LogError(message); break;
                    case LogType.Warning: Debug.LogWarning(message); break;
                }
            }
        } 
    }
}