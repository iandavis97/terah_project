using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Makinom;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

    [EditorHelp("Inherit Traits", "Sets a faction's traits to the average or sum of its parents' traits based on the faction database's setting.", "")]
	[NodeInfo("Love\u2215Hate")]
	public class InheritTraitsSchematicNode : BaseSchematicNode
    {

		// Faction
		[EditorHelp("Use Actor", "The faction that will inherit its parents' traits.", "")]
		[EditorInfo(titleLabel="Faction")]
		public bool useFactionActor = false;

		[EditorHelp("Actor", "Select the actor whose faction will inherit its parents' traits.", "")]
		[EditorInfo(isTabPopup=true, tabPopupID=0)]
		[EditorLayout("useFactionActor", true)]
		public SchematicObjectSelection actorObject = new SchematicObjectSelection();
		
		[EditorHelp("Faction", "Enter the name of the faction that will inherit its parents' traits.", "")]
		[EditorLayout(elseCheckGroup=true, endCheckGroup=true)]
		public string factionName = string.Empty;

		public InheritTraitsSchematicNode()
		{
		}

		public override void Execute(Schematic schematic)
		{
			var factionManager = MakinomTools.GetFactionManager(useFactionActor, actorObject, false, null, schematic);
			var factionID = MakinomTools.GetFactionID(useFactionActor, actorObject, factionName, factionManager, schematic);
			if (factionManager == null)
			{
				Debug.LogWarning("Love/Hate: Add Direct Parent - Can't find faction manager");
			}
			if (factionID == -1)
			{
				Debug.LogWarning("Love/Hate: Add Direct Parent - Can't find faction ID");
			}
			else
			{
                factionManager.factionDatabase.InheritTraitsFromParents(factionID);
			}
			schematic.NodeFinished(next);
		}		
		
		public override string GetNodeDetails()
		{
			return useFactionActor ? actorObject.ToString() : factionName;
		}

	}

}
