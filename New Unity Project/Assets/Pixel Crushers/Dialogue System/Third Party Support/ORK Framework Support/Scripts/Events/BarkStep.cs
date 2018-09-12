using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.ORKFrameworkSupport;

namespace ORKFramework.Events.Steps
{

	/// <summary>
	/// This ORK step starts a Dialogue System bark.
	/// </summary>
	[ORKEditorHelp("Bark", "Makes a character bark.", "")]
	[ORKEventStep(typeof(BaseEvent))]
	[ORKNodeInfo("Dialogue System Steps")]
	public class BarkStep : BaseEventStep
	{
		public ConversationData barkConversationData = new ConversationData();
		
		public BarkStep()
		{
		}
		
		public override void Execute(BaseEvent baseEvent)
		{
			var conversationTitle = barkConversationData.GetConversationTitle(baseEvent);
			var actor = ORKEventTools.GetEventObjectTransform(barkConversationData.actorObject, baseEvent);
			var conversant = ORKEventTools.GetEventObjectTransform(barkConversationData.conversantObject, baseEvent);
			DialogueManager.Bark(conversationTitle, actor, conversant);
			baseEvent.StepFinished(this.next);
		}

		public override string GetNodeDetails()
		{
            switch (barkConversationData.conversation.type)
            {
                case StringValueType.GameVariable:
                    return barkConversationData.origin == VariableOrigin.Object
                        ? string.Format("Object Variable [{0}]", barkConversationData.useObject ? barkConversationData.variableObject.type.ToString() : "<object ID>")
                        : string.Format("{0} Variable", barkConversationData.origin);
                case StringValueType.SceneName:
                    return "Use Scene Name";
                case StringValueType.PlayerPrefs:
                    return string.Format("Use PlayerPrefs[{0}]", barkConversationData.conversation.value);
                default:
                    return barkConversationData.conversation.value;
            }
        }

	}
}
