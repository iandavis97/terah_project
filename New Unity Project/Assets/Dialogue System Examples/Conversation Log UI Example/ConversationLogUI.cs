using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Add this to a Unity UI Dialogue UI to configure it as a conversation logger.
    /// This means it will log all text in the conversation so the player can
    /// scroll back to review it.
    /// </summary>
    [AddComponentMenu("Dialogue System/UI/Unity UI/Dialogue/Conversation Log UI")]
    public class ConversationLogUI : MonoBehaviour
    {

        [Tooltip("Show the speaker's name in front of the dialogue text")]
        public bool showSpeakerName = true;

        [Tooltip("If Show Speaker Name is ticked, use this separator between the name and dialogue text")]
        public string nameSeparator = ": ";

        [Tooltip("Color to use for NPC lines")]
        public Color npcColor = new Color(0.2f, 0.08f, 0.08f);

        [Tooltip("Color to use for PC lines")]
        public Color pcColor = Color.blue;

        [Tooltip("Text element that will contain the conversation log")]
        public UnityEngine.UI.Text conversationText;

        [Tooltip("Scroll Rect that holds the conversation text")]
        public UnityEngine.UI.ScrollRect scrollRect;

        void Awake()
        {
            if (conversationText == null && DialogueDebug.LogWarnings) Debug.LogWarning("Dialogue System: Need to assign Conversation Text", this);
            if (scrollRect == null && DialogueDebug.LogWarnings) Debug.LogWarning("Dialogue System: Need to assign Scroll Rect", this);
        }

        void OnConversationStart(Transform actor)
        {
            if (conversationText == null) return;
            conversationText.text = string.Empty;
        }

        void OnConversationLine(Subtitle subtitle)
        {
            if (conversationText == null || subtitle == null || string.IsNullOrEmpty(subtitle.formattedText.text)) return;
            var webColor = (subtitle.speakerInfo.characterType == CharacterType.NPC) ? Tools.ToWebColor(npcColor) : Tools.ToWebColor(pcColor);
            var text = subtitle.formattedText.text;
            if (showSpeakerName) text = subtitle.speakerInfo.Name + nameSeparator + text;
            if (!text.Contains("<color")) text = string.Format("<color={0}>{1}</color>", webColor, text);
            if (!string.IsNullOrEmpty(conversationText.text)) conversationText.text += "\n";
            conversationText.text += text;
            StartCoroutine(JumpToBottom());
        }
        
        void OnConversationResponseMenu(Response[] responses)
        {
            StartCoroutine(JumpToBottom());
        }

        IEnumerator JumpToBottom()
        {
            if (scrollRect == null) yield break;
            yield return null;
            scrollRect.verticalNormalizedPosition = 0;
        }

    }
}
