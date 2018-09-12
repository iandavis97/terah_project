using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace Makinom.Schematics.Nodes
{
	/// <summary>
	/// Checks if a conversation is active right now.
	/// </summary>
	[EditorHelp("Check Conversation Active", "Checks if a conversation is active right now.", "")]
	[NodeInfo("Dialogue System")]
	public class CheckConversationActiveNode : BaseSchematicCheckNode
	{
		public CheckConversationActiveNode()
		{
			
		}
		
		public override void Execute(Schematic schematic)
		{
			if (DialogueManager.IsConversationActive)
			{
				schematic.NodeFinished(this.next);
			}
			else
			{
				schematic.NodeFinished(this.nextFail);
			}
		}		
		
	}
}
