using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace SpicyConsole.Setup {

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class SpicyCommandAttribute : Attribute
    {
        public string CustomName { get; }

        public SpicyCommandAttribute(string customName)
        {
            CustomName = customName;
        }
    }
}

namespace SpicyConsole.Formatting.BinarySerializing {

    [System.Serializable]
    public class SerializableType : ISerializationCallbackReceiver
    {
        public System.Type type;
        public byte[] data;
        public SerializableType(System.Type aType)
        {
            type = aType;
        }

        public static System.Type Read(BinaryReader aReader)
        {
            var paramCount = aReader.ReadByte();
            if (paramCount == 0xFF)
                return null;
            var typeName = aReader.ReadString();
            var type = System.Type.GetType(typeName);
            if (type == null)
                throw new System.Exception("Can't find type; '" + typeName + "'");
            if (type.IsGenericTypeDefinition && paramCount > 0)
            {
                var p = new System.Type[paramCount];
                for (int i = 0; i < paramCount; i++)
                {
                    p[i] = Read(aReader);
                }
                type = type.MakeGenericType(p);
            }
            return type;
        }

        public static void Write(BinaryWriter aWriter, System.Type aType)
        {
            if (aType == null)
            {
                aWriter.Write((byte)0xFF);
                return;
            }
            if (aType.IsGenericType)
            {
                var t = aType.GetGenericTypeDefinition();
                var p = aType.GetGenericArguments();
                aWriter.Write((byte)p.Length);
                aWriter.Write(t.AssemblyQualifiedName);
                for (int i = 0; i < p.Length; i++)
                {
                    Write(aWriter, p[i]);
                }
                return;
            }
            aWriter.Write((byte)0);
            aWriter.Write(aType.AssemblyQualifiedName);
        }


        public void OnBeforeSerialize()
        {
            using (var stream = new MemoryStream())
            using (var w = new BinaryWriter(stream))
            {
                Write(w, type);
                data = stream.ToArray();
            }
        }

        public void OnAfterDeserialize()
        {
            using (var stream = new MemoryStream(data))
            using (var r = new BinaryReader(stream))
            {
                type = Read(r);
            }
        }
    }

    [System.Serializable]
    public class SerializableMethodInfo : ISerializationCallbackReceiver
    {
        public SerializableMethodInfo(MethodInfo aMethodInfo)
        {
            methodInfo = aMethodInfo;
        }
        public MethodInfo methodInfo;
        public SerializableType type;
        public string methodName;
        public List<SerializableType> parameters = null;
        public int flags = 0;

        public void OnBeforeSerialize()
        {
            if (methodInfo == null)
                return;
            type = new SerializableType(methodInfo.DeclaringType);
            methodName = methodInfo.Name;
            if (methodInfo.IsPrivate)
                flags |= (int)BindingFlags.NonPublic;
            else
                flags |= (int)BindingFlags.Public;
            if (methodInfo.IsStatic)
                flags |= (int)BindingFlags.Static;
            else
                flags |= (int)BindingFlags.Instance;
            var p = methodInfo.GetParameters();
            if (p != null && p.Length > 0)
            {
                parameters = new List<SerializableType>(p.Length);
                for (int i = 0; i < p.Length; i++)
                {
                    parameters.Add(new SerializableType(p[i].ParameterType));
                }
            }
            else
                parameters = null;
        }

        public void OnAfterDeserialize()
        {
            if (type == null || string.IsNullOrEmpty(methodName))
                return;
            var t = type.type;
            System.Type[] param = null;
            if (parameters != null && parameters.Count>0)
            {
                param = new System.Type[parameters.Count];
                for (int i = 0; i < parameters.Count; i++)
                {
                    param[i] = parameters[i].type;
                }
            }
            if (param == null)
                methodInfo = t.GetMethod(methodName, (BindingFlags)flags);
            else
                methodInfo = t.GetMethod(methodName, (BindingFlags)flags,null, param, null);
        }
    }

    public static class ByteConverter {
        public static MethodInfo[] SerializedMethodInfoArrayToInfoMethodArray(SerializableMethodInfo[] serializableMethodInfos)
        {
            List<MethodInfo> _return = new List<MethodInfo>();

            foreach (SerializableMethodInfo serializableMethodInfo in serializableMethodInfos)
            {
                _return.Add(serializableMethodInfo.methodInfo);
            }

            return _return.ToArray();
        }

        public static SerializableMethodInfo[] MethodInfoArrayToSerializedInfoMethodArray(MethodInfo[] methodInfos)
        {
            List<SerializableMethodInfo> _return = new List<SerializableMethodInfo>();

            foreach (MethodInfo methodInfo in methodInfos)
            {
                SerializableMethodInfo SMI = new SerializableMethodInfo(methodInfo);

                _return.Add(SMI);
            }

            return _return.ToArray();
        }
    }
}