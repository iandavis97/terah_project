using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ORKFramework;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.ORKFrameworkSupport;

namespace ORKFramework.Events.Steps
{

	/// <summary>
	/// This ORK step runs Lua code in the Dialogue System's Lua environment.
	/// </summary>
	[ORKEditorHelp("Lua", "Runs Lua code.", "")]
	[ORKEventStep(typeof(BaseEvent))]
	[ORKNodeInfo("Dialogue System Steps")]
	public class LuaStep : BaseEventStep
	{

		[ORKEditorHelp("Lua Code", "The Lua code to run.", "")]
		[ORKEditorInfo(labelText="Lua Code", expandWidth=true)]
		public string luaCode = string.Empty;

		[ORKEditorHelp("Save Return Value", "Tick to save the return value of the Lua code.", "")]
		[ORKEditorInfo(labelText="Save Return Value")]
		public bool saveReturnValue = false;

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
		[ORKEditorInfo(separator=true)]
		[ORKEditorLayout("origin", VariableOrigin.Object)]
		public bool useObject = true;
		
		[ORKEditorInfo(separator=true, labelText="Object")]
		[ORKEditorLayout("useObject", true, autoInit=true)]
		public EventObjectSetting variableObject;
		
		[ORKEditorHelp("Object ID", "Define the object ID of the object variables.\n" +
		               "If the object ID doesn't exist yet, it will be created.", "")]
		[ORKEditorInfo(expandWidth=true)]
		[ORKEditorLayout(elseCheckGroup=true, endCheckGroup=true, endGroups=2)]
		public string objectID = "";

        // variable key
        [ORKEditorHelp("Variable Key", "The key of the variable that will hold the return value.\n" +
                       "Typically you'll set the Value Type to Value and enter the name of the variable's key.", "")]
        [ORKEditorInfo(separator=true, labelText="Variable Key")]
		public StringValue key = new StringValue();

        [ORKEditorHelp("Variable Type", "The return value type.\n" +
                       "Bool, Float, and String are allowed. Vector3 is not, and will be converted to a string.", "")]
        [ORKEditorInfo(separator = false, labelText = "Variable Type")]
        public GameVariableType variableType = GameVariableType.String;

		public LuaStep()
		{
		}
		
		public override void Execute(BaseEvent baseEvent)
		{
			var result = Lua.Run(luaCode, DialogueDebug.LogInfo);
			if (saveReturnValue) {
                if (DialogueDebug.LogInfo) Debug.Log("Dialogue System: Returning value '" + result.AsString + "' to ORK event.");
                switch (variableType)
                {
                    case GameVariableType.Bool:
                        ORKEventTools.SetVariableValue(baseEvent, result.AsBool, origin, useObject, variableObject, objectID, key);
                        break;
                    case GameVariableType.Float:
                        ORKEventTools.SetVariableValue(baseEvent, result.AsFloat, origin, useObject, variableObject, objectID, key);
                        break;
                    default:
                        ORKEventTools.SetVariableValue(baseEvent, result.AsString, origin, useObject, variableObject, objectID, key);
                        break;
                }
			}
			baseEvent.StepFinished(this.next);
		}
		
		public override string GetNodeDetails()
		{
			return luaCode;
		}
		
	}
}
