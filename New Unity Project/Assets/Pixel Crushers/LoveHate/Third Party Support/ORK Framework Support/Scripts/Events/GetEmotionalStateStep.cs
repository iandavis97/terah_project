using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ORKFramework;
using ORKFramework.Behaviours;
using PixelCrushers.LoveHate;
using PixelCrushers.LoveHate.ORKFrameworkSupport;

namespace ORKFramework.Events.Steps
{

    [ORKEditorHelp("Get Emotional State", "Stores a faction member's emotional state in a string variable. If the faction member doesn't have an Emotional State component, it stores the faction member's temperament.", "")]
    [ORKEventStep(typeof(BaseEvent))]
    [ORKNodeInfo("Love\u2215Hate Steps")]
    public class GetEmotionalStateStep : BaseEventStep
    {

        // Actor
        [ORKEditorHelp("Actor", "Select the actor to check.", "")]
        [ORKEditorInfo(isTabPopup = true, tabPopupID = 0)]
        public EventObjectSetting actorObject = new EventObjectSetting();


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
        [ORKEditorInfo(separator = true, labelText = "Variable Key")]
        public StringValue key = new StringValue();

        // variable key
        [ORKEditorInfo(separator = true, labelText = "Debug")]
        public bool debug;

        public GetEmotionalStateStep()
        {
        }

        public override void Execute(BaseEvent baseEvent)
        {
            var factionMember = OrkEventTools.GetEventObjectComponentInChildren<FactionMember>(actorObject, baseEvent);
            if (factionMember == null)
            {
                Debug.LogWarning("Love/Hate: GetEmotionalState - Can't find faction member on " + actorObject.GetInfoText());
            }
            else
            {
                var emotionalState = factionMember.GetComponent<EmotionalState>();
                var value = (emotionalState != null) ? emotionalState.GetCurrentEmotionName() : factionMember.pad.GetTemperament().ToString();
                if (debug)
                {
                    var variableInfo = origin.ToString();
                    if (origin == VariableOrigin.Global || origin == VariableOrigin.Local)
                    {
                        variableInfo += " " + key.GetValue();
                    }
                    Debug.Log("Love/Hate: GetEmotionalState - setting variable " + variableInfo + " to " + value + " (Emotional state on " + factionMember + ")");
                }
                OrkEventTools.SetVariableValue(baseEvent, value, origin, useObject, variableObject, objectID, key);
            }
            baseEvent.StepFinished(next);
        }

        public override string GetNodeDetails()
        {
            return actorObject.GetInfoText();
        }

    }

}
