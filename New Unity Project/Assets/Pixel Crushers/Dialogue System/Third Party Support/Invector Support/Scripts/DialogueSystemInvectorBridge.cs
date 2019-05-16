using UnityEngine;
using System.Collections;
using Invector.vCharacterController;
using System;

namespace PixelCrushers.DialogueSystem.InvectorSupport
{

    /// <summary>
    /// Correctly stops and restarts the Invector player character during
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

        private static bool s_registeredLuaFunctions = false;

        private void Awake()
        {
            m_controller = GetComponent<vThirdPersonController>();
            m_input = GetComponent<vThirdPersonInput>();
            m_animator = GetComponent<Animator>();
            m_rb = GetComponent<Rigidbody>();
            if (!s_registeredLuaFunctions)
            {
                s_registeredLuaFunctions = true;
                RegisterLuaFunctions();
            }
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

#if USE_INVECTOR_INVENTORY

        private void RegisterLuaFunctions()
        {
            Lua.RegisterFunction("vAddItemByID", this, SymbolExtensions.GetMethodInfo(() => vAddItemByID((double)0, (double)0, false)));
            Lua.RegisterFunction("vGetHealth", this, SymbolExtensions.GetMethodInfo(() => vGetHealth()));
            Lua.RegisterFunction("vGetMaxHealth", this, SymbolExtensions.GetMethodInfo(() => vGetMaxHealth()));
            Lua.RegisterFunction("vAddHealth", this, SymbolExtensions.GetMethodInfo(() => vAddHealth((double)0)));
            Lua.RegisterFunction("vAddMaxHealth", this, SymbolExtensions.GetMethodInfo(() => vAddMaxHealth((double)0)));
        }

        private void UnregisterLuaFunctions()
        {
            Lua.UnregisterFunction("vAddItemByID");
            Lua.UnregisterFunction("vGetHealth");
            Lua.UnregisterFunction("vGetMaxHealth");
            Lua.UnregisterFunction("vAddHealth");
            Lua.UnregisterFunction("vAddMaxHealth");
        }

        public void vAddItemByID(double id, double amount, bool autoEquip)
        {
            var itemManager = GetComponent<Invector.vItemManager.vItemManager>();
            if (itemManager)
            {
                var reference = new Invector.vItemManager.ItemReference((int)id);
                reference.amount = (int)amount;
                reference.autoEquip = autoEquip;
                itemManager.AddItem(reference);
            }
        }

#else

        private void RegisterLuaFunctions()
        {
            Lua.RegisterFunction("vGetHealth", this, SymbolExtensions.GetMethodInfo(() => vGetHealth()));
            Lua.RegisterFunction("vGetMaxHealth", this, SymbolExtensions.GetMethodInfo(() => vGetMaxHealth()));
            Lua.RegisterFunction("vAddHealth", this, SymbolExtensions.GetMethodInfo(() => vAddHealth((double)0)));
            Lua.RegisterFunction("vAddMaxHealth", this, SymbolExtensions.GetMethodInfo(() => vAddMaxHealth((double)0)));
        }

        private void UnregisterLuaFunctions()
        {
            Lua.UnregisterFunction("vGetHealth");
            Lua.UnregisterFunction("vGetMaxHealth");
            Lua.UnregisterFunction("vAddHealth");
            Lua.UnregisterFunction("vAddMaxHealth");
        }

#endif

        private double vGetHealth()
        {
            return (m_controller != null) ? m_controller.currentHealth : 0;
        }

        private double vGetMaxHealth()
        {
            return (m_controller != null) ? m_controller.maxHealth : 0;
        }

        private void vAddHealth(double v)
        {
            if (m_controller != null) m_controller.ChangeHealth((int)v);
        }

        private void vAddMaxHealth(double v)
        {
            if (m_controller != null) m_controller.ChangeMaxHealth((int)v);
        }

    }
}
