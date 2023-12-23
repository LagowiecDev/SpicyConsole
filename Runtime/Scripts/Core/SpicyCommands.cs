using UnityEngine;
using System.Reflection;
using SpicyConsole.Formatting.BinarySerializing;

namespace SpicyConsole.Formatting {
    public class SpicyCommands : ScriptableObject
    {
        public SerializableMethodInfo[] methodInfos;
    }
}