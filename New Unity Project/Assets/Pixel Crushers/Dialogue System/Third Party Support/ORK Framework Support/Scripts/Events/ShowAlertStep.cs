using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ORKFramework;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.ORKFrameworkSupport;

namespace ORKFramework.Events.Steps
{

	/// <summary>
	/// This ORK step shows a Dialogue System alert message.
	/// </summary>
	[ORKEditorHelp("Show Alert", "Shows an alert message.", "")]
	[ORKEventStep(typeof(BaseEvent))]
	[ORKNodeInfo("Dialogue System Steps")]
	public class ShowAlertStep : BaseEventStep
	{

		[ORKEditorHelp("Alert Message", "The message to show.", "")]
		[ORKEditorInfo(labelText="Alert Message", expandWidth=true)]
		public string alertMessage = string.Empty;

		[ORKEditorHelp("Duration", "(Optional) If non-zero, the number of seconds to show the message.", "")]
		[ORKEditorInfo(labelText="Duration")]
		public float duration;

		public ShowAlertStep()
		{
		}
		
		public override void Execute(BaseEvent baseEvent)
		{
            if (DialogueDebug.LogInfo) Debug.Log("Dialogue System: ORK Step Show Alert: '" + alertMessage + "'");
            if (Mathf.Approximately(0, duration)) {
				DialogueManager.ShowAlert(alertMessage);
			} else {
				DialogueManager.ShowAlert(alertMessage, duration);
			}
			baseEvent.StepFinished(this.next);
		}

		public override string GetNodeDetails()
		{
			return alertMessage;
		}

	}
}
