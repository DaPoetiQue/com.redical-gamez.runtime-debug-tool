// Namespace.
namespace RedicalGamez.Dev.ToolKit
{
    public class Debugger
    {
        // Delegates.
        #region Delegates

        // Debug signature.
        public delegate void DebugEvent<T,C,M>(T logType, C logClassName, M logMessage);

        #endregion

        // Events.
        #region Events

        // Add listener.
        public static event DebugEvent<DebugData.LogType, object, object> AddListener;

        #endregion

        // Invokes.
        #region Invokes

        // Call log.
        public static void Log(DebugData.LogType logType, object logClassName, object logMessage)
        {
            // Check if has subscribers.
            if(AddListener != null)
            {
                // Add listeners.
                AddListener(logType, logClassName, logMessage);
            }
        }

        #endregion
    }
}
