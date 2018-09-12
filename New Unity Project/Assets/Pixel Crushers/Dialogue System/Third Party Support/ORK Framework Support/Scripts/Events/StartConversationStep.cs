using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ORKFramework;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.ORKFrameworkSupport;

namespace ORKFramework.Events.Steps
{

	/// <summary>
	/// This ORK step starts a Dialogue System conversation.
	/// </summary>
	[ORKEditorHelp("Start Conversation", "Starts a Dialogue System conversation.", "")]
	[ORKEventStep(typeof(BaseEvent))]
	[ORKNodeInfo("Dialogue System Steps")]
	public class StartConversationStep : BaseEventStep
	{
		public ConversationData conversationData = new ConversationData();
		
		public StartConversationStep()
		{
		}	    
		
		public override void Execute(BaseEvent baseEvent)
		{
		    var conversationTitle = conversationData.GetConversationTitle(baseEvent);
            //Debug.Log("[StartConversationStep] Starting conversation: " + conversationTitle);
			var actor = ORKEventTools.GetEventObjectTransform(conversationData.actorObject, baseEvent);
			var conversant = ORKEventTools.GetEventObjectTransform(conversationData.conversantObject, baseEvent);
			DialogueManager.StartConversation(conversationTitle, actor, conversant);
			DialogueManager.Instance.StartCoroutine(WaitForConversationEnd(baseEvent));
		}

		private IEnumerator WaitForConversationEnd(BaseEvent baseEvent) 
		{
			while (DialogueManager.IsConversationActive) 
			{
				yield return null;
			}
			baseEvent.StepFinished(this.next);
		}

        public override string GetNodeDetails()
        {
            switch (conversationData.conversation.type)
            {
                case StringValueType.GameVariable:                    
                    return conversationData.origin == VariableOrigin.Object
                        ? string.Format("Object Variable [{0}]", conversationData.useObject ? conversationData.variableObject.type.ToString() : "<object ID>")
                        : string.Format("{0} Variable", conversationData.origin);
                case StringValueType.SceneName:
                    return "Use Scene Name";
                case StringValueType.PlayerPrefs:
                    return string.Format("Use PlayerPrefs[{0}]", conversationData.conversation.value);
                default:
                    return conversationData.conversation.value;
            }            
        }
    }
}
