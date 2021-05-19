// Used libraries.
using UnityEngine;
using UnityEngine.EventSystems;

// Namespace.
namespace RedicalGamez.Dev.ToolKit
{
    // Disallow multiple components.
    [DisallowMultipleComponent]

    // Draggable window class.
    public class DraggableWindow : MonoBehaviour, IDragHandler
    {
        // Window.
        [SerializeField]
        private RectTransform draggableWindow;

        // Start.
        private void Start()
        {
            draggableWindow = this.GetComponent<RectTransform>();
        }

        // On drag.
        public void OnDrag(PointerEventData ped)
        {
            // Dragging.
            Debugger.Log(DebugData.LogType.LogInfo, this, $"--->>><color=orange>Dragging mouse at pos : </color><color=white>{ped.delta.ToString()}</color>");

            draggableWindow.anchoredPosition += ped.delta;
        }
    }
}
