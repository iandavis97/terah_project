using UnityEngine;
using System.Collections.Generic;
using ORKFramework;
using ORKFramework.Behaviours;
using ORKFramework.Events;

namespace PixelCrushers.DialogueSystem.ORKFrameworkSupport {

	public static class ORKEventTools {

        public static bool debug = false;

		public static Transform GetEventObjectTransform(EventObjectSetting eventObject, BaseEvent baseEvent) {
			var list = eventObject.GetObject(baseEvent);
			var hasItem = (list != null) && (list.Count > 0);
			return hasItem ? list[0].transform : null;
		}
		public static void SetVariableValue(BaseEvent baseEvent, System.Object value, 
		                                    VariableOrigin origin, bool useObject, 
		                                    EventObjectSetting variableObject, string objectID,
		                                    StringValue key)
		{
			if (value is string || value is bool || value is int || value is float || value is Vector3)
			{
				if (VariableOrigin.Local.Equals(origin))
				{
                    if (debug) Debug.Log("Setting local variable " + key + " to " + value);
                    SetVariableValue(baseEvent.Variables, value, key);
				}
				else if (VariableOrigin.Global.Equals(origin))
				{
                    if (debug) Debug.Log("Setting global variable " + key + " to " + value);
					SetVariableValue(ORK.Game.Variables, value, key);
				}
				else if(VariableOrigin.Object.Equals(origin))
				{
					if (useObject)
					{
						List<GameObject> list2 = variableObject.GetObject(baseEvent);
						for (int j=0; j < list2.Count; j++)
						{
							if (list2[j] != null)
							{
								ObjectVariablesComponent[] comps = list2[j].
									GetComponentsInChildren<ObjectVariablesComponent>();
								for (int k=0; k < comps.Length; k++)
								{
                                    if (debug) Debug.Log("Setting object variable " + key + " to " + value);
                                    SetVariableValue(comps[k].GetHandler(), value, key);
								}
							}
						}
					}
					else
					{
                        if (debug) Debug.Log("Setting scene object variable " + key + " to " + value);
                        SetVariableValue(ORK.Game.Scene.GetObjectVariables(objectID), value, key);
					}
				}
			}
		}
		
		public static void SetVariableValue(VariableHandler handler, System.Object value, StringValue key)
		{
			if (value is string)
			{
                if (debug) Debug.Log("Setting string key=" + key.GetValue() + " to '" + value + "'.");
				handler.Set(key.GetValue(), (string) value);
			}
			else if (value is bool)
			{
				handler.Set(key.GetValue(), (bool) value);
			}
			else if (value is int)
			{
				handler.Set(key.GetValue(), (int) value);
			}
			else if (value is float)
			{
				handler.Set(key.GetValue(), (float) value);
			}
			else if (value is Vector3)
			{
				handler.Set(key.GetValue(), (Vector3) value);
			}
		}
	   
        /// <summary>
        /// Get the value of a Global string variable
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
	    public static string GetStringValue(string key)
	    {
            return ORK.Game.Variables.GetString(key);
        }

        /// <summary>
        /// Get the value of a Local (event scope) string variable
        /// </summary>
        /// <param name="baseEvent"></param>
        /// <param name="key"></param>
        /// <returns></returns>
	    public static string GetStringValue(string key, BaseEvent baseEvent)
	    {
            return baseEvent.Variables.GetString(key);
        }

        /// <summary>
        /// Get the value of a string variable from the event's Selected Data
        /// </summary>
        /// <param name="baseEvent"></param>
        /// <param name="key"></param>
        /// <param name="selectedKey"></param>
        /// <returns></returns>
	    public static string GetStringValueFromSelected(string key, BaseEvent baseEvent, string selectedKey)
	    {
	        var handler = SelectedDataHelper.GetFirstVariableHandler(baseEvent.SelectedData.Get(selectedKey));
	        return (handler == null)
	            ? string.Empty
	            : handler.GetString(key);
	    }

        /// <summary>
        /// Get the value of an Object string variable, using the object's GUID
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objectID"></param>
        /// <returns></returns>
	    public static string GetStringValue(string key, string objectID)
	    {
            var objectVariables = ORK.Game.Scene.GetObjectVariables(objectID);

            return objectVariables == null 
                ? string.Empty 
                : objectVariables.GetString(key);
	    }

        /// <summary>
        /// Get the value of an Object string variable, using the event object
        /// </summary>
        /// <param name="baseEvent"></param>
        /// <param name="key"></param>
        /// <param name="variableObject"></param>
        /// <returns></returns>
        public static string GetStringValue(string key, BaseEvent baseEvent, EventObjectSetting variableObject)
	    {
	        string result = string.Empty;
           
            var objects = variableObject.GetObject(baseEvent);

            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] != null)
                {
                    // Get all Object Variables components on the game object
                    var objectVariables = objects[i].GetComponentsInChildren<ObjectVariablesComponent>();
                    if (objectVariables != null)
                    {
                        // Check for the variable on each component; as soon as we receive a non-empty string value
                        // for the provided variable key, we will return that value.
                        for (int j = 0; j < objectVariables.Length; j++)
                        {
                            var handler = objectVariables[j].GetHandler();
                            if (handler != null)
                            {
                                result = handler.GetString(key);
                                if (!string.IsNullOrEmpty(result))
                                {
                                    if (debug) Debug.Log("Found object variable " + key + " with value = " + result);
                                    return result;
                                }
                            }
                        }
                    }
                }
            }           

            // fallback: return string.Empty
	        return result;
	    }
	}
}
