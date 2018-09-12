using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.LoveHate;
using Makinom;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

	[EditorHelp("Report Deed", "Reports a deed to Love/Hate.", "")]
	[NodeInfo("Love\u2215Hate")]
	public class ReportDeedStep : BaseSchematicNode
	{

		[EditorHelp("Actor", "Select the actor committing the deed.", "")]
		[EditorInfo(titleLabel="Actor")]
		public SchematicObjectSelection actorObject = new SchematicObjectSelection();
		
		[EditorHelp("Target", "Select the actor that the deed is being done to.", "")]
		[EditorInfo(titleLabel="Target")]
		public SchematicObjectSelection targetObject = new SchematicObjectSelection();

		[EditorHelp("Deed Tag", "Specify the tag of the deed template as specified on the actor's deed reporter.", "")]
		[EditorInfo(titleLabel="Deed Tag", expandWidth=true)]
		public string deedTag = string.Empty;
		

		public ReportDeedStep()
		{
		}

		public override void Execute(Schematic schematic)
		{
			var deedReporter = MakinomTools.GetObjectSelectionComponentInChildren<DeedReporter>(actorObject, schematic);
			var target = MakinomTools.GetObjectSelectionComponentInChildren<FactionMember>(targetObject, schematic);
			if (deedReporter == null)
			{
				Debug.LogWarning("Love/Hate: Report Deed - Can't find deed reporter");
			}
			else if (target == null)
			{
				Debug.LogWarning("Love/Hate: Report Deed - Can't find target faction member");
			}
			else
			{
				deedReporter.ReportDeed(deedTag, target);
			}
			schematic.NodeFinished(this.next);
		}		
		
		public override string GetNodeDetails()
		{
			return deedTag;
		}

	}

}
