using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.LoveHate;
using ORKFramework;
using PixelCrushers.LoveHate.ORKFrameworkSupport;

namespace ORKFramework.Events.Steps
{

	[ORKEditorHelp("Inherit Traits", "Sets a faction's traits to the average or sum of its parents' traits based on the faction database's setting.", "")]
	[ORKEventStep(typeof(BaseEvent))]
	[ORKNodeInfo("Love\u2215Hate Steps")]
	public class InheritTraitsStep : BaseEventStep
	{

		// Faction
		[ORKEditorHelp("Use Actor", "The faction that will inherit its parents' traits.", "")]
		[ORKEditorInfo(labelText="Faction")]
		public bool useFactionActor = false;

		[ORKEditorHelp("Actor", "Select the actor whose faction will inherit its parents' traits.", "")]
		[ORKEditorInfo(isTabPopup=true, tabPopupID=0)]
		[ORKEditorLayout("useFactionActor", true)]
		public EventObjectSetting actorObject = new EventObjectSetting();
		
		[ORKEditorHelp("Faction", "Enter the name of the faction that will inherit its parents' traits.", "")]
		[ORKEditorLayout(elseCheckGroup=true, endCheckGroup=true)]
		public string factionName = string.Empty;

		public InheritTraitsStep()
		{
		}

		public override void Execute(BaseEvent baseEvent)
		{
			var factionManager = OrkEventTools.GetFactionManager(useFactionActor, actorObject, false, null, baseEvent);
			var factionID = OrkEventTools.GetFactionID(useFactionActor, actorObject, factionName, factionManager, baseEvent);
			if (factionManager == null)
			{
				Debug.LogWarning("Love/Hate: AddDirectParentStep - Can't find faction manager");
			}
			if (factionID == -1)
			{
				Debug.LogWarning("Love/Hate: AddDirectParentStep - Can't find faction ID");
			}
			else
			{
                factionManager.factionDatabase.InheritTraitsFromParents(factionID);
			}
			baseEvent.StepFinished(next);
		}		
		
		public override string GetNodeDetails()
		{
			return useFactionActor ? actorObject.GetInfoText() : factionName;
		}

	}

}
