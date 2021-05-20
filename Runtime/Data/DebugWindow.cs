// Used libraries.
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Namespace.
namespace RedicalGamez.Dev.ToolKit
{
    // Require component.
    [RequireComponent(typeof(RectTransform))]

    // Disallow multiple components.
    [DisallowMultipleComponent]

    // Debug window class.
    public abstract class DebugWindow : MonoBehaviour, IDragHandler
    {
        // Properties.
        #region Properties

        // Enable logs.
        [SerializeField]
        protected bool enable = true;

        // Log to console.
        [Space(5)]
        [SerializeField]
        protected bool logToUnityConsole = false;

        // Window panel colors
       [Space(5)]
        [SerializeField]
        protected Color consoleHeaderWindow = new Color(60.0f, 60.0f, 60.0f),
                        consoleWindow = new Color(30.0f, 30.0f, 30.0f),
                        consoleLogWindow = new Color(20.0f, 20.0f, 20.0f); 

        // Debug window.
        private RectTransform debugWindow = null, mainConsoleWindow = null;

        // Window created.
        private bool windowCreated = false;

        // Window state.
        private bool windowOpen = false;

        // Logs count display text.
        private Text logInfoCountDisplayer = null, logWarningCountDisplayer = null, logErrorCountDisplayer = null, logAllCountDisplayer = null;

        // Descriptive log message displayer.
        private Text descriptiveLogWindowMessageDisplayer = null;

        // Log count.
        private int logInfoCount = 0, logWarningCount = 0, logErrorCount = 0, logAllCount = 0;

        // Header text font size.
        private int headerTitleFontSize = 20, logMessageDisplayerFontSize = 20, descriptiveLogDisplayerFontSize = 20;

        private int descriptiveLogDisplayerLeftPaddingAmount = 25, descriptiveLogDisplayerRightPaddingAmount = 25;


        // Log panel padding amount.
        private float logPanelPaddingAmount = 25.0f;

        // Log panel height.
        private float logPanelMinHeight = 50.0f, logPanelPreferedHeight = 85.0f, descriptiveLogInfoWindowMinHeight = 100.0f, descriptiveLogInfoWindowPreferedHeight = 150.0f;

        // Default descriptive log window info.
        private string defaultDesLogWinDisplayerInfo = "Log.";

        // Active log panel list.
        [SerializeField]
        private List<DebugData.LogPanel> activeLogPanelList = new List<DebugData.LogPanel>();

        // Active log message displayer dictionary.
        private Dictionary<string, Text> logMessageDictionary = new Dictionary<string, Text>();

        // Log panel delete list.
        protected List<GameObject> logPanelDeleteList = new List<GameObject>();

        // Log message list.
        private List<string> logMessageDeleteList = new List<string>();

        #endregion

        // Unity.
        #region Unity

            // On drag.
        public void OnDrag(PointerEventData ped)
        {
            // Drag window.
            DragWindow(ped.delta);
        }

        #endregion

        // Main.
        #region Main

        // Draw window.
        protected virtual void DrawWindow(Vector2 screenResolution, GameObject debugWindowCanvas, RectTransform windowParent)
        {
            // Check if window is created.
            if (windowCreated) return;

            // Get window.
            debugWindow = CreateWindow(screenResolution : screenResolution, debugWindowCanvas : debugWindowCanvas, windowParent : windowParent);

            // Assign draggable component.
            // debugWindow.gameObject.AddComponent<DraggableWindow>();

        }

        // Log.
        protected void LogConsoleWindow(string rawLogMessage, string logMessage, DebugData.LogType logType)
        {
            // Check if message doesn't exist in the delete list.
            if(logMessageDeleteList.Contains(logMessage) == false)
            {
                // Switch log type.
                switch (logType)
                {
                    // Switching log types.
                    case DebugData.LogType.LogInfo: // Log info.

                        // Increment info count.
                        logInfoCount++;

                        break;

                    case DebugData.LogType.LogWarning: // Log warning.

                        // Increment info count.
                        logWarningCount++;

                        break;

                    case DebugData.LogType.LogError: // Log error.

                        // Increment info count.
                        logErrorCount++;

                        break;
                }

                // Increment all log count.
                logAllCount++;

                // Create console log.
                CreateConsoleLog("Logger", rawLogMessage, logMessage, Color.black, this.logPanelMinHeight, this.logPanelPreferedHeight, mainConsoleWindow, logType);

                // Update window content.
                UpdateWindowContent();

                // Add message to list
                logMessageDeleteList.Add(logMessage);
            }
            else
            {
                // Return.
                return;
            }
        }

        // Update window content.
        private void UpdateWindowContent()
        {
            // Update window buttons title.
            SetTitle($"Info ({logInfoCount})", logInfoCountDisplayer);
            SetTitle($"Warning ({logWarningCount})", logWarningCountDisplayer);
            SetTitle($"Error ({logErrorCount})", logErrorCountDisplayer);
            SetTitle($"All ({logAllCount})", logAllCountDisplayer);
        }

        // Set Title
        private void SetTitle(string title, Text titleDisplayer)
        {
            // Set title.
            titleDisplayer.text = title;
        }

        // Create window.
        private RectTransform CreateWindow(Vector2 screenResolution, GameObject debugWindowCanvas, RectTransform windowParent)
        {
            // Current resolution.
            Vector2 currentResolution = Vector2.zero;

            // Check if landscape.
            if(screenResolution.x > screenResolution.y)
            {
                // Setup landscape resolution.
                currentResolution = screenResolution;
            }
            else
            {
                // Setup portrait resolution.
                currentResolution.x = screenResolution.y;
                currentResolution.y = screenResolution.x;
            }

            // Create canvas.
            CreateCanvas(debugWindow: debugWindowCanvas, referenceResolution: currentResolution);

            // Create a new window object.
            GameObject debugWindow = new GameObject("Runtime Debug Window");

            // Assign a rect transform component.
            RectTransform window = debugWindow.AddComponent<RectTransform>();

            // Set window position.
            window.anchoredPosition = Vector2.zero;

            // Set window resolution.
            window.sizeDelta = currentResolution / 2;

            // Assign image component.
            Image windowBackground = debugWindow.AddComponent<Image>();

            // Assign background color.
            windowBackground.color = Color.black;

            window.SetParent(windowParent, false);

            // Set window created to true.
            windowCreated = true;

            // Create a window layout.
            CreateWindowLayout(window);

            // Return window.
            return window;
        }

        // create canvas.
        private void CreateCanvas(GameObject debugWindow, Vector2 referenceResolution)
        {

            // Add a canvas component.
            Canvas debugWinCanvas = debugWindow.AddComponent<Canvas>();

            // Setup canvas.
            CanvasSetup(debugWinCanvas);

            // Add a canvas scaler.
            CanvasScaler debugWinCanvasScaler = debugWindow.AddComponent<CanvasScaler>();

            // Setup canvas scaler.
            CanvasScalerSetup(debugWindowCanvasScaler : debugWinCanvasScaler, referenceResolution : referenceResolution);

            // Add a graphic raycaster.
            debugWindow.AddComponent<GraphicRaycaster>();

            // Create an event system.
            CreateEvent(debugWindow);
        }

        // Create a window layout.
        private void CreateWindowLayout(RectTransform window)
        {
            // Add a vertical layout group component.
            VerticalLayoutGroup layout = window.gameObject.AddComponent<VerticalLayoutGroup>();

            // Set layout spacing.
            layout.spacing = 5.0f;

            // Create console header window panel.
            RectTransform consoleHeaderWindowPanel = CreateWindowPanel("Console Window Header", this.consoleHeaderWindow, 50.0f, 50.0f, window);

            // Add horizontal layout group to header panel.
            consoleHeaderWindowPanel.gameObject.AddComponent<HorizontalLayoutGroup>().spacing = 5.0f;

            Button clearConsoleButton = CreateWindowButton("Clear Button", "Clear", Color.grey, 100.0f, 150.0f, consoleHeaderWindowPanel, DebugData.WindowButtonType.Clear);
            Button logInfoConsoleButton = CreateWindowButton("Show Log Info Button", "Info (0)", Color.grey, 100.0f, 150.0f, consoleHeaderWindowPanel, DebugData.WindowButtonType.LogInfo);
            Button logWarningsConsoleButton = CreateWindowButton("Show Log Warning Button", "Warning (0)", Color.grey, 100.0f, 150.0f, consoleHeaderWindowPanel, DebugData.WindowButtonType.LogWarning);
            Button logErrorsConsoleButton = CreateWindowButton("Show Log Error Button", "Error (0)", Color.grey, 100.0f, 150.0f, consoleHeaderWindowPanel, DebugData.WindowButtonType.LogError);
            Button logAllConsoleButton = CreateWindowButton("Show Log All Button", "All (0)", Color.grey, 100.0f, 150.0f, consoleHeaderWindowPanel, DebugData.WindowButtonType.LogAll);

            // Create console window panel.
            mainConsoleWindow = CreateWindowPanel("Console Window", this.consoleWindow, window.sizeDelta.y / 2.0f, window.sizeDelta.y, window);

            // Add mask.
            mainConsoleWindow.gameObject.AddComponent<Mask>();

            // Assign vertical layout group
            VerticalLayoutGroup consloleWindowLayout = mainConsoleWindow.gameObject.AddComponent<VerticalLayoutGroup>();

            // Setup layout.
            consloleWindowLayout.childControlWidth = true;
            consloleWindowLayout.childControlHeight = false;
            consloleWindowLayout.childForceExpandWidth = true;
            consloleWindowLayout.childForceExpandHeight = false;

            // Add spacing.
            consloleWindowLayout.spacing = 5.0f;

            // Create console log window panel.
            RectTransform consoleDescriptiveLogWindowPanel = CreateWindowPanel("Log Window", this.consoleLogWindow,descriptiveLogInfoWindowMinHeight, descriptiveLogInfoWindowPreferedHeight, window);

            HorizontalLayoutGroup consoleDescriptiveLogWindowPanelLayout = consoleDescriptiveLogWindowPanel.gameObject.AddComponent<HorizontalLayoutGroup>();

            // Assign panel padding.
            consoleDescriptiveLogWindowPanelLayout.padding.left = descriptiveLogDisplayerLeftPaddingAmount;
            consoleDescriptiveLogWindowPanelLayout.padding.right = descriptiveLogDisplayerRightPaddingAmount;

            // Add text component.
            AddTextComponentToWindowPanel("Descriptive Log Displayer", descriptiveLogInfoWindowPreferedHeight, consoleDescriptiveLogWindowPanel);

            // Create window header buttons.
            Button closeConsoleButton = CreateWindowButton("Close Button", "X", Color.red, 25.0f, 50.0f, consoleHeaderWindowPanel, DebugData.WindowButtonType.Close);
        }

        // Add text component.
        private void AddTextComponentToWindowPanel(string logDisplayerName, float panelPreferedHeight, RectTransform parentWindow)
        {
            // Create a new text object.
            GameObject logWindowMessageDisplayer = new GameObject(logDisplayerName);

            // Add rect transform.
            RectTransform logWindowMessageDisplayerRect = logWindowMessageDisplayer.AddComponent<RectTransform>();

            // Text rect anchors.
            Vector2 anchorMin = Vector2.zero;
            anchorMin.y = 0.5f;
            Vector2 anchorMax = Vector2.zero;
            anchorMax.y = 0.5f;

            // Setup panel rect anchors.
            logWindowMessageDisplayerRect.anchorMin = anchorMin;
            logWindowMessageDisplayerRect.anchorMax = anchorMax;

            // Set pivot.
            logWindowMessageDisplayerRect.pivot = anchorMin;

            // logDisplayTextRect position.
            Vector2 logDisplayTextPosition = Vector2.zero;

            // Add text displayer margin.
            logDisplayTextPosition.x = logPanelPaddingAmount;

            // Set button position.
            logWindowMessageDisplayerRect.anchoredPosition = logDisplayTextPosition;

            // Add Text component to button text.
            Text logTextDisplayer = logWindowMessageDisplayer.AddComponent<Text>();

            // Set line spacing.
            logTextDisplayer.lineSpacing = 1.5f;

            // Add text font
            logTextDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            // Align text.
            logTextDisplayer.alignment = TextAnchor.MiddleLeft;

            // Set text font size.
            logTextDisplayer.fontSize = descriptiveLogDisplayerFontSize;

            // Set text overflow.
            logTextDisplayer.horizontalOverflow = HorizontalWrapMode.Wrap;
            logTextDisplayer.verticalOverflow = VerticalWrapMode.Overflow;

            // Assign log message.
            logTextDisplayer.text = defaultDesLogWinDisplayerInfo;

            // Displayer size
            Vector2 messageDisplayerSize = Vector2.zero;

            // Assign size values.
            messageDisplayerSize.x = Screen.width / 2.0f;
            messageDisplayerSize.y = panelPreferedHeight;

            // Assign displayer size.
            logWindowMessageDisplayerRect.sizeDelta = messageDisplayerSize;

            // Parent text.
            logWindowMessageDisplayer.transform.SetParent(parentWindow, false);

            // Get descriptive log text.
            descriptiveLogWindowMessageDisplayer = logTextDisplayer;
        }

        // Create panel.
        private RectTransform CreateWindowPanel(string panelName, Color bgColor, float minPanelHeight, float panelHeight, RectTransform parentWindow)
        {
            // Create a new a new panel.
            GameObject windowPanel = new GameObject(panelName);

            // Parent window panel.
            windowPanel.transform.SetParent(parentWindow, false);

            // Add rect transform component.
            RectTransform panelRect = windowPanel.AddComponent<RectTransform>();

            // Add image component to panel.
            Image panelBackground = windowPanel.AddComponent<Image>();

            // Assign panel background color.
            panelBackground.color = bgColor;

            // Add layout element.
            LayoutElement panelLayout = windowPanel.AddComponent<LayoutElement>();

            // Min height.
            panelLayout.minHeight = minPanelHeight;

            // Set prefared height
            panelLayout.preferredHeight = panelHeight;

            // Return panel rect.
            return panelRect;
        }

        // Create window button.
        private Button CreateWindowButton(string buttonName, string buttonTitle, Color buttonColor, float minButtonWidth, float preferedButtonWidth, RectTransform parentWindow, DebugData.WindowButtonType windowButtonType)
        {
            // Create a new a new window button.
            GameObject windowButton = new GameObject(buttonName);

            // Add rect transform component.
            RectTransform windowButtonRect = windowButton.AddComponent<RectTransform>();

            // Add button image component with color assigned.
            windowButton.AddComponent<Image>().color = buttonColor;

            // Create button text.
            GameObject buttonText = new GameObject("Button Title");

            // Add button component.
            Button button = windowButton.AddComponent<Button>();

            // Switch button type.
            switch (windowButtonType)
            {
                // Switching button types.
                case DebugData.WindowButtonType.Close: // Close.

                    // Add Text component to button text.
                    Text closeButtonTitleDisplayer = buttonText.AddComponent<Text>();

                    // Add text font
                    closeButtonTitleDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

                    // Align text.
                    closeButtonTitleDisplayer.alignment = TextAnchor.MiddleCenter;

                    // Set text font size.
                    closeButtonTitleDisplayer.fontSize = headerTitleFontSize;

                    // Set text overflow.
                    closeButtonTitleDisplayer.horizontalOverflow = HorizontalWrapMode.Overflow;
                    closeButtonTitleDisplayer.verticalOverflow = VerticalWrapMode.Overflow;

                    // Add title text.
                    closeButtonTitleDisplayer.text = buttonTitle;

                    // Assign button function.
                    button.onClick.AddListener(this.CloseWindow);

                    break;

                case DebugData.WindowButtonType.LogInfo: // Log info.

                    // Add Text component to button text.
                    logInfoCountDisplayer = buttonText.AddComponent<Text>();

                    // Add text font
                    logInfoCountDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

                    // Align text.
                    logInfoCountDisplayer.alignment = TextAnchor.MiddleCenter;

                    // Set text font size.
                    logInfoCountDisplayer.fontSize = headerTitleFontSize;

                    // Set text overflow.
                    logInfoCountDisplayer.horizontalOverflow = HorizontalWrapMode.Overflow;
                    logInfoCountDisplayer.verticalOverflow = VerticalWrapMode.Overflow;

                    // Add title text.
                    logInfoCountDisplayer.text = buttonTitle;

                    // Assign button function.
                    button.onClick.AddListener(() => this.ShowSelectedLogType(DebugData.LogType.LogInfo));

                    // Set selection color.
                    ColorBlock infoColorBlock = button.colors;
                    infoColorBlock.selectedColor = consoleWindow;
                    infoColorBlock.highlightedColor = consoleWindow;
                    infoColorBlock.pressedColor = consoleWindow;

                    // Set pressed state.
                    button.colors = infoColorBlock;

                    break;

                case DebugData.WindowButtonType.LogWarning: // Log warning.

                    // Add Text component to button text.
                    logWarningCountDisplayer = buttonText.AddComponent<Text>();

                    // Add text font
                    logWarningCountDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

                    // Align text.
                    logWarningCountDisplayer.alignment = TextAnchor.MiddleCenter;

                    // Set text font size.
                    logWarningCountDisplayer.fontSize = headerTitleFontSize;

                    // Set text overflow.
                    logWarningCountDisplayer.horizontalOverflow = HorizontalWrapMode.Overflow;
                    logWarningCountDisplayer.verticalOverflow = VerticalWrapMode.Overflow;

                    // Add title text.
                    logWarningCountDisplayer.text = buttonTitle;

                    // Assign button function.
                    button.onClick.AddListener(() => this.ShowSelectedLogType(DebugData.LogType.LogWarning));

                    // Set selection color.
                    ColorBlock warningColorBlock = button.colors;
                    warningColorBlock.selectedColor = consoleWindow;
                    warningColorBlock.highlightedColor = consoleWindow;
                    warningColorBlock.pressedColor = consoleWindow;

                    // Set pressed state.
                    button.colors = warningColorBlock;

                    break;

                case DebugData.WindowButtonType.LogError: // Log error.

                    // Add Text component to button text.
                    logErrorCountDisplayer = buttonText.AddComponent<Text>();

                    // Add text font
                    logErrorCountDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

                    // Align text.
                    logErrorCountDisplayer.alignment = TextAnchor.MiddleCenter;

                    // Set text font size.
                    logErrorCountDisplayer.fontSize = headerTitleFontSize;

                    // Set text overflow.
                    logErrorCountDisplayer.horizontalOverflow = HorizontalWrapMode.Overflow;
                    logErrorCountDisplayer.verticalOverflow = VerticalWrapMode.Overflow;

                    // Add title text.
                    logErrorCountDisplayer.text = buttonTitle;

                    // Assign button function.
                    button.onClick.AddListener(() => this.ShowSelectedLogType(DebugData.LogType.LogError));

                    // Set selection color.
                    ColorBlock errorColorBlock = button.colors;
                    errorColorBlock.selectedColor = consoleWindow;
                    errorColorBlock.highlightedColor = consoleWindow;
                    errorColorBlock.pressedColor = consoleWindow;

                    // Set pressed state.
                    button.colors = errorColorBlock;

                    break;

                case DebugData.WindowButtonType.LogAll: // Log all.

                    // Add Text component to button text.
                    logAllCountDisplayer = buttonText.AddComponent<Text>();

                    // Add text font
                    logAllCountDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

                    // Align text.
                    logAllCountDisplayer.alignment = TextAnchor.MiddleCenter;

                    // Set text font size.
                    logAllCountDisplayer.fontSize = headerTitleFontSize;

                    // Set text overflow.
                    logAllCountDisplayer.horizontalOverflow = HorizontalWrapMode.Overflow;
                    logAllCountDisplayer.verticalOverflow = VerticalWrapMode.Overflow;

                    // Add title text.
                    logAllCountDisplayer.text = buttonTitle;

                    // Assign button function.
                    button.onClick.AddListener(() => this.ShowSelectedLogType(DebugData.LogType.LogAll));

                    // Set selection color.
                    ColorBlock allColorBlock = button.colors;
                    allColorBlock.selectedColor = consoleWindow;
                    allColorBlock.highlightedColor = consoleWindow;
                    allColorBlock.pressedColor = consoleWindow;

                    // Set pressed state.
                    button.colors = allColorBlock;

                    break;

                case DebugData.WindowButtonType.Clear: // Clear.

                    // Add Text component to button text.
                    Text clearButtonTitleDisplayer = buttonText.AddComponent<Text>();

                    // Add text font
                    clearButtonTitleDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

                    // Align text.
                    clearButtonTitleDisplayer.alignment = TextAnchor.MiddleCenter;

                    // Set text font size.
                    clearButtonTitleDisplayer.fontSize = headerTitleFontSize;

                    // Set text overflow.
                    clearButtonTitleDisplayer.horizontalOverflow = HorizontalWrapMode.Overflow;
                    clearButtonTitleDisplayer.verticalOverflow = VerticalWrapMode.Overflow;

                    // Add title text.
                    clearButtonTitleDisplayer.text = buttonTitle;

                    // Assign button function.
                    button.onClick.AddListener(this.ClearLogs);

                    break;
            }

            // Parent button text.
            buttonText.transform.SetParent(windowButtonRect, false);

            // Add layout element component to button.
            LayoutElement layoutElement = windowButton.AddComponent<LayoutElement>();

            // Assign layout size.
            layoutElement.minWidth = minButtonWidth;
            layoutElement.preferredWidth = preferedButtonWidth;

            // Parent button.
            windowButton.transform.SetParent(parentWindow, false);

            // Return button;
            return button;
        }

        // Create console log.
        private void CreateConsoleLog(string logName, string rawLogMessage, string logMessage, Color panelColor, float minLogPanelHeight, float preferedLogPanelHeight, RectTransform parentWindow, DebugData.LogType logType)
        {
            // Create a new log panel
            GameObject logPanel = new GameObject(logName + " : " + logAllCount.ToString());

            // Assign rect transform.
            RectTransform logPanelRect = logPanel.AddComponent<RectTransform>();

            // Log panel size.
            Vector2 logPanelSize = Vector2.zero;

            // Assign panel size.
            logPanelSize.x = logPanelRect.sizeDelta.x;
            logPanelSize.y = preferedLogPanelHeight;

            // Assign panel size.
            logPanelRect.sizeDelta = logPanelSize;

            // Add layout element.
            LayoutElement layoutElement = logPanel.AddComponent<LayoutElement>();

            // Set size.
            layoutElement.minHeight = minLogPanelHeight;
            layoutElement.preferredHeight = preferedLogPanelHeight;

            // Add button component.
            Button logButton = logPanel.AddComponent<Button>();

            // Assign button function.
            logButton.onClick.AddListener(() => this.ShowSelectedLog(rawLogMessage));

            // Add image component with color.
            logPanel.AddComponent<Image>().color = panelColor;

            // Create log panel text displayer.
            GameObject logDisplayText = new GameObject("Log Message Text Displayer");

            // Add rect transform.
            RectTransform logDisplayTextRect = logDisplayText.AddComponent<RectTransform>();

            // Text rect anchors.
            Vector2 anchorMin = Vector2.zero;
            anchorMin.y = 0.5f;
            Vector2 anchorMax = Vector2.zero;
            anchorMax.y = 0.5f;

            // Setup panel rect anchors.
            logDisplayTextRect.anchorMin = anchorMin;
            logDisplayTextRect.anchorMax = anchorMax;

            // Set pivot.
            logDisplayTextRect.pivot = anchorMin;

            // logDisplayTextRect position.
            Vector2 logDisplayTextPosition = Vector2.zero;

            // Add text displayer margin.
            logDisplayTextPosition.x = logPanelPaddingAmount;

            // Set button position.
            logDisplayTextRect.anchoredPosition = logDisplayTextPosition;

            // Add Text component to button text.
            Text logTextDisplayer = logDisplayText.AddComponent<Text>();

            // Add text font
            logTextDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            // Align text.
            logTextDisplayer.alignment = TextAnchor.MiddleLeft;

            // Set text font size.
            logTextDisplayer.fontSize = logMessageDisplayerFontSize;

            // Set text overflow.
            logTextDisplayer.horizontalOverflow = HorizontalWrapMode.Overflow;
            logTextDisplayer.verticalOverflow = VerticalWrapMode.Overflow;

            // Assign log message.
            logTextDisplayer.text = logMessage;

            // Parent text.
            logDisplayText.transform.SetParent(logPanel.transform, false);

            // Parent log panel.
            logPanel.transform.SetParent(parentWindow, false);

            // Log panel.
            DebugData.LogPanel logPanelData = new DebugData.LogPanel { content = logPanel, logType = logType };

            // Add panel to active list.
            activeLogPanelList.Add(logPanelData);

            // Add panel to list.
            logPanelDeleteList.Add(logPanel);

            // Get active log message displayer list.
            // logMessageDictionary.Add(logMessage, logTextDisplayer);
        }

        // Create event.
        private void CreateEvent(GameObject debugWindow)
        {
            // Check if event system is not available.
            if (FindObjectOfType<EventSystem>() == false)
            {
                // Add an event system.
                debugWindow.AddComponent<EventSystem>();

                // Add standalone module.
                debugWindow.AddComponent<StandaloneInputModule>();

                // Add a base input module.
                debugWindow.AddComponent<BaseInput>();
            }
        }

        // Show selected log.
        private void ShowSelectedLog(string logMessage)
        {
            // Check if descriptive log window message displayer is not assigned and return.
            if (descriptiveLogWindowMessageDisplayer == null) return;

            // Log message.
            descriptiveLogWindowMessageDisplayer.text = logMessage;
        }

        // Show selected log type.
        private void ShowSelectedLogType(DebugData.LogType logType)
        {
            // Check if there are no logs and return.
            if (logAllCount <= 0) return;

            // Switch log type.
            switch (logType)
            {
                // Switching log type.
                case DebugData.LogType.LogInfo: // Info logs only.

                    // Loop through active logs.
                    for(int i = 0; i < logAllCount; i++)
                    {
                        // Check if is content to modify.
                        if(activeLogPanelList[i].logType == logType)
                        {
                            // Enable content.
                            activeLogPanelList[i].content.SetActive(true);
                        }
                        else
                        {
                            // Disable content.
                            activeLogPanelList[i].content.SetActive(false);
                        }
                    }

                    break;

                case DebugData.LogType.LogWarning: // Warning logs only.
                                                   // Loop through active logs.
                    for (int i = 0; i < logAllCount; i++)
                    {
                        // Check if is content to modify.
                        if (activeLogPanelList[i].logType == logType)
                        {
                            // Enable content.
                            activeLogPanelList[i].content.SetActive(true);
                        }
                        else
                        {
                            // Disable content.
                            activeLogPanelList[i].content.SetActive(false);
                        }
                    }

                    break;

                case DebugData.LogType.LogError: // Error logs only.

                    // Loop through active logs.
                    for (int i = 0; i < logAllCount; i++)
                    {
                        // Check if is content to modify.
                        if (activeLogPanelList[i].logType == logType)
                        {
                            // Enable content.
                            activeLogPanelList[i].content.SetActive(true);
                        }
                        else
                        {
                            // Disable content.
                            activeLogPanelList[i].content.SetActive(false);
                        }
                    }

                    break;


                case DebugData.LogType.LogAll: // Log all.

                    // Loop through active logs.
                    for (int i = 0; i < logAllCount; i++)
                    {
                        // Enable content.
                        activeLogPanelList[i].content.SetActive(true);
                    }

                    break;
            }
        }

        // Canvas setup.
        private Canvas CanvasSetup(Canvas debugWindowCanvas)
        {
            // Setup render mode
            debugWindowCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // Enable pixel perfect.
            debugWindowCanvas.pixelPerfect = true;

            // Return a customized debug window canvas.
            return debugWindowCanvas;
        }

        // Canvas scaler setup.
        private CanvasScaler CanvasScalerSetup(CanvasScaler debugWindowCanvasScaler, Vector2 referenceResolution)
        {
            // Setup render mode.
            debugWindowCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            // Modify scale
            debugWindowCanvasScaler.referenceResolution = referenceResolution;

            // Return a customized debug window canvas scaler.
            return debugWindowCanvasScaler;
        }

        // On drag.
        private void DragWindow(Vector2 dragPosition)
        {
            // Check if window is not created and return.
            if (windowCreated == false) return;

            // Set window position.
            debugWindow.anchoredPosition += dragPosition;
        }

        // Close window.
        private void CloseWindow()
        {
            // Set window state.
            windowOpen = !windowOpen;

            // Set window state.
            debugWindow.gameObject.SetActive(windowOpen);
        }

        // Clear logs.
        private void ClearLogs()
        {
            // Check if there are no logs to clear and return.
            if (logAllCount <= 0) return;

            // Loop through all logs.
            for (int i = 0; i < logAllCount; i++)
            {
                // Delete log.
                Destroy(logPanelDeleteList[i]);
            }

            descriptiveLogWindowMessageDisplayer.text = defaultDesLogWinDisplayerInfo;

            // Clear content lists.
            logPanelDeleteList = new List<GameObject>();
            logMessageDeleteList = new List<string>();
            activeLogPanelList = new List<DebugData.LogPanel>();

            // Reset log counts.
            logInfoCount = 0;
            logWarningCount = 0;
            logErrorCount = 0;
            logAllCount = 0;

            // Update window content.
            UpdateWindowContent();
        }

        #endregion
    }
}
