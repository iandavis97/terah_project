using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.LoveHate;
using Makinom;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

	[EditorHelp("Destroy Faction", "Removes a Love/Hate faction from the faction database.", "")]
	[NodeInfo("Love\u2215Hate")]
	public class DestroyFactionStep : BaseSchematicNode
	{

        // Faction manager
        [EditorHelp("Faction manager", "Assign to use a specific faction manager.", "")]
        public FactionManager factionManager = null;

        // Faction name
        [EditorHelp("Faction name", "Enter the name of the faction to destroy.", "")]
		public string factionName = string.Empty;

        public DestroyFactionStep()
		{
		}

		public override void Execute(Schematic schematic)
		{
            if (factionManager == null) factionManager = GameObject.FindObjectOfType<FactionManager>();
			if (factionManager == null)
			{
				Debug.LogWarning("Love/Hate: Destroy Faction - Can't find faction manager");
			}
			else
			{
                factionManager.factionDatabase.DestroyFaction(factionName);
			}
			schematic.NodeFinished(this.next);
		}		
		
		public override string GetNodeDetails()
		{
            return factionName;
		}

	}

}
