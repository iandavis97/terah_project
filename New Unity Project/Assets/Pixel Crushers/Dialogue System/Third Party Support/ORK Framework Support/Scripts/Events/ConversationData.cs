using UnityEngine;
using ORKFramework.Events;
using PixelCrushers.DialogueSystem.ORKFrameworkSupport;
using UnityEngine.SceneManagement;

namespace ORKFramework
{

	/// <summary>
	/// Conversation data for the Dialogue System's StartConversationStep.
	/// </summary>
	public class ConversationData : BaseData
	{        
        [ORKEditorHelp("Variable Origin", "Select the origin of the variable:\n" +
            "- Local: Local variables are only used in a running event and don't interfere with global variables. " +
            "The variable will be gone once the event ends.\n" +
            "- Global: Global variables are persistent and available everywhere, everytime. " +
            "They can be saved in save games.\n" +
            "- Object: Object variables are bound to objects in the scene by an object ID. " +
            "They can be saved in save games.\n" +
            "- Selected: Variables assigned to selected data of the event.\n" +
            "In a battle event, the variables are usually assigned to the ability or item of the action.", "")]
        [ORKEditorInfo(separator = true, labelText = "Variable Settings")]
        public VariableOrigin origin = VariableOrigin.Global;

        [ORKEditorInfo(labelText = "Selected Key")]
        [ORKEditorLayout("origin", VariableOrigin.Selected, endCheckGroup = true, autoInit = true)]
        public StringValue selectedKey;

        // object variables
        [ORKEditorHelp("Use Object", "Use the 'Object Variables' component of game objects to get the object variables.\n" +            
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
        
        [ORKEditorHelp("Conversation Title", "The title of the conversation in the dialogue database.\n" +
            "If the ValueType is set to 'Game Variable' then this is the variable key.", "")]
		[ORKEditorInfo(separator = true, labelText = "Conversation Title", expandWidth=true)]
		public StringValue conversation =new StringValue();
		
		[ORKEditorInfo(labelText="Actor")]
		public EventObjectSetting actorObject = new EventObjectSetting();

		[ORKEditorInfo(labelText="Conversant")]
		public EventObjectSetting conversantObject = new EventObjectSetting();

		public ConversationData()
		{
		}

	    public string GetConversationTitle(BaseEvent baseEvent)
	    {
	        switch (conversation.type)
	        {
	            case StringValueType.Value:
	                return conversation.value;
                case StringValueType.GameVariable:
	                return GetObjectVariableString(baseEvent);
                case StringValueType.SceneName:
                    return SceneManager.GetActiveScene().name;
                case StringValueType.PlayerPrefs:
	                return PlayerPrefs.GetString(conversation.value);
                default:                    
	                return string.Empty;
	        }
	    }

	    private string GetObjectVariableString(BaseEvent baseEvent)
	    {
	        switch (origin)
	        {
	            case VariableOrigin.Global:
	                return ORKEventTools.GetStringValue(conversation.value);
                case VariableOrigin.Local:
	                return ORKEventTools.GetStringValue(conversation.value, baseEvent);
                case VariableOrigin.Object:
	                return useObject
	                    ? ORKEventTools.GetStringValue(conversation.value, baseEvent, variableObject)
	                    : ORKEventTools.GetStringValue(conversation.value, objectID);
                case VariableOrigin.Selected:
	                return ORKEventTools.GetStringValueFromSelected(conversation.value, baseEvent, selectedKey.GetValue());	                
                default:
	                return string.Empty;
	        }
	    }

	}
}
