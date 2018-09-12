using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ORKFramework;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.ORKFrameworkSupport;

namespace ORKFramework.Events.Steps
{

	/// <summary>
	/// This ORK step plays a Dialogue System sequence.
	/// </summary>
	[ORKEditorHelp("Play Sequence", "Plays a sequence.", "")]
	[ORKEventStep(typeof(BaseEvent))]
	[ORKNodeInfo("Dialogue System Steps")]
	public class PlaySequenceStep : BaseEventStep
	{

		[ORKEditorHelp("Sequence", "The sequence to play.", "")]
		[ORKEditorInfo(labelText="Sequence", expandWidth=true)]
		public string sequence = string.Empty;

		[ORKEditorHelp("Speaker", "(Optional) The sequencer's speaker.", "")]
		[ORKEditorInfo(labelText="Speaker")]
		public EventObjectSetting actorObject = new EventObjectSetting();
		
		[ORKEditorHelp("Listener", "(Optional) The sequencer's listener.", "")]
		[ORKEditorInfo(labelText="Listener")]
		public EventObjectSetting conversantObject = new EventObjectSetting();
		
		public PlaySequenceStep()
		{
		}
		
		public override void Execute(BaseEvent baseEvent)
		{
			var actor = ORKEventTools.GetEventObjectTransform(actorObject, baseEvent);
			var conversant = ORKEventTools.GetEventObjectTransform(conversantObject, baseEvent);
			DialogueManager.PlaySequence(sequence, actor, conversant);
			baseEvent.StepFinished(this.next);
		}

		public override string GetNodeDetails()
		{
			return sequence;
		}

	}
}
