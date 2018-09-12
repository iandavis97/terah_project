using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Makinom;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

	/// <summary>
	/// This node starts a Dialogue System bark.
	/// </summary>
	[EditorHelp("Bark", "Makes a character bark.", "")]
	[NodeInfo("Dialogue System")]
	public class BarkNode : BaseSchematicNode
	{

		[EditorHelp("Bark Conversation Title", "The title of the conversation in the dialogue database containing the bark lines.", "")]
		public string conversation = string.Empty;
		
		[EditorInfo(separator = true, titleLabel = "Barker")]
		[EditorHelp("Barker", "The actor that barks", "")]
		public SchematicObjectSelection actorObject = new SchematicObjectSelection();
		
		[EditorInfo(separator = true, titleLabel = "Target")]
		[EditorHelp("Target", "(Optional) The target to whom the bark is directed", "")]
		public SchematicObjectSelection conversantObject = new SchematicObjectSelection();

		public BarkNode()
		{
		}
		
		public override void Execute(Schematic schematic)
		{
			var actor = MakinomNodeTools.GetObjectSelectionTransform(actorObject, schematic);
			var conversant = MakinomNodeTools.GetObjectSelectionTransform(conversantObject, schematic);
			DialogueManager.Bark(conversation, actor, conversant);
			schematic.NodeFinished(this.next);
		}

		public override string GetNodeDetails()
		{
			return conversation;
		}

	}
}
