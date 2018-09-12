using UnityEngine;
using Makinom;
using PixelCrushers.LoveHate;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

	[EditorHelp("Add Direct Parent", "Adds a direct parent to a Love/Hate faction.", "")]
	[NodeInfo("Love\u2215Hate")]
	public class AddDirectParentSchematicNode : BaseSchematicNode
	{

        [EditorInfo(separator = true, titleLabel = "Faction")]
        [EditorHelp("Use Actor", "Add a direct parent to this actor's faction.", "")]
		public bool useFactionActor = false;

		[EditorHelp("Actor", "Select the actor whose faction will get a new direct parent.", "")]
		[EditorInfo(isTabPopup=true, tabPopupID=0)]
		[EditorLayout("useFactionActor", true)]
		public SchematicObjectSelection actorObject = new SchematicObjectSelection();
		
		[EditorHelp("Faction", "Enter the name of the faction that will be changed.", "")]
		[EditorLayout(elseCheckGroup=true, endCheckGroup=true)]
		public string factionName = string.Empty;


        // Parent
        [EditorInfo(separator = true, titleLabel = "New Parent")]
        [EditorHelp("Use Actor", "Add this actor's faction as the direct parent.", "")]
		public bool useParentFactionActor = false;
		
		[EditorHelp("Actor", "Select the actor whose faction will be the new direct parent.", "")]
		[EditorInfo(isTabPopup=true, tabPopupID=0)]
		[EditorLayout("useParentFactionActor", true)]
		public SchematicObjectSelection parentActorObject = new SchematicObjectSelection();
		
		[EditorHelp("Parent Faction", "Enter the name of the direct parent faction.", "")]
		[EditorLayout(elseCheckGroup=true, endCheckGroup=true)]
		public string parentFactionName = string.Empty;


		public AddDirectParentSchematicNode()
		{
		}

		public override void Execute(Schematic schematic)
		{
            var factionManager = MakinomTools.GetFactionManager(useFactionActor, actorObject, useParentFactionActor, parentActorObject, schematic);
            var factionID = MakinomTools.GetFactionID(useFactionActor, actorObject, factionName, factionManager, schematic);
            var parentID = MakinomTools.GetFactionID(useParentFactionActor, parentActorObject, parentFactionName, factionManager, schematic);
			if (factionManager == null)
			{
				Debug.LogWarning("Love/Hate: Add Direct Parent - Can't find faction manager");
			}
			if (factionID == -1)
			{
				Debug.LogWarning("Love/Hate: Add Direct Parent - Can't find faction ID");
			}
			else if (parentID == -1)
			{
				Debug.LogWarning("Love/Hate: Add Direct Parent - Can't find parent faction ID");
			}
			else
			{
                factionManager.AddFactionParent(factionID, parentID);
			}
            schematic.NodeFinished(this.next);
		}		
		
		public override string GetNodeDetails()
		{
			return useParentFactionActor 
                ? parentActorObject.ToString()
                : parentFactionName;
		}

	}

}
