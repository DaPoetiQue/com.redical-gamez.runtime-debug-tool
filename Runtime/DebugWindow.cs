// Used libraries.
using UnityEngine;

// Namespace.
namespace RedicalGamez.Dev.ToolKit
{
    // Disallow multiple components.
    [DisallowMultipleComponent]

    // Debug window class.
    public abstract class DebugWindow : MonoBehaviour
    {
        // Properties.
        #region Properties

        // Enable logs.
        [SerializeField]
        protected bool enableLogs; 

        #endregion
    }
}
