using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.LoveHate;
using ORKFramework;
using PixelCrushers.LoveHate.ORKFrameworkSupport;

namespace ORKFramework.Events.Steps
{

	[ORKEditorHelp("Destroy Faction", "Removes a Love/Hate faction from the faction database.", "")]
	[ORKEventStep(typeof(BaseEvent))]
	[ORKNodeInfo("Love\u2215Hate Steps")]
	public class DestroyFactionStep : BaseEventStep
	{

        // Faction manager
        [ORKEditorHelp("Faction manager", "Assign to use a specific faction manager.", "")]
        public FactionManager factionManager = null;

        // Faction name
        [ORKEditorHelp("Faction name", "Enter the name of the faction to destroy.", "")]
		public string factionName = string.Empty;

        public DestroyFactionStep()
		{
		}

		public override void Execute(BaseEvent baseEvent)
		{
            if (factionManager == null) factionManager = GameObject.FindObjectOfType<FactionManager>();
			if (factionManager == null)
			{
				Debug.LogWarning("Love/Hate: DestroyFactionStep - Can't find faction manager");
			}
			else
			{
                factionManager.factionDatabase.DestroyFaction(factionName);
			}
			baseEvent.StepFinished(next);
		}		
		
		public override string GetNodeDetails()
		{
            return factionName;
		}

	}

}
