using UnityEngine;
using System.Collections.Generic;
using Makinom.Schematics;

namespace Makinom
{

	/// <summary>
	/// Conversation data for the Dialogue System's conversation schematic nodes.
	/// </summary>
	public class ConversationData : BaseData
	{
		[EditorHelp("Conversation Title", "The title of the conversation in the dialogue database.", "")]
		public string conversation = string.Empty;
		
		[EditorInfo(separator = true, titleLabel = "Actor")]
		[EditorHelp("Actor", "The initiator", "")]
		public SchematicObjectSelection actorObject = new SchematicObjectSelection();

		[EditorInfo(separator = true, titleLabel = "Conversant")]
		[EditorHelp("Conversant", "The other participant", "")]
		public SchematicObjectSelection conversantObject = new SchematicObjectSelection();

		public ConversationData()
		{
		}
	}
}
