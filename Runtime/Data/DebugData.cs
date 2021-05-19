// Used libraries.
using System;
using UnityEngine;

// Namespace.
namespace RedicalGamez.Dev.ToolKit
{
    public class DebugData    
    {
        // Enum data.
        #region Enum Data

        // Log type.
        public enum LogType
        {
            // Log types.
            LogInfo, LogWarning, LogError, LogAll
        }

        // Console toggle button position.
        public enum ConsoleToggleButtonPosition
        {
            // Button positions.
            BottomLeft, BottonRight, TopLeft, TopRight
        }

        // Console window button type.
        public enum WindowButtonType
        {
            // Button types.
            None, Close, Clear, LogInfo, LogWarning, LogError, LogAll
        }

        #endregion

        // Event data.
        #region Event Data

        #endregion

        // Structs.
        #region Structs

        // Log panel
        [Serializable]
        public struct LogPanel
        {
            // Panel.
            public GameObject content;

            // Log type.
            public LogType logType;
        }

        public struct LogItem
        {
            // Log data.
            public string logName, logMessage;

            // Log panel.
            public GameObject logPanel;

            // Panel color.
            public Color panelColor;

            // Panel dimensions.
            public float minLogPanelHeight, preferedLogPanelHeight;
        }
        
        #endregion;
    }
}
