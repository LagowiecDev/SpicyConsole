using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SpicyConsole;
using SpicyConsole.Setup;

public class SpicyConsoleCommandAddon : MonoBehaviour
{
    [SpicyCommand("console_print")]
    private void SpicyPrinting(string Message, string LogType = "Log")
    {
        switch (LogType.ToLower())
        {
            case "log": Debug.Log(Message); break;
            case "warning": Debug.LogWarning(Message); break;
            case "error": Debug.LogError(Message); break;
        }
    }

    [SpicyCommand("console_spawn")]
    private void SpawnModel(string objectPath, string materialPath, string[] _position, string[] _rotation)
    {
        // Load the model prefab (assumes it's in a "Assets" folder)
        GameObject model = Instantiate(Resources.Load<GameObject>(objectPath));

        // Load the material (assumes it's in a "Materials" folder)
        Material material = Resources.Load<Material>("Materials/" + materialPath);

        // Apply the material to the model
        Renderer renderer = model.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
        else
        {
            Debug.LogError("Model doesn't have a Renderer component.");
        }

        // Optionally, you can set the spawned model's position, rotation, etc.
        model.transform.position = StringArrayToVector3(_position);
        model.transform.rotation = StringArrayToQuaternion(_rotation);
    }

    Vector3 StringArrayToVector3(string[] _toConvert)
    {
        if (_toConvert.Length == 3)
        {
            try {
                return new Vector3((float)float.Parse(_toConvert[0]),(float)float.Parse(_toConvert[1]),(float)float.Parse(_toConvert[2]));
            } catch (Exception ex) {
                Debug.LogError(ex.Message);
                return new Vector3(0,0,0);
            }
        } else {
            Debug.LogWarning("Cannot convert: Array Lenght is not 3");
            return new Vector3(0,0,0);
        }
    }

    Quaternion StringArrayToQuaternion(string[] _toConvert)
    {
        if (_toConvert.Length == 3)
        {
            try {
                return Quaternion.Euler((float)float.Parse(_toConvert[0]),(float)float.Parse(_toConvert[1]),(float)float.Parse(_toConvert[2]));
            } catch (Exception ex) {
                Debug.LogError(ex.Message);
                return Quaternion.Euler(0,0,0);
            }
        } else {
            Debug.LogWarning("Cannot convert: Array Lenght is not 3");
            return Quaternion.Euler(0,0,0);
        }
    }
}
