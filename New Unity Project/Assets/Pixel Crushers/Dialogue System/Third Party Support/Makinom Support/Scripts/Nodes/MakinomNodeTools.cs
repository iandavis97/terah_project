using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Makinom;
using Makinom.Schematics;

namespace PixelCrushers.DialogueSystem.MakinomSupport
{
	
	public static class MakinomNodeTools
	{
		
		public static Transform GetObjectSelectionTransform(SchematicObjectSelection objectSelection, Schematic schematic)
		{
			var go = objectSelection.GetObject(schematic);
			return (go == null) ? null : go.transform;
		}
		
		public static T GetObjectSelectionComponentInChildren<T>(SchematicObjectSelection objectSelection, Schematic schematic) where T : Component
		{
			var transform = GetObjectSelectionTransform(objectSelection, schematic);
			return (transform == null) ? null : transform.GetComponentInChildren<T>();
		}
		
		public static void SetVariableValue(Schematic schematic, Lua.Result value, 
		                                    VariableOrigin origin, bool useObject, 
		                                    SchematicObjectSelection variableObject, string objectID,
		                                    StringValue key, VariableType type)
		{
			switch (origin) {
			case VariableOrigin.Global:
				SetVariableValue(Maki.Game.Variables, value, key, type);
				break;
			case VariableOrigin.Local:
				SetVariableValue(schematic.Variables, value, key, type);
				break;
			case VariableOrigin.Object:
				if (useObject)
				{
					SetVariableValue(schematic.Variables, value, key, type);
				}
				else
				{
					SetVariableValue(Maki.Game.GetObjectVariables(objectID), value, key, type);
				}
				break;
			case VariableOrigin.ObjectID:
				SetVariableValue(Maki.Game.GetObjectVariables(objectID), value, key, type);
				break;
			default:
				Debug.LogWarning("Dialogue System: Can't handle variable origin " + origin + " to set variable");
				break;
			}
		}
		
		public static void SetVariableValue(VariableHandler handler, Lua.Result value, StringValue key, VariableType type)
		{
			var keyName = key.GetValue(null);
			switch (type)
			{
			case VariableType.Bool:
				handler.Set(keyName, value.AsBool);
				break;
			case VariableType.Float:
				handler.Set(keyName, value.AsFloat);
				break;
			case VariableType.Int:
				handler.Set(keyName, value.AsInt);
				break;
			case VariableType.String:
				handler.Set(keyName, value.AsString);
				break;
			default:
				Debug.LogWarning("Dialogue System: Can't handle type " + type + " to set variable " + keyName + ". Setting as string");
                handler.Set(keyName, value.AsString);
                break;
			}
		}
		
	}
}
