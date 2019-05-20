using UnityEngine;
using ORKFramework;
using ORKFramework.Behaviours;

namespace PixelCrushers.DialogueSystem.ORKFrameworkSupport
{

    /// <summary>
    /// This class provides a bridge between the Dialogue System and
    /// ORK Framework. Add it to you Dialogue Manager GameObject.
    /// 
    /// This class also implements ISaveData to save and load the Dialogue
    /// System's data with ORK saved games.
    /// </summary>
    [AddComponentMenu("Pixel Crushers/Dialogue System/Third Party/ORK Framework/Dialogue System ORK Bridge (Dialogue Manager)")]
    public class DialogueSystemOrkBridge : MonoBehaviour, ISaveData
    {

        private bool started = false;
        private bool registeredSaveData = false;
        private static bool s_registeredLua = false;

        #region Initialization

        public virtual void Start()
        {
            started = true;
            RegisterSaveData();
        }

        public virtual void OnEnable()
        {
            if (started) RegisterSaveData();
            RegisterLuaFunctions();
        }

        public virtual void OnDisable()
        {
            UnregisterSaveData();
            //--- No need to unregister: UnregisterLuaFunctions();
        }

        public virtual void OnConversationStart(Transform actor)
        {
            SetComponentEnabled<ButtonPlayerController>(actor, false);
            SetComponentEnabled<MousePlayerController>(actor, false);
        }

        public virtual void OnConversationEnd(Transform actor)
        {
            SetComponentEnabled<ButtonPlayerController>(actor, true);
            SetComponentEnabled<MousePlayerController>(actor, true);
        }

        protected void SetComponentEnabled<T>(Transform actor, bool value) where T : MonoBehaviour
        {
            if (actor == null) return;
            T component = actor.GetComponent<T>();
            if (component != null)
            {
                component.enabled = value;
            }
        }

        #endregion

        #region Register Lua

        protected virtual void RegisterLuaFunctions()
        {
            if (s_registeredLua) return;
            s_registeredLua = true;

            // Variables:
            Lua.RegisterFunction("ORKGetBool", null, SymbolExtensions.GetMethodInfo(() => ORKGetBool(string.Empty)));
            Lua.RegisterFunction("ORKGetFloat", null, SymbolExtensions.GetMethodInfo(() => ORKGetFloat(string.Empty)));
            Lua.RegisterFunction("ORKGetString", null, SymbolExtensions.GetMethodInfo(() => ORKGetString(string.Empty)));
            Lua.RegisterFunction("ORKSetBool", null, SymbolExtensions.GetMethodInfo(() => ORKSetBool(string.Empty, false)));
            Lua.RegisterFunction("ORKSetFloat", null, SymbolExtensions.GetMethodInfo(() => ORKSetFloat(string.Empty, (double)0)));
            Lua.RegisterFunction("ORKSetString", null, SymbolExtensions.GetMethodInfo(() => ORKSetString(string.Empty, string.Empty)));
            // Status:
            Lua.RegisterFunction("ORKGetStatus", null, SymbolExtensions.GetMethodInfo(() => ORKGetStatus(string.Empty, string.Empty)));
            Lua.RegisterFunction("ORKSetStatus", null, SymbolExtensions.GetMethodInfo(() => ORKSetStatus(string.Empty, string.Empty, (double)0)));
            // Faction:
            Lua.RegisterFunction("ORKChangeFaction", null, SymbolExtensions.GetMethodInfo(() => ORKChangeFaction(string.Empty, string.Empty)));
            Lua.RegisterFunction("ORKGetFactionSympathy", null, SymbolExtensions.GetMethodInfo(() => ORKGetFactionSympathy(string.Empty, string.Empty)));
            Lua.RegisterFunction("ORKSetFactionSympathy", null, SymbolExtensions.GetMethodInfo(() => ORKSetFactionSympathy(string.Empty, string.Empty, (double)0)));
            Lua.RegisterFunction("ORKAddFactionSympathy", null, SymbolExtensions.GetMethodInfo(() => ORKAddFactionSympathy(string.Empty, string.Empty, (double)0)));
            Lua.RegisterFunction("ORKSubFactionSympathy", null, SymbolExtensions.GetMethodInfo(() => ORKSubFactionSympathy(string.Empty, string.Empty, (double)0)));
            // Quests:
            Lua.RegisterFunction("ORKHasQuest", null, SymbolExtensions.GetMethodInfo(() => ORKHasQuest(string.Empty)));
            Lua.RegisterFunction("ORKAddQuest", null, SymbolExtensions.GetMethodInfo(() => ORKAddQuest(string.Empty)));
            Lua.RegisterFunction("ORKRemoveQuest", null, SymbolExtensions.GetMethodInfo(() => ORKRemoveQuest(string.Empty)));
            Lua.RegisterFunction("ORKChangeQuestStatus", null, SymbolExtensions.GetMethodInfo(() => ORKChangeQuestStatus(string.Empty, string.Empty)));
            Lua.RegisterFunction("ORKChangeQuestTaskStatus", null, SymbolExtensions.GetMethodInfo(() => ORKChangeQuestTaskStatus(string.Empty, string.Empty)));
            // Inventory:
            Lua.RegisterFunction("ORKHasItem", null, SymbolExtensions.GetMethodInfo(() => ORKHasItem(string.Empty, string.Empty)));
            Lua.RegisterFunction("ORKAddItem", null, SymbolExtensions.GetMethodInfo(() => ORKAddItem(string.Empty, string.Empty)));
            Lua.RegisterFunction("ORKRemoveItem", null, SymbolExtensions.GetMethodInfo(() => ORKRemoveItem(string.Empty, string.Empty)));
            Lua.RegisterFunction("ORKHasItemQuantity", null, SymbolExtensions.GetMethodInfo(() => ORKHasItemQuantity(string.Empty, string.Empty, (double)0)));
            Lua.RegisterFunction("ORKAddItemQuantity", null, SymbolExtensions.GetMethodInfo(() => ORKAddItemQuantity(string.Empty, string.Empty, (double)0)));
            Lua.RegisterFunction("ORKRemoveItemQuantity", null, SymbolExtensions.GetMethodInfo(() => ORKRemoveItemQuantity(string.Empty, string.Empty, (double)0)));
            Lua.RegisterFunction("ORKGetMoney", null, SymbolExtensions.GetMethodInfo(() => ORKGetMoney(string.Empty, string.Empty)));
            Lua.RegisterFunction("ORKSetMoney", null, SymbolExtensions.GetMethodInfo(() => ORKSetMoney(string.Empty, string.Empty, (double)0)));
            // Events:
            Lua.RegisterFunction("ORKStartEvent", null, SymbolExtensions.GetMethodInfo(() => ORKStartEvent(string.Empty, string.Empty)));
        }

        protected virtual void UnregisterLuaFunctions()
        {
            // Variables:
            Lua.UnregisterFunction("ORKGetBool");
            Lua.UnregisterFunction("ORKGetFloat");
            Lua.UnregisterFunction("ORKGetString");
            Lua.UnregisterFunction("ORKSetBool");
            Lua.UnregisterFunction("ORKSetFloat");
            Lua.UnregisterFunction("ORKSetString");
            // Status:
            Lua.UnregisterFunction("ORKGetStatus");
            Lua.UnregisterFunction("ORKSetStatus");
            // Faction:
            Lua.UnregisterFunction("ORKChangeFaction");
            Lua.UnregisterFunction("ORKGetFactionSympathy");
            Lua.UnregisterFunction("ORKSetFactionSympathy");
            Lua.UnregisterFunction("ORKAddFactionSympathy");
            Lua.UnregisterFunction("ORKSubFactionSympathy");
            // Quests:
            Lua.UnregisterFunction("ORKHasQuest");
            Lua.UnregisterFunction("ORKAddQuest");
            Lua.UnregisterFunction("ORKRemoveQuest");
            Lua.UnregisterFunction("ORKChangeQuestStatus");
            Lua.UnregisterFunction("ORKChangeQuestTaskStatus");
            // Inventory:
            Lua.UnregisterFunction("ORKHasItem");
            Lua.UnregisterFunction("ORKAddItem");
            Lua.UnregisterFunction("ORKRemoveItem");
            Lua.UnregisterFunction("ORKHasItemQuantity");
            Lua.UnregisterFunction("ORKAddItemQuantity");
            Lua.UnregisterFunction("ORKRemoveItemQuantity");
            Lua.UnregisterFunction("ORKGetMoney");
            Lua.UnregisterFunction("ORKSetMoney");
            // Events:
            Lua.UnregisterFunction("ORKStartEvent");
        }

        #endregion

        #region Variables

        public static bool ORKGetBool(string key)
        {
            return ORKFramework.ORK.Game.Variables.GetBool(key);
        }

        public static double ORKGetFloat(string key)
        {
            return ORKFramework.ORK.Game.Variables.GetFloat(key);
        }

        public static string ORKGetString(string key)
        {
            return ORKFramework.ORK.Game.Variables.GetString(key);
        }

        public static void ORKSetBool(string key, bool value)
        {
            ORKFramework.ORK.Game.Variables.Set(key, value);
        }

        public static void ORKSetFloat(string key, double value)
        {
            ORKFramework.ORK.Game.Variables.Set(key, (float)value);
        }

        public static void ORKSetString(string key, string value)
        {
            ORKFramework.ORK.Game.Variables.Set(key, value);
        }

        #endregion

        #region Status

        public static double ORKGetStatus(string combatantName, string statusValueName)
        {
            var statusValue = GetStatusValue(combatantName, statusValueName);
            if (statusValue == null) return 0;
            return statusValue.GetValue();
        }

        public static void ORKSetStatus(string combatantName, string statusValueName, double value)
        {
            int intValue = (int)value;
            var statusValue = GetStatusValue(combatantName, statusValueName);
            if (statusValue == null) return;
            statusValue.SetValue(intValue, false, true, true, true, new StatusValueChangeSource(GetCombatant(combatantName)));
        }

        #endregion

        #region Events

        public static void ORKStartEvent(string eventObjectName, string startingObjectName)
        {
            var eventObject = FindGameObject(eventObjectName);
            var eventInteraction = (eventObject != null) ? eventObject.GetComponent<EventInteraction>() : null;
            if (eventInteraction == null) return;
            var startingObject = FindGameObject(startingObjectName);
            if (startingObject == null) startingObject = ORKFramework.ORK.Game.GetPlayer();
            eventInteraction.StartEvent(startingObject);
        }

        #endregion

        #region Faction

        public static void ORKChangeFaction(string combatantName, string newFactionName)
        {
            var combatant = GetCombatant(combatantName);
            combatant.Group.FactionID = GetFactionID(newFactionName);
        }

        public static double ORKGetFactionSympathy(string factionName, string otherName)
        {
            return ORKFramework.ORK.Game.Faction.GetSympathy(GetFactionID(factionName), GetFactionID(otherName));
        }

        public static void ORKSetFactionSympathy(string factionName, string otherName, double amount)
        {
            ORKChangeFactionSympathy(factionName, otherName, amount, SimpleOperator.Set);
        }

        public static void ORKAddFactionSympathy(string factionName, string otherName, double amount)
        {
            ORKChangeFactionSympathy(factionName, otherName, amount, SimpleOperator.Add);
        }

        public static void ORKSubFactionSympathy(string factionName, string otherName, double amount)
        {
            ORKChangeFactionSympathy(factionName, otherName, amount, SimpleOperator.Sub);
        }

        public static void ORKChangeFactionSympathy(string factionName, string otherName, double change, SimpleOperator op)
        {
            ORKFramework.ORK.Game.Faction.ChangeSympathy(GetFactionID(factionName), GetFactionID(otherName), (float)change, op);
        }

        #endregion

        #region Quests

        public static bool ORKHasQuest(string questName)
        {
            return ORKFramework.ORK.Game.Quests.HasQuest(GetQuestID(questName));
        }

        public static void ORKAddQuest(string questName)
        {
            ORKFramework.ORK.Game.Quests.AddQuest(GetQuestID(questName), true, true);
        }

        public static void ORKRemoveQuest(string questName)
        {
            ORKFramework.ORK.Game.Quests.RemoveQuest(GetQuestID(questName), true);
        }

        public static void ORKChangeQuestStatus(string questName, string status)
        {
            Quest quest = ORKFramework.ORK.Game.Quests.GetQuest(GetQuestID(questName));
            if (quest == null) return;
            if (string.Equals(status, "inactive", System.StringComparison.OrdinalIgnoreCase))
            {
                quest.SetInactive(ORKFramework.ORK.Game.ActiveGroup.Leader, true, true);
            }
            else if (string.Equals(status, "active", System.StringComparison.OrdinalIgnoreCase))
            {
                quest.SetActive(ORKFramework.ORK.Game.ActiveGroup.Leader, true, true);
            }
            else if (string.Equals(status, "finished", System.StringComparison.OrdinalIgnoreCase))
            {
                quest.SetFinished(ORKFramework.ORK.Game.ActiveGroup.Leader, true, true, true, false, true);
            }
            else if (string.Equals(status, "failed", System.StringComparison.OrdinalIgnoreCase))
            {
                quest.SetFailed(ORKFramework.ORK.Game.ActiveGroup.Leader, true, true);
            }
            if (DialogueDebug.LogWarnings)
            {
                Debug.LogWarning("Dialogue System: ORKChangeQuestStatus: invalid status '" + status + "'");
            }
        }

        public static void ORKChangeQuestTaskStatus(string questTaskName, string status)
        {
            QuestTask task = ORKFramework.ORK.Game.Quests.GetTask(GetQuestTaskID(questTaskName));
            if (task == null) return;
            if (string.Equals(status, "inactive", System.StringComparison.OrdinalIgnoreCase))
            {
                task.SetInactive(ORKFramework.ORK.Game.ActiveGroup.Leader, true, true);
            }
            else if (string.Equals(status, "active", System.StringComparison.OrdinalIgnoreCase))
            {
                task.SetActive(ORKFramework.ORK.Game.ActiveGroup.Leader, true, true);
            }
            else if (string.Equals(status, "finished", System.StringComparison.OrdinalIgnoreCase))
            {
                task.SetFinished(ORKFramework.ORK.Game.ActiveGroup.Leader, true, true, true, true);
            }
            else if (string.Equals(status, "failed", System.StringComparison.OrdinalIgnoreCase))
            {
                task.SetFailed(ORKFramework.ORK.Game.ActiveGroup.Leader, true, true);
            }
            else if (DialogueDebug.LogWarnings)
            {
                Debug.LogWarning("Dialogue System: ORKChangeQuestTaskStatus: invalid status '" + status + "'");
            }
        }

        #endregion

        #region Inventory

        public static bool ORKHasItem(string combatantName, string itemName)
        {
            return ORKHasItemQuantity(combatantName, itemName, 1);
        }

        public static void ORKAddItem(string combatantName, string itemName)
        {
            ORKAddItemQuantity(combatantName, itemName, 1);
        }

        public static void ORKRemoveItem(string combatantName, string itemName)
        {
            ORKRemoveItemQuantity(combatantName, itemName, 1);
        }

        public static bool ORKHasItemQuantity(string combatantName, string itemName, double quantity)
        {
            var combatant = GetCombatant(combatantName);
            if (combatant == null) return false;
            
            // Check items:
            for (int i = 0; i < ORKFramework.ORK.Items.Count; i++)
            {
                if (string.Equals(ORKFramework.ORK.Items.GetName(i), itemName))
                {
                    return combatant.Inventory.Has(new ItemShortcut(i, 1), (int)quantity);
                }
            }
            // Check weapons:
            for (int i = 0; i < ORKFramework.ORK.Weapons.Count; i++)
            {
                if (string.Equals(ORKFramework.ORK.Weapons.GetName(i), itemName))
                {
                    return combatant.Inventory.Weapons.Has(i, (int)quantity);
                }
            }
            // Check armor:
            for (int i = 0; i < ORKFramework.ORK.Armors.Count; i++)
            {
                if (string.Equals(ORKFramework.ORK.Armors.GetName(i), itemName))
                {
                    return combatant.Inventory.Armors.Has(i, (int)quantity);
                }
            }
            return false;
        }

        public static void ORKAddItemQuantity(string combatantName, string itemName, double quantity)
        {
            var combatant = GetCombatant(combatantName);
            if (combatant == null) return;

            // Try adding item:
            for (int i = 0; i < ORKFramework.ORK.Items.Count; i++)
            {
                if (string.Equals(ORKFramework.ORK.Items.GetName(i), itemName))
                {
                    combatant.Inventory.Add(new ItemShortcut(i, (int)quantity), true, true, true);
                    return;
                }
            }
            // Try adding weapon:
            for (int i = 0; i < ORKFramework.ORK.Weapons.Count; i++)
            {
                if (string.Equals(ORKFramework.ORK.Weapons.GetName(i), itemName))
                {
                    combatant.Inventory.Weapons.Add(new EquipShortcut(EquipSet.Weapon, i, 1, (int)quantity), true, true, true);
                    return;
                }
            }
            // Try adding armor:
            for (int i = 0; i < ORKFramework.ORK.Armors.Count; i++)
            {
                if (string.Equals(ORKFramework.ORK.Armors.GetName(i), itemName))
                {
                    combatant.Inventory.Armors.Add(new EquipShortcut(EquipSet.Armor, i, 1, (int)quantity), true, true, true);
                    return;
                }
            }
        }

        public static void ORKRemoveItemQuantity(string combatantName, string itemName, double quantity)
        {
            var combatant = GetCombatant(combatantName);
            if (combatant == null) return;
            // Try removing item:
            for (int i = 0; i < ORKFramework.ORK.Items.Count; i++)
            {
                if (string.Equals(ORKFramework.ORK.Items.GetName(i), itemName))
                {
                    combatant.Inventory.Remove(new ItemShortcut(i, 1), (int)quantity, true, true);
                    return;
                }
            }
            // Try removing weapon:
            for (int i = 0; i < ORKFramework.ORK.Weapons.Count; i++)
            {
                if (string.Equals(ORKFramework.ORK.Weapons.GetName(i), itemName))
                {
                    combatant.Inventory.RemoveEquipment(new EquipShortcut(EquipSet.Weapon, i, 1, 1), (int)quantity, true, true);
                    return;
                }
            }
            // Try removing armor:
            for (int i = 0; i < ORKFramework.ORK.Armors.Count; i++)
            {
                if (string.Equals(ORKFramework.ORK.Armors.GetName(i), itemName))
                {
                    combatant.Inventory.RemoveEquipment(new EquipShortcut(EquipSet.Armor, i, 1, 1), (int)quantity, true, true);
                    return;
                }
            }
        }

        public static double ORKGetMoney(string combatantName, string currencyName)
        {
            var combatant = GetCombatant(combatantName);
            if (combatant == null) return 0;
            return combatant.Inventory.GetMoney(GetCurrencyIndex(currencyName));
        }

        public static void ORKSetMoney(string combatantName, string currencyName, double quantity)
        {
            var combatant = GetCombatant(combatantName);
            if (combatant == null) return;
            combatant.Inventory.SetMoney(GetCurrencyIndex(currencyName), (int)quantity, true, true);
        }

        #endregion

        #region Lua Helper Methods

        protected static Combatant GetCombatant(string combatantName)
        {
            if (string.Equals(combatantName, "player", System.StringComparison.OrdinalIgnoreCase))
            {
                return ORKFramework.ORK.Game.ActiveGroup.Leader;
            }
            else
            {
                return ComponentHelper.GetCombatant(FindGameObject(combatantName));
            }
        }

        protected static GameObject FindGameObject(string gameObjectName)
        {
            var go = Tools.GameObjectHardFind(gameObjectName);
            if (go == null) go = Tools.GameObjectHardFind(gameObjectName + "(Clone)");
            return go;
        }

        protected static StatusValue GetStatusValue(string combatantName, string statusValueName)
        {
            var combatant = GetCombatant(combatantName);
            if (combatant == null) return null;
            for (int i = 0; i < ORKFramework.ORK.StatusValues.Count; i++)
            {
                if (string.Equals(ORKFramework.ORK.StatusValues.GetName(i), statusValueName))
                {
                    return combatant.Status[i];
                }
            }
            if (DialogueDebug.LogWarnings) Debug.LogWarning("Dialogue System: Couldn't find status value '" + statusValueName + "' for ORK combatant '" + combatantName + "'");
            return null;
        }

        protected static int GetFactionID(string factionName)
        {
            for (int i = 0; i < ORKFramework.ORK.Factions.Count; i++)
            {
                if (string.Equals(ORKFramework.ORK.Factions.GetName(i), factionName))
                {
                    return i;
                }
            }
            if (DialogueDebug.LogWarnings) Debug.LogWarning("Dialogue System: Couldn't find ORK faction ID for faction named '" + factionName + "'");
            return 0;
        }

        protected static int GetQuestID(string questName)
        {
            for (int i = 0; i < ORKFramework.ORK.Quests.Count; i++)
            {
                if (string.Equals(ORKFramework.ORK.Quests.GetName(i), questName))
                {
                    return i;
                }
            }
            if (DialogueDebug.LogWarnings) Debug.LogWarning("Dialogue System: Couldn't find ORK quest ID for quest named '" + questName + "'");
            return 0;
        }

        protected static int GetQuestTaskID(string questTaskName)
        {
            for (int i = 0; i < ORKFramework.ORK.QuestTasks.Count; i++)
            {
                if (string.Equals(ORKFramework.ORK.QuestTasks.GetName(i), questTaskName))
                {
                    return i;
                }
            }
            if (DialogueDebug.LogWarnings) Debug.LogWarning("Dialogue System: Couldn't find ORK quest task ID for quest task named '" + questTaskName + "'");
            return 0;
        }

        protected static int GetItemIndex(string itemName)
        {
            for (int i = 0; i < ORKFramework.ORK.Items.Count; i++)
            {
                if (string.Equals(ORKFramework.ORK.Items.GetName(i), itemName))
                {
                    return i;
                }
            }
            if (DialogueDebug.LogWarnings) Debug.LogWarning("Dialogue System: Couldn't find ORK item index for item named '" + itemName + "'");
            return 0;
        }

        protected static ItemShortcut GetItemShortcut(string itemName)
        {
            return new ItemShortcut(GetItemIndex(itemName), 1);
        }

        protected static int GetCurrencyIndex(string currencyName)
        {
            for (int i = 0; i < ORKFramework.ORK.Currencies.Count; i++)
            {
                if (string.Equals(ORKFramework.ORK.Currencies.GetName(i), currencyName))
                {
                    return i;
                }
            }
            if (DialogueDebug.LogWarnings) Debug.LogWarning("Dialogue System: Couldn't find ORK currency index for currency named '" + currencyName + "'");
            return 0;
        }

        #endregion

        #region SaveData

        private void RegisterSaveData()
        {
            if (!registeredSaveData)
            {
                registeredSaveData = true;
                if (ORKFramework.ORK.SaveGame != null) ORKFramework.ORK.SaveGame.RegisterCustomData("DialogueSystemOrkBridge", this, false);
            }
        }

        private void UnregisterSaveData()
        {
            if (registeredSaveData)
            {
                registeredSaveData = false;
                if (ORKFramework.ORK.SaveGame != null) ORKFramework.ORK.SaveGame.UnregisterCustomData("DialogueSystemOrkBridge", false);
            }
        }

        public DataObject SaveGame()
        {
            var data = new DataObject();
            data.Set("savedata", PersistentDataManager.GetSaveData());
            return data;
        }

        public void LoadGame(DataObject data)
        {
            if (data == null)
            {
                DialogueManager.ResetDatabase(DatabaseResetOptions.KeepAllLoaded);
            }
            else
            {
                string savedata = string.Empty;
                data.Get("savedata", ref savedata);
                PersistentDataManager.ApplySaveData(savedata);
            }
        }

        #endregion

    }
}
