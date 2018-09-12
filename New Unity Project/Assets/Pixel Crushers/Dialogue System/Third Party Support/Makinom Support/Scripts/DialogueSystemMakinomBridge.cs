using UnityEngine;
using Makinom;

namespace PixelCrushers.DialogueSystem.MakinomSupport
{

    /// <summary>
    /// This class provides a bridge between the Dialogue System and
    /// Makinom. Add it to you Dialogue Manager GameObject.
    /// 
    /// This class also implements ISaveData to save and load the Dialogue
    /// System's data with Makinom saved games.
    /// </summary>
    [AddComponentMenu("Pixel Crushers/Dialogue System/Third Party/Makinom/Dialogue System Makinom Bridge (Dialogue Manager)")]
    public class DialogueSystemMakinomBridge : MonoBehaviour, ISaveData
    {

        public bool useMakinomLanguage = true;

        public bool useMakinomSaveSystem = true;
        public string SaveDataTag = "MakinomDialogueSystem";
        public string SaveDataKey = "DialogueSystemData";

        private bool started = false;
        private bool registeredSaveData = false;

        public virtual void Start()
        {
            started = true;
            RegisterSaveData();
            UseMakinomLanguage();
        }

        public virtual void OnEnable()
        {
            if (started) RegisterSaveData();
        }

        public virtual void OnDisable()
        {
            UnregisterSaveData();
        }

        private void RegisterSaveData()
        {
            if (useMakinomSaveSystem && !registeredSaveData)
            {
                registeredSaveData = true;
                Maki.SaveGame.RegisterCustomData(SaveDataTag, this, false);
            }
        }

        private void UnregisterSaveData()
        {
            if (registeredSaveData)
            {
                registeredSaveData = false;
                Maki.SaveGame.UnregisterCustomData(SaveDataTag, false);
            }
        }

        public DataObject SaveGame()
        {
            var data = new DataObject();
            data.Set(SaveDataKey, PersistentDataManager.GetSaveData());
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
                data.Get(SaveDataKey, ref savedata);
                PersistentDataManager.ApplySaveData(savedata);
            }
        }

        private void UseMakinomLanguage()
        {
            if (useMakinomLanguage)
            {
                Localization.Language = Maki.Languages.GetName(Maki.Game.Language);
            }
        }

    }
}
