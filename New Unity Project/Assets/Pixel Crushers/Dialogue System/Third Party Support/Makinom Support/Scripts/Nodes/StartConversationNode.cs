using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Makinom;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

	/// <summary>
	/// This node starts a Dialogue System conversation.
	/// </summary>
	[EditorHelp("Start Conversation", "Starts a Dialogue System conversation. Doesn't wait for it to end.", "")]
	[NodeInfo("Dialogue System")]
	public class StartConversationNode : BaseSchematicNode
	{

		public ConversationData conversationData = new ConversationData();
		
		public StartConversationNode()
		{
		}
		
		public override void Execute(Schematic schematic)
		{
			var conversationTitle = conversationData.conversation;
			var actor = MakinomNodeTools.GetObjectSelectionTransform(conversationData.actorObject, schematic);
			var conversant = MakinomNodeTools.GetObjectSelectionTransform(conversationData.conversantObject, schematic);
			DialogueManager.StartConversation(conversationTitle, actor, conversant);
			schematic.NodeFinished(this.next);
		}

		public override string GetNodeDetails()
		{
			return conversationData.conversation;
		}

	}
}
