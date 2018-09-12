using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Makinom;
using PixelCrushers.LoveHate;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

	[EditorHelp("Share Rumors", "Shares rumors between two faction members.", "")]
	[NodeInfo("Love\u2215Hate")]
	public class ShareRumorsStep : BaseSchematicNode
	{

		// First Actor
		[EditorHelp("First Actor", "Select the first actor.", "")]
		[EditorInfo(titleLabel="First Actor", isTabPopup=true, tabPopupID=0)]
		public SchematicObjectSelection firstObject = new SchematicObjectSelection();

		// Second Actor
		[EditorHelp("Second Actor", "Select the second actor.", "")]
		[EditorInfo(titleLabel="Second Actor", isTabPopup=true, tabPopupID=0)]
		public SchematicObjectSelection secondObject = new SchematicObjectSelection();
		

		public ShareRumorsStep()
		{
		}
		
		public override void Execute(Schematic schematic)
		{
			var firstMember = MakinomTools.GetObjectSelectionComponentInChildren<FactionMember>(firstObject, schematic);
			var secondMember = MakinomTools.GetObjectSelectionComponentInChildren<FactionMember>(secondObject, schematic);
			if (firstMember == null)
			{
				Debug.LogWarning("Love/Hate: Share Rumors - Can't find first faction member");
			}
			if (secondMember == null)
			{
				Debug.LogWarning("Love/Hate: Share Rumors - Can't find second faction member");
			}
			else
			{
				firstMember.ShareRumors(secondMember);
			}
			schematic.NodeFinished(this.next);
		}		
		
		public override string GetNodeDetails()
		{
            return firstObject.ToString() + " <-> " + secondObject.ToString();
		}

	}

}
