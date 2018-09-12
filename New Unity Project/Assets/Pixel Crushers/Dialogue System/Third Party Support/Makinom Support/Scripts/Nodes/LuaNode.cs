using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Makinom;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.MakinomSupport;

namespace Makinom.Schematics.Nodes
{
	
	/// <summary>
	/// This node runs Lua code in the Dialogue System's Lua environment.
	/// </summary>
	[EditorHelp("Lua", "Runs Lua code.", "")]
	[NodeInfo("Dialogue System")]
	public class LuaNode : BaseSchematicNode
	{

		[EditorHelp("Lua Code", "The Lua code to run.", "")]
		[EditorInfo(titleLabel="Lua Code", expandWidth=true)]
		public string luaCode = string.Empty;

		[EditorHelp("Save Return Value", "Tick to save the return value of the Lua code.", "")]
		[EditorInfo(titleLabel="Save Return Value")]
		public bool saveReturnValue = false;

		// Result variable
		[EditorHelp("Variable Origin", "Select the origin of the variables:\n" +
		            "- Local: Local variables are only used in a running event and don't interfere with global variables. " +
		            "The variable will be gone once the event ends.\n" +
		            "- Global: Global variables are persistent and available everywhere, everytime. " +
		            "They can be saved in save games.\n" +
		            "- Object: Object variables are bound to objects in the scene by an object ID. " +
		            "They can be saved in save games.", "")]
		public VariableOrigin origin = VariableOrigin.Global;

		// object variables
		[EditorHelp("Use Object", "Use the 'Object Variables' component of game objects to change the object variables.\n" +
		            "The changes will be made on every 'Object Variables' component that is found. " +
		            "If no component is found, no variables will be changed.\n" +
		            "If disabled, you need to define the object ID used to change the object variables.", "")]
		[EditorInfo(separator=true)]
		[EditorLayout("origin", VariableOrigin.Object)]
		public bool useObject = true;
		
		[EditorInfo(separator=true, titleLabel="Object")]
		[EditorLayout("useObject", true, autoInit=true)]
		public SchematicObjectSelection variableObject;
		
		[EditorHelp("Object ID", "Define the object ID of the object variables.\n" +
		            "If the object ID doesn't exist yet, it will be created.", "")]
		[EditorInfo(expandWidth=true)]
		[EditorLayout(elseCheckGroup=true, endCheckGroup=true, endGroups=2)]
		public string objectID = "";

		// variable key
		[EditorInfo(separator=true, titleLabel="Variable Key")]
		public StringValue key = new StringValue();

		// variable type
		[EditorInfo(separator=true, titleLabel="Variable Type")]
		public VariableType type;


		public LuaNode()
		{
		}
		
		public override void Execute(Schematic schematic)
		{
			var result = Lua.Run(luaCode, DialogueDebug.LogInfo);
			if (saveReturnValue) {
				MakinomNodeTools.SetVariableValue(schematic, result, origin, useObject, variableObject, objectID, key, type);
			}
			schematic.NodeFinished(this.next);
		}
		
		public override string GetNodeDetails()
		{
			return luaCode;
		}
		
	}
}
