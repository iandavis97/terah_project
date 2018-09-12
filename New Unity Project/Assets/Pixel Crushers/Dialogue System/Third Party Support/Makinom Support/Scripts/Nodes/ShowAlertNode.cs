using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Makinom;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

	/// <summary>
	/// This node shows a Dialogue System alert message.
	/// </summary>
	[EditorHelp("Show Alert", "Plays a Dialogue System sequence. Doesn't wait for it to end.", "")]
	[NodeInfo("Dialogue System")]
	public class ShowAlertNode : BaseSchematicNode
	{

        [EditorHelp("Alert Message", "The message to show.", "")]
        public StringValue alertMessage = new StringValue();

		[EditorHelp("Duration", "(Optiona) If non-zero, the duration to show the message.", "")]
		public float duration = 0;

		public ShowAlertNode()
		{
		}
		
		public override void Execute(Schematic schematic)
		{
			if (Mathf.Approximately(0, duration)) {
				DialogueManager.ShowAlert(alertMessage.GetValue(null));
			} else {
				DialogueManager.ShowAlert(alertMessage.GetValue(null), duration);
			}
			schematic.NodeFinished(this.next);
		}

		public override string GetNodeDetails()
		{
			return alertMessage.GetValue(null);
		}

	}
}
