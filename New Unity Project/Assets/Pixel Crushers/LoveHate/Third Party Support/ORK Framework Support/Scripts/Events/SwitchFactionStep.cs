using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.LoveHate;
using ORKFramework;
using PixelCrushers.LoveHate.ORKFrameworkSupport;

namespace ORKFramework.Events.Steps
{

	[ORKEditorHelp("Switch Faction", "Changes a faction member's faction.", "")]
	[ORKEventStep(typeof(BaseEvent))]
	[ORKNodeInfo("Love\u2215Hate Steps")]
	public class SwitchFactionStep : BaseEventStep
	{

		// Faction member
		[ORKEditorHelp("Use Actor", "Change this actor's (combatant) faction.", "")]
        [ORKEditorInfo(labelText = "Actor")]
        public EventObjectSetting actorObject = new EventObjectSetting();

        // New faction
        [ORKEditorHelp("Use Actor", "Add this actor's (combatant) faction as the new faction.", "")]
		[ORKEditorInfo(labelText="New Faction")]
		public bool useNewFactionActor = false;
		
		[ORKEditorHelp("Actor", "Select the actor whose faction the target actor will switch to.", "")]
		[ORKEditorInfo(isTabPopup=true, tabPopupID=0)]
		[ORKEditorLayout("useNewFactionActor", true)]
		public EventObjectSetting newFactionActorObject = new EventObjectSetting();
		
		[ORKEditorHelp("Faction", "Enter the name of the new faction the actor is switching to.", "")]
		[ORKEditorLayout(elseCheckGroup=true, endCheckGroup=true)]
		public string newFactionName = string.Empty;


		public SwitchFactionStep()
		{
		}

		public override void Execute(BaseEvent baseEvent)
		{
            var factionManager = OrkEventTools.GetFactionManager(true, actorObject, useNewFactionActor, newFactionActorObject, baseEvent);
            var factionMember = OrkEventTools.GetEventObjectComponentInChildren<FactionMember>(actorObject, baseEvent);
			var newFactionID = OrkEventTools.GetFactionID(useNewFactionActor, newFactionActorObject, newFactionName, factionManager, baseEvent);
			if (factionMember == null)
			{
				Debug.LogWarning("Love/Hate: SwitchFactionStep - Can't find faction member");
			}
			if (newFactionID == -1)
			{
				Debug.LogWarning("Love/Hate: SwitchFactionStep - Can't find new faction ID");
			}
			else
			{
                factionMember.SwitchFaction(newFactionID);
			}
			baseEvent.StepFinished(next);
		}		
		
		public override string GetNodeDetails()
		{
			return useNewFactionActor ? newFactionActorObject.GetInfoText() : newFactionName;
		}

	}

}
