// Used libraries.
using UnityEngine;
using UnityEngine.EventSystems;

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

        // Window client rect.
        private Rect windowRect = new Rect { position = new Vector2(Screen.width / 4.0f, Screen.height / 4.0f), size = new Vector2(Screen.width/2.0f, Screen.height/2.0f)};

        // Debug window.
        private bool showDebugWindow;

        // Max debug log text length.
        private int maxLogDisplayCharacterCount = 35;

        #endregion

        // Unity.
        #region Unity

        // On enabled.
        private void OnEnable()
        {
            // Subscribe to events.
            AddListeners(true);
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

        // Awake.
        private void Awake()
        {
            // Check if debug is not enabled and return.
            if (enable == false) return;

            // Draw debug console window.
            DrawWindow(new Vector2(Screen.width, Screen.height), this.gameObject, (RectTransform)this.transform);
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
            // Formated message.
            string formatedMessage = (string)logMessage;

            // Checking string length.
            if(formatedMessage.Length <= maxLogDisplayCharacterCount)
            {
                // Assign formated message.
                formatedMessage = (string)logMessage;
            }
            else
            {
                // Format text.
                formatedMessage = formatedMessage.Substring(0, maxLogDisplayCharacterCount - 2);

                // Add surfix.
                formatedMessage += "...";
            }

            // Switch log types.
            switch(logType)
            {
                // Switching log types.
                case DebugData.LogType.LogInfo: // Logging infos.

                    // Raw info format.
                    string rawInfoFormat = string.Format($"<color=white>--->>></color> <color=blue>Info Message Results : </color><color=white>{logMessage}.</color><color=grey> From class : </color><color=white>{logClassName}</color>.");

                    // Log info format.
                    string logInfoFormat = string.Format($"<color=white>--->>></color> <color=blue>Log Info! : </color><color=white>{formatedMessage}.</color><color=grey> From class : </color><color=white>{logClassName}</color>.");

                    // Log to console window.
                    base.LogConsoleWindow(rawInfoFormat, logInfoFormat, DebugData.LogType.LogInfo);

                    // Check if log to Unity console.
                    if(logToUnityConsole)
                    {
                        // Log to console.
                        Debug.Log(logInfoFormat);
                    }

                    break;

                case DebugData.LogType.LogWarning: // Logging warnings.

                    // Raw info format.
                    string rawWarningFormat = string.Format($"<color=white>--->>></color> <color=orange>Warning Message Results : </color><color=white>{logMessage}.</color><color=grey> From class : </color><color=white>{logClassName}</color>.");

                    // Log warning format.
                    string logWarningFormat = string.Format($"<color=white>--->>></color> <color=orange>Log Warning! </color>: <color=white>{formatedMessage}.</color><color=grey> From class : </color><color=white>{logClassName}</color>.");

                    // Log to console window.
                    base.LogConsoleWindow(rawWarningFormat, logWarningFormat, DebugData.LogType.LogWarning);

                    // Check if log to Unity console.
                    if (logToUnityConsole)
                    {
                        // Log to console.
                        Debug.LogWarning(logWarningFormat);
                    }

                    break;

                case DebugData.LogType.LogError: // Logging errors.

                    // Raw info format.
                    string rawErrorFormat = string.Format($"<color=white>--->>></color> <color=red>Error Message Results : </color><color=white>{logMessage}.</color><color=grey> From class : </color><color=white>{logClassName}</color>.");

                    // Log error format.
                    string logErrorFormat = string.Format($"<color=white>--->>></color> <color=red>Log Error!</color> : <color=white>{formatedMessage}.</color><color=grey> From class : </color><color=white>{logClassName}</color>.");

                    // Log to console window.
                    base.LogConsoleWindow(rawErrorFormat, logErrorFormat, DebugData.LogType.LogError);

                    // Check if log to Unity console.
                    if (logToUnityConsole)
                    {
                        // Log to console.
                        Debug.LogError(logErrorFormat);
                    }

                    break;
            }
        }

        #endregion
    }
}
