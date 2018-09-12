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
    [ORKEditorHelp("Show Variable Alert", "Shows an alert message using an ORK variable string value.", "")]
    [ORKEventStep(typeof(BaseEvent))]
    [ORKNodeInfo("Dialogue System Steps")]
    public class ShowVariableAlertStep : BaseEventStep
    {

        [ORKEditorHelp("Alert Message", "The message to show.", "")]
        [ORKEditorInfo(labelText = "Alert Message", expandWidth = true)]
        public StringValue alertMessage = new StringValue();

        [ORKEditorHelp("Duration", "(Optional) If non-zero, the number of seconds to show the message.", "")]
        [ORKEditorInfo(labelText = "Duration")]
        public float duration;

        public ShowVariableAlertStep()
        {
        }

        public override void Execute(BaseEvent baseEvent)
        {
            if (DialogueDebug.LogInfo) Debug.Log("Dialogue System: ORK Step Show Variable Alert: '" + alertMessage.GetValue() + "'");
            if (Mathf.Approximately(0, duration))
            {
                DialogueManager.ShowAlert(alertMessage.GetValue());
            }
            else {
                DialogueManager.ShowAlert(alertMessage.GetValue(), duration);
            }
            baseEvent.StepFinished(this.next);
        }

        public override string GetNodeDetails()
        {
            return alertMessage.value;
        }

    }
}
