using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.LoveHate;
using Makinom;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

	[EditorHelp("Create New Faction", "Adds a new Love/Hate faction to the faction database.", "")]
	[NodeInfo("Love\u2215Hate")]
	public class CreateNewFactionStep : BaseSchematicNode
	{

        // Faction manager
        [EditorHelp("Faction manager", "Assign to use a specific faction manager.", "")]
        public FactionManager factionManager = null;

        // Faction name
        [EditorHelp("Faction name", "Enter the name of the new faction.", "")]
		public string factionName = string.Empty;

        // Faction name
        [EditorHelp("Description", "Enter an optional description for the faction.", "")]
        public string description = string.Empty;

        public CreateNewFactionStep()
		{
		}

		public override void Execute(Schematic schematic)
		{
            if (factionManager == null) factionManager = GameObject.FindObjectOfType<FactionManager>();
			if (factionManager == null)
			{
				Debug.LogWarning("Love/Hate: Create New Faction - Can't find faction manager");
			}
			else
			{
                factionManager.factionDatabase.CreateNewFaction(factionName, description);
			}
			schematic.NodeFinished(this.next);
		}		
		
		public override string GetNodeDetails()
		{
            return factionName;
		}

	}

}
