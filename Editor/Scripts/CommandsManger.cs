using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SpicyConsole.Setup;
using SpicyConsole.Formatting;
using SpicyConsole.Formatting.BinarySerializing;
using System.Reflection;

namespace SpicyConsole.EditorScripts {
    public class CommandsManger : Editor {

        static string defaultPath = "Assets/Settings/ThirdParty/LagowiecDev/SpicyConsole";

        [InitializeOnLoadMethod]
        static void SC_IOLM()
        {
            Debug.Log("Reloading Commands List");
            GetAllCommands();
        }

        [MenuItem("ThirdParty/LagowiecDev/SpicyConsole/Update Commands List")]
        public static void GetAllCommands()
        {
            List<MethodInfo> methodInfosList = new List<MethodInfo>();

            var MethodsWhatGot = TypeCache.GetMethodsWithAttribute<SpicyCommandAttribute>();
            foreach (MethodInfo methodToConvert in MethodsWhatGot)
            {
                methodInfosList.Add(methodToConvert);
            }

            SpicyCommands fileData = CreateInstance<SpicyCommands>();
            fileData.methodInfos =  ByteConverter.MethodInfoArrayToSerializedInfoMethodArray(methodInfosList.ToArray());

            if (!Directory.Exists(defaultPath))
            {
                Directory.CreateDirectory(defaultPath);
            }

            // Save the fileData to the asset file
            AssetDatabase.CreateAsset(fileData, $"{defaultPath}/CommandsList.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
