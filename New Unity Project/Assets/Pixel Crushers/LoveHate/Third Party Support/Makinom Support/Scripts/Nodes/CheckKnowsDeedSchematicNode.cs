using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.LoveHate;
using Makinom;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

	[EditorHelp("Check Knows Deed", "Which steps will be executed next is decided by whether a faction knows of a deed committed by another faction.", "")]
	[NodeInfo("Love\u2215Hate")]
	public class CheckKnowsDeedSchematicNode : BaseSchematicCheckNode
	{

		// Judge
		[EditorHelp("Judge", "Select the actor whose knowledge to check.", "")]
		[EditorInfo(titleLabel="Judge", isTabPopup=true, tabPopupID=0)]
		public SchematicObjectSelection judgeObject = new SchematicObjectSelection();


		// Actor
		[EditorHelp("Use Actor", "Check whether this actor's faction committed the deed.", "")]
		[EditorInfo(titleLabel="Actor")]
		public bool useActor = false;
		
		[EditorHelp("Actor", "Select the actor who may have committed the deed.", "")]
		[EditorInfo(isTabPopup=true, tabPopupID=0)]
		[EditorLayout("useActor", true)]
		public SchematicObjectSelection actorObject = new SchematicObjectSelection();
		
		[EditorHelp("Actor Faction", "Enter the name of the faction of the actor who may have committed the deed.", "")]
		[EditorLayout(elseCheckGroup=true, endCheckGroup=true)]
		public string actorFactionName = string.Empty;
		
		
		// Target
		[EditorHelp("Use Actor", "Check if the deed was done to this actor's faction.", "")]
		[EditorInfo(titleLabel="Target Faction")]
		public bool useTargetFactionActor = false;
		
		[EditorHelp("Target", "Select the target actor.", "")]
		[EditorInfo(isTabPopup=true, tabPopupID=0)]
		[EditorLayout("useTargetFactionActor", true)]
		public SchematicObjectSelection targetObject = new SchematicObjectSelection();
		
		[EditorHelp("Target Faction", "Enter the name of the target faction.", "")]
		[EditorLayout(elseCheckGroup=true, endCheckGroup=true)]
		public string targetFactionName = string.Empty;


		// Trait
		[EditorHelp("Deed Tag", "Enter the deed tag to check.", "")]
		[EditorInfo(titleLabel="Deed Tag", expandWidth=true)]
		public string deedTag = string.Empty;
		
		
		public CheckKnowsDeedSchematicNode()
		{
		}

		public override void Execute(Schematic schematic)
		{
			var judge = MakinomTools.GetObjectSelectionComponentInChildren<FactionMember>(judgeObject, schematic);
			var factionManager = (judge == null) ? null : judge.factionManager;
			var actorID = MakinomTools.GetFactionID(useActor, actorObject, actorFactionName, factionManager, schematic);
			var targetID = MakinomTools.GetFactionID(useTargetFactionActor, targetObject, targetFactionName, factionManager, schematic);
			if (factionManager == null || factionManager.factionDatabase == null)
			{
				Debug.LogWarning("Love/Hate: Check Knows Deed - Can't find faction manager");
			}
			else if (judge == null)
			{
				Debug.LogWarning("Love/Hate: Check Knows Deed - Can't find judge faction member");
			}
			if (actorID == -1)
			{
				Debug.LogWarning("Love/Hate: Check Knows Deed - Can't find actor faction ID");
			}
			else if (targetID == -1)
			{
				Debug.LogWarning("Love/Hate: Check Knows Deed - Can't find target faction ID");
			}
			else
			{
				if (judge.KnowsAboutDeed(actorID, targetID, deedTag))
				{
					schematic.NodeFinished(this.next);
				}
				else
				{
					schematic.NodeFinished(this.nextFail);
				}
			}
			schematic.NodeFinished(this.next);
		}		
		
		public override string GetNodeDetails()
		{
			return (useActor ? actorObject.ToString() : actorFactionName) + " " +
				deedTag + " " + 
				(useTargetFactionActor ? targetObject.ToString() : targetFactionName);
		}

	}

}
