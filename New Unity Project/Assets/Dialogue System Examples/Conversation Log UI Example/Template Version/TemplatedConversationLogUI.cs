using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Use this instead of Unity UI Dialogue UI to allow scrollback using templates for each line.
    /// </summary>
    [AddComponentMenu("Dialogue System/UI/Unity UI/Dialogue/Templated Conversation Log UI")]
    public class TemplatedConversationLogUI : UnityUIDialogueUI
    {

        [Tooltip("The scrollbar for the scroll rect.")]
        public UnityEngine.UI.Scrollbar scrollbar;

        [Tooltip("The scroll rect containing the content panel.")]
        public UnityEngine.UI.ScrollRect scrollRect;

        [Tooltip("The content panel inside the scroll rect containing the message panel and response panel.")]
        public RectTransform contentPanel;

        [Tooltip("Add messages to this panel.")]
        public RectTransform messagePanel;

        [Tooltip("Jump to bottom when showing subtitles.")]
        public bool autoScrollSubtitles = true;

        [Tooltip("Jump to bottom when showing response menu.")]
        public bool autoScrollResponseMenu = true;

        [Tooltip("Speed at which to smoothly scroll down.")]
        public float scrollSpeed = 1f;

        [Tooltip("If non-zero, drop older messages when the number of messages in the history reaches this value.")]
        public int maxMessages = 0;

        private List<GameObject> instantiatedMessages = new List<GameObject>();
        private Coroutine scrollCoroutine = null;

        public override void Close()
        {
            base.Close();
            DestroyInstantiatedMessages(); // Clean up.
        }

        public override void ShowSubtitle(Subtitle subtitle)
        {
            if (subtitle.dialogueEntry.id == 0) return; // Don't need to show START entry.
            if (findActorOverrides && subtitle != null)
            {
                var overrideControls = (subtitle.speakerInfo != null) ? FindActorOverride(subtitle.speakerInfo.transform) : null;
                if (subtitle.speakerInfo == null || subtitle.speakerInfo.characterType == CharacterType.NPC)
                {
                    dialogue.npcSubtitle = (overrideControls != null) ? overrideControls.subtitle : originalNPCSubtitle;
                }
                else
                {
                    dialogue.pcSubtitle = (overrideControls != null) ? overrideControls.subtitle : originalPCSubtitle;
                }
            }
            AddMessage(subtitle, subtitle.speakerInfo.IsNPC ? dialogue.npcSubtitle : dialogue.pcSubtitle);
        }

        public override void HideSubtitle(Subtitle subtitle)
        {
            // Don't hide the subtitle.
        }

        public override void ShowResponses(Subtitle subtitle, Response[] responses, float timeout)
        {
            base.ShowResponses(subtitle, responses, timeout);
            if (autoScrollResponseMenu) ScrollToBottom();
        }

        /// <summary>
        /// Adds the subtitle as a message in the UI.
        /// </summary>
        private void AddMessage(Subtitle subtitle, UnityUISubtitleControls template)
        {
            
            template.SetSubtitle(subtitle);
            template.SetActive(true);
            var go = (template.panel != null) ? Instantiate<GameObject>(template.panel.gameObject)
                : Instantiate<GameObject>(template.line.gameObject);
            go.transform.SetParent(messagePanel.transform, false);
            instantiatedMessages.Add(go);
            template.DeactivateUIElements(); // Updated for DS 1.8.1+.
            var typewriterEffect = go.GetComponentInChildren<UnityUITypewriterEffect>();
            if (typewriterEffect != null)
            {
                typewriterEffect.gameObject.SetActive(true);
                typewriterEffect.PlayText(subtitle.formattedText.text);
            }

            if (maxMessages > 0 && instantiatedMessages.Count > maxMessages)
            {
                Destroy(instantiatedMessages[0]);
                instantiatedMessages.RemoveAt(0);
            }
            if (autoScrollSubtitles) ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            if (scrollCoroutine != null) StopCoroutine(scrollCoroutine);
            scrollCoroutine = StartCoroutine(ScrollToBottomCoroutine());
        }

        IEnumerator ScrollToBottomCoroutine()
        {
            if (scrollRect == null) yield break;
            yield return null;
            var contentHeight = contentPanel.rect.height;
            var scrollRectHeight = scrollRect.GetComponent<RectTransform>().rect.height;
            var needToScroll = contentHeight > scrollRectHeight;
            scrollbar.gameObject.SetActive(needToScroll);
            if (needToScroll)
            {
                var ratio = scrollRectHeight / contentHeight;
                var timeout = Time.time + 10f; // Avoid infinite loops by maxing out at 10 seconds.
                while (scrollRect.verticalNormalizedPosition > 0.01f && Time.time < timeout)
                {
                    var newPos = scrollRect.verticalNormalizedPosition - scrollSpeed * Time.deltaTime * ratio;
                    scrollRect.verticalNormalizedPosition = Mathf.Max(0, newPos);
                    yield return null;
                }
            }
            scrollRect.verticalNormalizedPosition = 0;
            scrollbar.value = 0;
            scrollCoroutine = null;
        }

        private void DestroyInstantiatedMessages()
        {
            for (int i = 0; i < instantiatedMessages.Count; i++)
            {
                Destroy(instantiatedMessages[i]);
            }
            instantiatedMessages.Clear();
        }

    }
}
