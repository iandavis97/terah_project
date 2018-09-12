using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ORKFramework;
using ORKFramework.Behaviours;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.ORKFrameworkSupport;

namespace ORKFramework.Events.Steps
{
    // Based on contribution by forum user hellwalker.

    /// <summary>
    /// This ORK step sets a Dialogue System Lua variable.
    /// </summary>
    [ORKEditorHelp("Set DS Variable", "Sets a Dialogue System Lua variable.", "")]
    [ORKEventStep(typeof(BaseEvent))]
    [ORKNodeInfo("Dialogue System Steps")]
    public class SetDSVariableStep : BaseEventStep
    {

        [ORKEditorHelp("Name of DS Variable", "A variable defined in your dialogue database.", "")]
        [ORKEditorInfo(labelText = "DS Variable", expandWidth = true)]
        public string DSVariableName = string.Empty;

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
        [ORKEditorHelp("Variable Key", "The key of the variable whose value DS var will be set to.\n" +
            "Typically you'll set the Value Type to Value and enter the name of the variable's key.", "")]
        [ORKEditorInfo(separator = true, labelText = "Variable Key")]
        public StringValue key = new StringValue();

        [ORKEditorHelp("Variable Type", "What type of variable are you setting.\n" +
            "Bool, Float, and String are allowed. Vector3 is not, and will be converted to a string.", "")]
        [ORKEditorInfo(separator = false, labelText = "Variable Type")]
        public GameVariableType variableType = GameVariableType.String;

        public SetDSVariableStep()
        {
        }

        //Get Variable based on origin and variable type
        private object GetVar(VariableHandler handler, GameVariableType variableType)
        {
            switch (variableType)
            {
                case GameVariableType.Bool:
                    return handler.GetBool(key.GetValue());
                case GameVariableType.Float:
                    return handler.GetFloat(key.GetValue());
                default:
                    return handler.GetString(key.GetValue());
            }
        }


        public override void Execute(BaseEvent baseEvent)
        {
            object value = "unassigned";

            if (VariableOrigin.Local.Equals(origin))
            {

                value = GetVar(baseEvent.Variables, variableType);
            }
            else if (VariableOrigin.Global.Equals(origin))
            {
                value = GetVar(ORK.Game.Variables, variableType);

            }
            else if (VariableOrigin.Object.Equals(origin))
            {
                if (useObject)
                {
                    List<GameObject> list2 = variableObject.GetObject(baseEvent);
                    for (int j = 0; j < list2.Count; j++)
                    {
                        if (list2[j] != null)
                        {
                            ObjectVariablesComponent[] comps = list2[j].
                                GetComponentsInChildren<ObjectVariablesComponent>();
                            for (int k = 0; k < comps.Length; k++)
                            {
                                value = GetVar(comps[k].GetHandler(), variableType);

                            }
                        }
                    }
                }
                else
                {
                    value = GetVar(ORK.Game.Scene.GetObjectVariables(objectID), variableType);
                }
            }

            if (DialogueDebug.LogInfo) Debug.Log("Dialogue System: ORK Step Set DS Variable " + DSVariableName + " to '" + value + "'");
            DialogueLua.SetVariable(DSVariableName, value);
            baseEvent.StepFinished(this.next);
        }

        public override string GetNodeDetails()
        {
            return DSVariableName;
        }

    }
}