// Used libraries.
using UnityEngine;

// Namespace.
namespace RedicalGamez.Dev.ToolKit
{
    // Runtime debugger class.
    public class RuntimeDebugger : DebugWindow
    {
        // Variables.
        #region Variables

        // Console toggle button position.
        [SerializeField]
        private DebugData.ConsoleToggleButtonPosition consoleToggleButtonPosition;

        // Debug window toggler rect.
        private Rect bottomLeftButtonAnchorPoint = new Rect { position = new Vector2(Screen.width / 25.0f, (Screen.height - ((Screen.height / 10.0f) * 2))), size = new Vector2(Screen.width / 5.0f, Screen.height / 10.0f) },
                     bottomRightButtonAnchorPoint = new Rect { position = new Vector2((Screen.width - (Screen.width / 5.0f)) - Screen.width / 25.0f, (Screen.height - ((Screen.height / 10.0f) * 2))), size = new Vector2(Screen.width / 5.0f, Screen.height / 10.0f) },
                     topLeftButtonAnchorPoint = new Rect { position = new Vector2(Screen.width/25.0f,Screen.height/15.0f), size = new Vector2(Screen.width/5.0f, Screen.height/10.0f) },
                     topRightButtonAnchorPoint = new Rect { position = new Vector2((Screen.width - (Screen.width /5.0f)) - Screen.width / 25.0f, Screen.height/15.0f), size = new Vector2(Screen.width/5.0f, Screen.height/10.0f) };

        // Button anchor point.
        private Rect buttonScreenAnchorPoint = new Rect();

        // Window client rect.
        private Rect windowClientRect = new Rect { position = new Vector2(Screen.width / 4.0f, Screen.height / 4.0f), size = new Vector2(Screen.width/2.0f, Screen.height/2.0f)};

        // Debug window.
        private bool showDebugWindow;

        #endregion

        // Unity.
        #region Unity

        // On enabled.
        private void OnEnable()
        {
            // Subscribe to events.
            AddListeners(true);

            // L<og
            Debugger.Log(DebugData.LogType.LogInfo, this, $"Screen width : {Screen.width}, Screen height : {Screen.height}");
        }

        // On disabled.
        private void OnDisable()
        {
            // Un-subscribe from events.
            AddListeners(false);
        }

        // On destroyed.
        private void OnDestroy()
        {
            // Un-subscribe from events.
            AddListeners(false);
        }

        // On gui.
        private void OnGUI()
        {
            // Check if showing debug window is not enabled.
            if (enableLogs == false) return;

            // Switch button position.
            switch(consoleToggleButtonPosition)
            {
                // Switching button positions.
                case DebugData.ConsoleToggleButtonPosition.BottomLeft: // Bottom left.

                    // Assign button screen anchor point.
                    buttonScreenAnchorPoint = bottomLeftButtonAnchorPoint;

                    break;

                case DebugData.ConsoleToggleButtonPosition.BottonRight: // Bottom right.

                    // Assign button screen anchor point.
                    buttonScreenAnchorPoint = bottomRightButtonAnchorPoint;

                    break;

                case DebugData.ConsoleToggleButtonPosition.TopLeft: // Top left.

                    // Assign button screen anchor point.
                    buttonScreenAnchorPoint = topLeftButtonAnchorPoint;

                    break;

                case DebugData.ConsoleToggleButtonPosition.TopRight: // Top right.

                    // Assign button screen anchor point.
                    buttonScreenAnchorPoint = topRightButtonAnchorPoint;

                    break;
            }

            // Debug window toggle button.
            if (GUI.Button(buttonScreenAnchorPoint, "Console"))
            {
                // Window state.
                showDebugWindow = !showDebugWindow;

                // Log
                Debugger.Log(DebugData.LogType.LogInfo, this, $"Show window : {showDebugWindow}");
            }

            if(showDebugWindow)
            {
                // Window rect.
                windowClientRect = GUI.Window(0, windowClientRect, OnDrawDebugWindow, "Unity Runtime Debug Tool");
            }
            else
            {
                // Return.
                return;
            }
        }

        #endregion

        // Main.
        #region Main

        // Add listeners.
        private void AddListeners(bool add)
        {
            // Checking if can add a listener.
            if(add)
            {
                // Adding listeners.
                Debugger.AddListener += this.OnDebug;
            }
            else
            {
                // Removing listeners.
                Debugger.AddListener -= this.OnDebug;
            }
        }

        // Debug.
        private void OnDebug(DebugData.LogType logType, object logClassName, object logMessage)
        {
            // Switch log types.
            switch(logType)
            {
                // Switching log types.
                case DebugData.LogType.LogInfo: // Logging infos.

                    // Log info format.
                    string logInfoFormat = string.Format($"<color=white>--->>></color><color=blue>Log Info! : </color><color=white>{logMessage}.</color><color=grey> From class : </color><color=white>{logClassName}</color>.");

                    // Log.
                    Debug.Log(logInfoFormat);

                    break;

                case DebugData.LogType.LogWarning: // Logging warnings.

                    // Log warning format.
                    string logWarningFormat = string.Format($"<color=white>--->>></color><color=orange>Log Warning! </color>: <color=white>{logMessage}.</color><color=grey> From class : </color><color=white>{logClassName}</color>.");

                    // Log warning.
                    Debug.LogWarning(logWarningFormat);

                    break;

                case DebugData.LogType.LogError: // Logging errors.

                    // Log error format.
                    string logErrorFormat = string.Format($"<color=white>--->>></color><color=red>Log Error!</color> : <color=white>{logMessage}.</color><color=grey> From class : </color><color=white>{logClassName}</color>.");

                    // Log error.
                    Debug.LogError(logErrorFormat);

                    break;
            }
        }

        // Draw
        private void OnDrawDebugWindow(int windowID)
        {
            // Make window draggable.
            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000,10000));
        }

        #endregion
    }
}
