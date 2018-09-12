using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.LoveHate;
using ORKFramework;
using PixelCrushers.LoveHate.ORKFrameworkSupport;

namespace ORKFramework.Events.Steps
{

	[ORKEditorHelp("Use Faction Manager", "Changes a faction member's faction manager.", "")]
	[ORKEventStep(typeof(BaseEvent))]
	[ORKNodeInfo("Love\u2215Hate Steps")]
	public class UseFactionManagerStep : BaseEventStep
	{

		// Faction member
		[ORKEditorHelp("Use Actor", "Change this actor's (combatant) faction.", "")]
        [ORKEditorInfo(labelText = "Actor")]
        public EventObjectSetting actorObject = new EventObjectSetting();

        // New faction
        [ORKEditorHelp("Faction Manager", "Specify the new faction manager.", "")]
        [ORKEditorInfo(labelText = "Faction Manager")]
        public FactionManager factionManager = null;


		public UseFactionManagerStep()
		{
		}

		public override void Execute(BaseEvent baseEvent)
		{
            var factionMember = OrkEventTools.GetEventObjectComponentInChildren<FactionMember>(actorObject, baseEvent);
			if (factionMember == null)
			{
				Debug.LogWarning("Love/Hate: UseFactionManagerStep - Can't find faction member");
			}
			if (factionManager == null)
			{
				Debug.LogWarning("Love/Hate: UseFactionManagerStep - No faction manager was specified");
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
            baseEvent.StepFinished(next);
		}		
		
		public override string GetNodeDetails()
		{
            return (factionManager == null) ? "Faction Manager" : factionManager.name;
        }

	}

}
