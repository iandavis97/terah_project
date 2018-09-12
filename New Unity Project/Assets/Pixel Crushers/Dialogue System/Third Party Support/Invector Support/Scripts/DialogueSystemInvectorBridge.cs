using UnityEngine;
using System.Collections;
using Invector.vCharacterController;

namespace PixelCrushers.DialogueSystem.InvectorSupport
{

    /// <summary>
    /// Correct stops and restarts the Invector player character during
    /// conversations.
    /// </summary>
    [AddComponentMenu("Dialogue System/Third Party/Invector/Dialogue System Invector Bridge")]
    public class DialogueSystemInvectorBridge : MonoBehaviour
    {

        [Tooltip("Face the other conversation participant when starting a conversation.")]
        public bool faceConversant = true;

        private vThirdPersonController m_controller = null;
        private vThirdPersonInput m_input = null;
        private Animator m_animator = null;
        private Rigidbody m_rb = null;

        private void Awake()
        {
            m_controller = GetComponent<vThirdPersonController>();
            m_input = GetComponent<vThirdPersonInput>();
            m_animator = GetComponent<Animator>();
            m_rb = GetComponent<Rigidbody>();
        }

        private void OnConversationStart(Transform other)
        {
            if (m_controller != null) m_controller.enabled = false;
            if (m_input != null) m_input.enabled = false;
            StartCoroutine(StopCharacter(other));
        }

        private void OnConversationEnd(Transform other)
        {
            if (m_controller != null) m_controller.enabled = true;
            if (m_input != null) m_input.enabled = true;
        }

        private IEnumerator StopCharacter(Transform other)
        {
            var elapsed = 0f;
            while (elapsed < 0.1f)
            {
                if (m_rb != null)
                {
                    m_rb.velocity *= 0.5f;
                    m_rb.angularVelocity *= 0.5f;
                }
                elapsed += Time.deltaTime;
                yield return null;
            }
            if (m_animator != null)
            {
                m_animator.SetFloat("InputVertical", 0);
                m_animator.SetFloat("InputHorizontal", 0);
                m_animator.SetFloat("InputMagnitude", 0);
            }
            if (faceConversant) transform.LookAt(other, Vector3.up);
        }
    }
}