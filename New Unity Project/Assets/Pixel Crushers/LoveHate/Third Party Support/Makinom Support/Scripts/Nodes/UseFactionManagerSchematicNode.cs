using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Makinom;
using PixelCrushers.LoveHate;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

	[EditorHelp("Use Faction Manager", "Sets a faction member to use a faction manager.", "")]
	[NodeInfo("Love\u2215Hate")]
	public class UseFactionManagerSchematicNode : BaseSchematicNode
	{

		// Faction member
		[EditorHelp("Use Actor", "Change this actor's (combatant) faction manager.", "")]
        [EditorInfo(titleLabel = "Actor")]
        public SchematicObjectSelection actorObject = new SchematicObjectSelection();

        // New faction manager
        [EditorHelp("Faction Manager", "Specify the faction manager to assign to the faction member.", "")]
        [EditorInfo(titleLabel = "Faction Manager")]
        public FactionManager factionManager = null;


		public UseFactionManagerSchematicNode()
		{
		}

		public override void Execute(Schematic schematic)
		{
            var factionMember = MakinomTools.GetObjectSelectionComponentInChildren<FactionMember>(actorObject, schematic);
			if (factionMember == null)
			{
				Debug.LogWarning("Love/Hate: Use Faction Manager - Can't find faction member");
			}
			if (factionManager == null)
			{
				Debug.LogWarning("Love/Hate: Use Faction Manager - No faction manager was specified");
			}
			else
			{
                if (factionMember.factionManager != null)
                {
                    // Unregister from the old faction manager first:
                    factionMember.factionManager.UnregisterFactionMember(factionMember);
                }
                factionMember.factionManager = factionManager;
                factionManager.RegisterFactionMember(factionMember);
            }
            schematic.NodeFinished(next);
		}		
		
		public override string GetNodeDetails()
		{
            return (factionManager == null) ? "Faction Manager" : factionManager.name;
		}

	}

}
