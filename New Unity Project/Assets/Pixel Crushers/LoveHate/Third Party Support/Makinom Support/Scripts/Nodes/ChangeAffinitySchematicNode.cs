using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Makinom;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

    [EditorHelp("Change Affinity", "Changes the affinity of a faction for another faction.", "")]
    [NodeInfo("Love\u2215Hate")]
    public class ChangeAffinityStep : BaseSchematicNode
    {

        // Judge
        [EditorHelp("Use Actor", "The judging actor's faction.", "")]
        [EditorInfo(titleLabel = "Judge Faction")]
        public bool useJudgeActor = false;

        [EditorHelp("Actor", "Select the actor whose faction will judge affinity to the subject.", "")]
        [EditorInfo(isTabPopup = true, tabPopupID = 0)]
        [EditorLayout("useJudgeActor", true)]
        public SchematicObjectSelection judgeObject = new SchematicObjectSelection();

        [EditorHelp("Judge Faction", "Enter the name of the faction that will judge affinity to the subject.", "")]
        [EditorLayout(elseCheckGroup = true, endCheckGroup = true)]
        public string judgeFactionName = string.Empty;


        // Subject
        [EditorHelp("Use Actor", "The subject actor's faction.", "")]
        [EditorInfo(titleLabel = "Subject Faction")]
        public bool useSubjectActor = false;

        [EditorHelp("Actor", "Select the subject actor whose faction will be judged.", "")]
        [EditorInfo(isTabPopup = true, tabPopupID = 0)]
        [EditorLayout("useSubjectActor", true)]
        public SchematicObjectSelection subjectObject = new SchematicObjectSelection();

        [EditorHelp("Subject Faction", "Enter the name of the subject faction.", "")]
        [EditorLayout(elseCheckGroup = true, endCheckGroup = true)]
        public string subjectFactionName = string.Empty;


        // Change by
        [EditorInfo(titleLabel = "Change By", separator = true)]
        public FloatValue value = new FloatValue();

        [EditorHelp("Operator", "Select how the affinity value will be changed:\n" +
                       "- Add: The value will be added to the current affinity value.\n" +
                       "- Sub: The value will be subtracted from the current affinity value.\n" +
                       "- Set: The affinity value will be set to the value." +
                       "Affinity is in the range [-100,+100].", "")]
        [EditorInfo(isEnumToolbar = true, toolbarWidth = 50)]
        public FloatOperator op = FloatOperator.Add;

        public ChangeAffinityStep()
        {
        }

        public override void Execute(Schematic schematic)
        {
            var factionManager = MakinomTools.GetFactionManager(useJudgeActor, judgeObject, useSubjectActor, subjectObject, schematic);
            var judgeID = MakinomTools.GetFactionID(useJudgeActor, judgeObject, judgeFactionName, factionManager, schematic);
            var subjectID = MakinomTools.GetFactionID(useSubjectActor, subjectObject, subjectFactionName, factionManager, schematic);
            if (factionManager == null)
            {
                Debug.LogWarning("Love/Hate: Change Affinity - Can't find faction manager");
            }
            if (judgeID == -1)
            {
                Debug.LogWarning("Love/Hate: Change Affinity - Can't find judge faction ID");
            }
            else if (subjectID == -1)
            {
                Debug.LogWarning("Love/Hate: Change Affinity - Can't find subject faction ID");
            }
            else
            {
                switch (op)
                {
                    case FloatOperator.Set:
                        factionManager.SetPersonalAffinity(judgeID, subjectID, value.GetValue(null));
                        break;
                    case FloatOperator.Add:
                        factionManager.ModifyPersonalAffinity(judgeID, subjectID, value.GetValue(null));
                        break;
                    case FloatOperator.Sub:
                        factionManager.ModifyPersonalAffinity(judgeID, subjectID, -value.GetValue(null));
                        break;
                }
            }
            schematic.NodeFinished(this.next);
        }

        public override string GetNodeDetails()
        {
            return (useSubjectActor ? subjectObject.ToString() : subjectFactionName) + " " +
                op.ToString() + " " + value.ToString();
        }

    }

}
