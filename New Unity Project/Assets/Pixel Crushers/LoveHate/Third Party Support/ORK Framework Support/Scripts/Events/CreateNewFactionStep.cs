using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.LoveHate;
using ORKFramework;
using PixelCrushers.LoveHate.ORKFrameworkSupport;

namespace ORKFramework.Events.Steps
{

	[ORKEditorHelp("Create New Faction", "Adds a new Love/Hate faction to the faction database.", "")]
	[ORKEventStep(typeof(BaseEvent))]
	[ORKNodeInfo("Love\u2215Hate Steps")]
	public class CreateNewFactionStep : BaseEventStep
	{

        // Faction manager
        [ORKEditorHelp("Faction manager", "Assign to use a specific faction manager.", "")]
        public FactionManager factionManager = null;

        // Faction name
        [ORKEditorHelp("Faction name", "Enter the name of the new faction.", "")]
		public string factionName = string.Empty;

        // Faction name
        [ORKEditorHelp("Description", "Enter an optional description for the faction.", "")]
        public string description = string.Empty;

        public CreateNewFactionStep()
		{
		}

		public override void Execute(BaseEvent baseEvent)
		{
            if (factionManager == null) factionManager = GameObject.FindObjectOfType<FactionManager>();
			if (factionManager == null)
			{
				Debug.LogWarning("Love/Hate: CreateNewFactionStep - Can't find faction manager");
			}
			else
			{
                factionManager.factionDatabase.CreateNewFaction(factionName, description);
			}
			baseEvent.StepFinished(next);
		}		
		
		public override string GetNodeDetails()
		{
            return factionName;
		}

	}

}
