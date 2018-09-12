using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Makinom;
using PixelCrushers.LoveHate;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

	[EditorHelp("Switch Faction", "Changes a faction member's faction.", "")]
	[NodeInfo("Love\u2215Hate")]
	public class SwitchFactionSchematicNode : BaseSchematicNode
	{

		// Faction member
		[EditorHelp("Use Actor", "Change this actor's (combatant) faction.", "")]
        [EditorInfo(titleLabel = "Actor")]
        public SchematicObjectSelection actorObject = new SchematicObjectSelection();

        // New faction
        [EditorHelp("Use Actor", "Add this actor's (combatant) faction as the new faction.", "")]
		[EditorInfo(titleLabel="New Faction")]
		public bool useNewFactionActor = false;
		
		[EditorHelp("Actor", "Select the actor whose faction the target actor will switch to.", "")]
		[EditorInfo(isTabPopup=true, tabPopupID=0)]
		[EditorLayout("useNewFactionActor", true)]
		public SchematicObjectSelection newFactionActorObject = new SchematicObjectSelection();
		
		[EditorHelp("Faction", "Enter the name of the new faction the actor is switching to.", "")]
		[EditorLayout(elseCheckGroup=true, endCheckGroup=true)]
		public string newFactionName = string.Empty;


		public SwitchFactionSchematicNode()
		{
		}

		public override void Execute(Schematic schematic)
		{
            var factionManager = MakinomTools.GetFactionManager(true, actorObject, useNewFactionActor, newFactionActorObject, schematic);
            var factionMember = MakinomTools.GetObjectSelectionComponentInChildren<FactionMember>(actorObject, schematic);
			var newFactionID = MakinomTools.GetFactionID(useNewFactionActor, newFactionActorObject, newFactionName, factionManager, schematic);
			if (factionMember == null)
			{
				Debug.LogWarning("Love/Hate: Switch Faction - Can't find faction member");
			}
			if (newFactionID == -1)
			{
				Debug.LogWarning("Love/Hate: Switch Faction - Can't find new faction ID");
			}
			else
			{
                factionMember.SwitchFaction(newFactionID);
			}
			schematic.NodeFinished(next);
		}		
		
		public override string GetNodeDetails()
		{
			return useNewFactionActor ? newFactionActorObject.ToString() : newFactionName;
		}

	}

}
