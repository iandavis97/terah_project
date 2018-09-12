using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Makinom;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

	/// <summary>
	/// This node plays a Dialogue System sequence.
	/// </summary>
	[EditorHelp("Play Sequence", "Plays a Dialogue System sequence. Doesn't wait for it to end.", "")]
	[NodeInfo("Dialogue System")]
	public class PlaySequenceNode : BaseSchematicNode
	{

		[EditorHelp("Sequence", "The sequence to play.", "")]
		public string sequence = string.Empty;
		
		[EditorInfo(separator = true, titleLabel = "Speaker")]
		[EditorHelp("Speaker", "(Optional) The speaker in the sequence", "")]
		public SchematicObjectSelection actorObject = new SchematicObjectSelection();
		
		[EditorInfo(separator = true, titleLabel = "Listener")]
		[EditorHelp("Listener", "(Optional) The listener in the sequence", "")]
		public SchematicObjectSelection conversantObject = new SchematicObjectSelection();
		
		public PlaySequenceNode()
		{
		}
		
		public override void Execute(Schematic schematic)
		{
			var actor = MakinomNodeTools.GetObjectSelectionTransform(actorObject, schematic);
			var conversant = MakinomNodeTools.GetObjectSelectionTransform(conversantObject, schematic);
			DialogueManager.PlaySequence(sequence, actor, conversant);
			schematic.NodeFinished(this.next);
		}

		public override string GetNodeDetails()
		{
			return sequence;
		}

	}
}
