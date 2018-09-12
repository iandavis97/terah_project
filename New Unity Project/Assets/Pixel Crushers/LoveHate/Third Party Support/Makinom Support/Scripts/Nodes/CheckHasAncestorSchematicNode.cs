using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.LoveHate;
using Makinom;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

	[EditorHelp("Check Has Ancestor", "Which steps will be executed next is decided by whether a faction has another faction as an ancestor.", "")]
	[NodeInfo("Love\u2215Hate")]
	public class CheckHasAncestorStep : BaseSchematicCheckNode
	{

		// Faction
		[EditorHelp("Use Actor", "Check this actor's faction.", "")]
		[EditorInfo(titleLabel="Faction")]
		public bool useFactionActor = false;
		
		[EditorHelp("Actor", "Select the actor whose faction to check.", "")]
		[EditorInfo(isTabPopup=true, tabPopupID=0)]
		[EditorLayout("useFactionActor", true)]
		public SchematicObjectSelection actorObject = new SchematicObjectSelection();
		
		[EditorHelp("Faction", "Enter the name of the faction to check.", "")]
		[EditorLayout(elseCheckGroup=true, endCheckGroup=true)]
		public string factionName = string.Empty;
		
		
		// Parent
		[EditorHelp("Use Actor", "Check if this actor's faction as a direct parent.", "")]
		[EditorInfo(titleLabel="Parent Faction")]
		public bool useParentFactionActor = false;
		
		[EditorHelp("Actor", "Select the actor whose faction may be a direct parent.", "")]
		[EditorInfo(isTabPopup=true, tabPopupID=0)]
		[EditorLayout("useParentFactionActor", true)]
		public SchematicObjectSelection parentActorObject = new SchematicObjectSelection();
		
		[EditorHelp("Faction", "Enter the name of the direct parent faction.", "")]
		[EditorLayout(elseCheckGroup=true, endCheckGroup=true)]
		public string parentFactionName = string.Empty;


		public CheckHasAncestorStep()
		{
		}

		public override void Execute(Schematic schematic)
		{
			var factionManager = MakinomTools.GetFactionManager(useFactionActor, actorObject, useParentFactionActor, parentActorObject, schematic);
			var factionID = MakinomTools.GetFactionID(useFactionActor, actorObject, factionName, factionManager, schematic);
			var parentID = MakinomTools.GetFactionID(useParentFactionActor, parentActorObject, parentFactionName, factionManager, schematic);
			if (factionManager == null || factionManager.factionDatabase == null)
			{
				Debug.LogWarning("Love/Hate: Check Has Ancestor - Can't find faction manager");
			}
			if (factionID == -1)
			{
				Debug.LogWarning("Love/Hate: Check Has Ancestor - Can't find faction ID");
			}
			else if (parentID == -1)
			{
				Debug.LogWarning("Love/Hate: Check Has Ancestor - Can't find parent faction ID");
			}
			else
			{
				if (factionManager.factionDatabase.FactionHasAncestor(factionID, parentID))
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
			return useParentFactionActor ? parentActorObject.ToString() : parentFactionName;
		}

	}

}
