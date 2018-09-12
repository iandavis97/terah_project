using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelCrushers.DialogueSystem.ORKFrameworkSupport
{

    /// <summary>
    /// This class fixes up ORK's EventSystem for use with general Unity UI navigation,
    /// including Unity UI Dialogue UIs.
    /// </summary>
    [AddComponentMenu("Dialogue System/Third Party/ORK Framework/Unity UI Dialogue UI ORK Bridge (Dialogue Manager)")]
    public class UnityUIDialogueUIOrkBridge : MonoBehaviour
    {

        private EventSystem eventSystem;

        public void FixEventSystem()
        {
            eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                Debug.LogWarning("Dialogue System: Unity UI Dialogue UI ORK Bridge can't find an EventSystem.");
                return;
            }
            eventSystem.sendNavigationEvents = true;
            if (eventSystem.GetComponent<StandaloneInputModule>() == null)
            {
                eventSystem.gameObject.AddComponent<StandaloneInputModule>();
            }
        }

        public void UnfixEventSystem()
        {
            if (eventSystem != null) Destroy(eventSystem.GetComponent<StandaloneInputModule>());
        }

        void OnConversationStart(Transform actor)
        {
            FixEventSystem();
        }

        void OnConversationEnd(Transform actor)
        {
            UnfixEventSystem();
        }
    }
}