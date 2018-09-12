using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ORKFramework;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.ORKFrameworkSupport;

namespace ORKFramework.Events.Steps
{

    /// <summary>
    /// This ORK step sets a Dialogue System quest or quest entry to a specified state.
    /// </summary>
    [ORKEditorHelp("Set Quest State", "Sets a quest state or quest entry state.", "")]
    [ORKEventStep(typeof(BaseEvent))]
    [ORKNodeInfo("Dialogue System Steps")]
    public class SetQuestStateStep : BaseEventStep
    {

        [ORKEditorHelp("Quest Name", "The name of the quest as defined in your dialogue database.", "")]
        [ORKEditorInfo(labelText = "Quest Name", expandWidth = true)]
        public StringValue questName = new StringValue();

        [ORKEditorHelp("Set Quest Entry State", "Tick to set a quest entry's state instead of the quest's state.", "")]
        [ORKEditorInfo(labelText = "Set Quest Entry State")]
        public bool setQuestEntryState = false;

        [ORKEditorHelp("Quest Entry Number", "If Set Quest Entry State is ticked, set this entry.", "")]
        [ORKEditorInfo(labelText = "Quest Entry Number")]
        public FloatValue questEntryNumber = new FloatValue();

        [ORKEditorHelp("State", "The new state.", "")]
        [ORKEditorInfo(labelText = "New State")]
        public QuestState questState = QuestState.Unassigned;

        public SetQuestStateStep()
        {
        }

        public override void Execute(BaseEvent baseEvent)
        {
            if (setQuestEntryState)
            {
                var entryNum = (int)questEntryNumber.GetValue(null, null);
                if (DialogueDebug.LogInfo) Debug.Log("Dialogue System: ORK Step Set Quest State: '" + questName.GetValue() + "' entry #" + entryNum + " to " + questState);
                QuestLog.SetQuestEntryState(questName.GetValue(), entryNum, questState);
            }
            else
            {
                if (DialogueDebug.LogInfo) Debug.Log("Dialogue System: ORK Step Set Quest State: '" + questName.GetValue() + "' to " + questState);
                QuestLog.SetQuestState(questName.GetValue(), questState);
            }
            baseEvent.StepFinished(this.next);
        }

        public override string GetNodeDetails()
        {
            if (setQuestEntryState)
            {
                var entryNum = (int)questEntryNumber.GetValue(null, null);
                return questName.value + " entry #" + entryNum + " = " + questState;
            }
            {
                return questName.value + " = " + questState;
            }
        }

    }
}
