using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ORKFramework;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.ORKFrameworkSupport;

namespace ORKFramework.Events.Steps
{

    /// <summary>
    /// This ORK step gets the state of a Dialogue System quest or quest entry.
    /// </summary>
    [ORKEditorHelp("Get Quest State", "Gets the state of a quest or quest entry.", "")]
    [ORKEventStep(typeof(BaseEvent))]
    [ORKNodeInfo("Dialogue System Steps")]
    public class GetQuestStateStep : BaseEventStep
    {

        [ORKEditorHelp("Quest Name", "The name of the quest as defined in your dialogue database.", "")]
        [ORKEditorInfo(labelText = "Quest Name", expandWidth = true)]
        public StringValue questName = new StringValue();

        [ORKEditorHelp("Get Quest Entry State", "Tick to get a quest entry's state instead of the quest's state.", "")]
        [ORKEditorInfo(labelText = "Get Quest Entry State")]
        public bool getQuestEntryState = false;

        [ORKEditorHelp("Quest Entry Number", "If Set Quest Entry State is ticked, get the state of this entry.", "")]
        [ORKEditorInfo(labelText = "Quest Entry Number")]
        public FloatValue questEntryNumber = new FloatValue();

        // Result variable
        [ORKEditorHelp("Variable Origin", "Select the origin of the variables:\n" +
                       "- Local: Local variables are only used in a running event and don't interfere with global variables. " +
                       "The variable will be gone once the event ends.\n" +
                       "- Global: Global variables are persistent and available everywhere, everytime. " +
                       "They can be saved in save games.\n" +
                       "- Object: Object variables are bound to objects in the scene by an object ID. " +
                       "They can be saved in save games.", "")]
        public VariableOrigin origin = VariableOrigin.Global;

        // object variables
        [ORKEditorHelp("Use Object", "Use the 'Object Variables' component of game objects to change the object variables.\n" +
                       "The changes will be made on every 'Object Variables' component that is found. " +
                       "If no component is found, no variables will be changed.\n" +
                       "If disabled, you need to define the object ID used to change the object variables.", "")]
        [ORKEditorInfo(separator = true)]
        [ORKEditorLayout("origin", VariableOrigin.Object)]
        public bool useObject = true;

        [ORKEditorInfo(separator = true, labelText = "Object")]
        [ORKEditorLayout("useObject", true, autoInit = true)]
        public EventObjectSetting variableObject;

        [ORKEditorHelp("Object ID", "Define the object ID of the object variables.\n" +
                       "If the object ID doesn't exist yet, it will be created.", "")]
        [ORKEditorInfo(expandWidth = true)]
        [ORKEditorLayout(elseCheckGroup = true, endCheckGroup = true, endGroups = 2)]
        public string objectID = "";

        // variable key
        [ORKEditorHelp("Variable Key", "The key of the variable that will hold the return value.\n" +
                       "Typically you'll set the Value Type to Value and enter the name of the variable's key.", "")]
        [ORKEditorInfo(separator = true, labelText = "Variable Key")]
        public StringValue key = new StringValue();

        [ORKEditorHelp("Variable Type", "The return value type.\n" +
                       "Float and String are allowed. Others are not, and will be converted to a string.", "")]
        [ORKEditorInfo(separator = false, labelText = "Variable Type")]
        public GameVariableType variableType = GameVariableType.String;

        public GetQuestStateStep()
        {
        }

        public override void Execute(BaseEvent baseEvent)
        {
            QuestState questState;
            if (getQuestEntryState)
            {
                var entryNum = (int)questEntryNumber.GetValue(null, null);
                questState = QuestLog.GetQuestEntryState(questName.GetValue(), entryNum);
                if (DialogueDebug.LogInfo) Debug.Log("Dialogue System: ORK Step Get Quest State: '" + questName.GetValue() + "' entry #" + entryNum + " is " + questState);
            }
            else
            {
                questState = QuestLog.GetQuestState(questName.GetValue());
                if (DialogueDebug.LogInfo) Debug.Log("Dialogue System: ORK Step Get Quest State: '" + questName.GetValue() + "' is " + questState);
                QuestLog.SetQuestState(questName.GetValue(), questState);
            }
            if (variableType == GameVariableType.Float)
            {
                ORKEventTools.SetVariableValue(baseEvent, (float)((int)questState), origin, useObject, variableObject, objectID, key);
            }
            else
            {
                ORKEventTools.SetVariableValue(baseEvent, QuestLog.StateToString(questState), origin, useObject, variableObject, objectID, key);
            }
            baseEvent.StepFinished(this.next);
        }

        public override string GetNodeDetails()
        {
            if (getQuestEntryState)
            {
                var entryNum = (int)questEntryNumber.GetValue(null, null);
                return questName.value + " entry #" + entryNum;
            }
            else
            {
                return questName.value;
            }
        }

    }
}
