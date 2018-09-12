﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.LoveHate;
using ORKFramework;
using PixelCrushers.LoveHate.ORKFrameworkSupport;

namespace ORKFramework.Events.Steps
{

	[ORKEditorHelp("Add Direct Parent", "Adds a direct parent to a Love/Hate faction.", "")]
	[ORKEventStep(typeof(BaseEvent))]
	[ORKNodeInfo("Love\u2215Hate Steps")]
	public class AddDirectParentStep : BaseEventStep
	{

		// Faction
		[ORKEditorHelp("Use Actor", "Add a direct parent to this actor's (combatant) faction.", "")]
		[ORKEditorInfo(labelText="Faction")]
		public bool useFactionActor = false;

		[ORKEditorHelp("Actor", "Select the actor whose faction will get a new direct parent.", "")]
		[ORKEditorInfo(isTabPopup=true, tabPopupID=0)]
		[ORKEditorLayout("useFactionActor", true)]
		public EventObjectSetting actorObject = new EventObjectSetting();
		
		[ORKEditorHelp("Faction", "Enter the name of the faction that will be changed.", "")]
		[ORKEditorLayout(elseCheckGroup=true, endCheckGroup=true)]
		public string factionName = string.Empty;


		// Parent
		[ORKEditorHelp("Use Actor", "Add this actor's (combatant) faction as the direct parent.", "")]
		[ORKEditorInfo(labelText="Parent Faction")]
		public bool useParentFactionActor = false;
		
		[ORKEditorHelp("Actor", "Select the actor whose faction will be the new direct parent.", "")]
		[ORKEditorInfo(isTabPopup=true, tabPopupID=0)]
		[ORKEditorLayout("useParentFactionActor", true)]
		public EventObjectSetting parentActorObject = new EventObjectSetting();
		
		[ORKEditorHelp("Faction", "Enter the name of the direct parent faction.", "")]
		[ORKEditorLayout(elseCheckGroup=true, endCheckGroup=true)]
		public string parentFactionName = string.Empty;


		public AddDirectParentStep()
		{
		}

		public override void Execute(BaseEvent baseEvent)
		{
			var factionManager = OrkEventTools.GetFactionManager(useFactionActor, actorObject, useParentFactionActor, parentActorObject, baseEvent);
			var factionID = OrkEventTools.GetFactionID(useFactionActor, actorObject, factionName, factionManager, baseEvent);
			var parentID = OrkEventTools.GetFactionID(useParentFactionActor, parentActorObject, parentFactionName, factionManager, baseEvent);
			if (factionManager == null)
			{
				Debug.LogWarning("Love/Hate: AddDirectParentStep - Can't find faction manager");
			}
			if (factionID == -1)
			{
				Debug.LogWarning("Love/Hate: AddDirectParentStep - Can't find faction ID");
			}
			else if (parentID == -1)
			{
				Debug.LogWarning("Love/Hate: AddDirectParentStep - Can't find parent faction ID");
			}
			else
			{
				factionManager.AddFactionParent(factionID, parentID);
			}
			baseEvent.StepFinished(next);
		}		
		
		public override string GetNodeDetails()
		{
			return useParentFactionActor ? parentActorObject.GetInfoText() : parentFactionName;
		}

	}

}
