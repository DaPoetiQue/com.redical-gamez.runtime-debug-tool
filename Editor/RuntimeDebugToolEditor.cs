// Used libraries.
using UnityEditor;
using UnityEngine;
using UnityEngine.Device;

// Namespace.
namespace RedicalGamez.Dev.ToolKit
{
    // Runtime debug tool editor class.
    public class RuntimeDebugToolEditor : Editor
    {
        // Menu item.
        [MenuItem("Redical Gamez/Dev Tools/Create Console Window")]
        private static void CreateRuntimeDebugTool()
        {
            // Create a new debug window.
            GameObject debugWindow = new GameObject("Runtime Debug Console");

            // Assign debug indow component.
            debugWindow.AddComponent<RuntimeDebugger>();
        }

        // Menu item.
        [MenuItem("Redical Gamez/Dev Tools/Create Console Window", true)]
        private static bool OnCreateRuntimeDebugTool()
        {
            // Checking if debug window exist in the scene.
            if (FindObjectOfType<RuntimeDebugger>() == false)
            {
                // Enable seletion.
                return true;
            }
            else
            {
                // Disable seletion.
                return false;
            }
        }
    }
}
