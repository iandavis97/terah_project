using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.LoveHate;
using Makinom;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

	[EditorHelp("Remove Direct Parent", "Removes a direct parent from a Love/Hate faction.", "")]
	[NodeInfo("Love\u2215Hate")]
	public class RemoveDirectParentStep : BaseSchematicNode
	{

		// Faction
		[EditorHelp("Use Actor", "Remove a direct parent from this actor's (combatant) faction.", "")]
		[EditorInfo(titleLabel="Faction")]
		public bool useFactionActor = false;

		[EditorHelp("Actor", "Select the actor whose faction will lose a direct parent.", "")]
		[EditorInfo(isTabPopup=true, tabPopupID=0)]
		[EditorLayout("useFactionActor", true)]
		public SchematicObjectSelection actorObject = new SchematicObjectSelection();
		
		[EditorHelp("Faction", "Enter the name of the faction that will be changed.", "")]
		[EditorLayout(elseCheckGroup=true, endCheckGroup=true)]
		public string factionName = string.Empty;


		// Parent
		[EditorHelp("Use Actor", "Remove this actor's faction as the direct parent.", "")]
		[EditorInfo(titleLabel="Parent Faction")]
		public bool useParentFactionActor = false;
		
		[EditorHelp("Actor", "Select the actor whose faction will no longer be a direct parent.", "")]
		[EditorInfo(isTabPopup=true, tabPopupID=0)]
		[EditorLayout("useParentFactionActor", true)]
		public SchematicObjectSelection parentActorObject = new SchematicObjectSelection();
		
		[EditorHelp("Faction", "Enter the name of the direct parent faction.", "")]
		[EditorLayout(elseCheckGroup=true, endCheckGroup=true)]
		public string parentFactionName = string.Empty;


		[EditorHelp("Inherit Relationships", "If ticked, the faction inherits its parent's relationships before the parent is removed.", "")]
		public bool inheritRelationships = false;


		public RemoveDirectParentStep()
		{
		}

		public override void Execute(Schematic schematic)
		{
			var factionManager = MakinomTools.GetFactionManager(useFactionActor, actorObject, useParentFactionActor, parentActorObject, schematic);
			var factionID = MakinomTools.GetFactionID(useFactionActor, actorObject, factionName, factionManager, schematic);
			var parentID = MakinomTools.GetFactionID(useParentFactionActor, parentActorObject, parentFactionName, factionManager, schematic);
			if (factionManager == null)
			{
				Debug.LogWarning("Love/Hate: Remove Direct Parent - Can't find faction manager");
			}
			if (factionID == -1)
			{
				Debug.LogWarning("Love/Hate: Remove Direct Parent - Can't find faction ID");
			}
			else if (parentID == -1)
			{
				Debug.LogWarning("Love/Hate: Remove Direct Parent - Can't find parent faction ID");
			}
			else
			{
				factionManager.RemoveFactionParent(factionID, parentID, inheritRelationships);
			}
			schematic.NodeFinished(this.next);
		}		
		
		public override string GetNodeDetails()
		{
			return useParentFactionActor ? parentActorObject.ToString() : parentFactionName;
		}

	}

}
