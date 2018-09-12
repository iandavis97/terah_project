using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.LoveHate;
using Makinom;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

    [EditorHelp("Check Affinity", "Which steps will be executed next is decided by a judge faction's affinity to a subject faction.", "")]
    [NodeInfo("Love\u2215Hate")]
    public class CheckAffinityStep : BaseSchematicCheckNode
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


        // Value
        [EditorHelp("Check Type", "Checks if the value is equal, not equal, less or greater than a defined value.\n" +
                       "Affinity is in the range [-100,+100].\n" +
                       "Range inclusive checks if the value is between two defind values, including the values.\n" +
                       "Range exclusive checks if the value is between two defined values, excluding the values.\n" +
                       "Approximately checks if the value is similar to the defined value.", "")]
        [EditorInfo(separator = true)]
        public ValueCheck check = ValueCheck.IsEqual;

        [EditorInfo(separator = true, titleLabel = "Check Value")]
        public FloatValue checkValue = new FloatValue();

        [EditorInfo(separator = true, titleLabel = "Check Value 2")]
        [EditorLayout(new string[] { "check", "check" },
        new System.Object[] { ValueCheck.RangeInclusive, ValueCheck.RangeExclusive },
        needed = Needed.One, endCheckGroup = true, autoInit = true)]
        public FloatValue checkValue2;


        public CheckAffinityStep()
        {
        }

        public override void Execute(Schematic schematic)
        {
            var factionManager = MakinomTools.GetFactionManager(useJudgeActor, judgeObject, useSubjectActor, subjectObject, schematic);
            var judgeID = MakinomTools.GetFactionID(useJudgeActor, judgeObject, judgeFactionName, factionManager, schematic);
            var subjectID = MakinomTools.GetFactionID(useSubjectActor, subjectObject, subjectFactionName, factionManager, schematic);
            if (factionManager == null)
            {
                Debug.LogWarning("Love/Hate: Check Affinity - Can't find faction manager");
            }
            if (judgeID == -1)
            {
                Debug.LogWarning("Love/Hate: Check Affinity - Can't find judge faction ID");
            }
            else if (subjectID == -1)
            {
                Debug.LogWarning("Love/Hate: Check Affinity - Can't find subject faction ID");
            }
            else
            {
                var affinity = factionManager.GetAffinity(judgeID, subjectID);

                if (ValueHelper.CheckValue(
                    affinity,
                    checkValue.GetValue(null),
                    checkValue2 != null ? this.checkValue2.GetValue(null) : 0,
                    check))
                {
                    schematic.NodeFinished(this.next);
                }
                else
                {
                    schematic.NodeFinished(this.nextFail);
                }
            }
            schematic.NodeFinished(this.next);
        }

        public override string GetNodeDetails()
        {
            return (useSubjectActor ? subjectObject.ToString() : subjectFactionName) + " " +
                check + " " + checkValue.ToString() +
                ((ValueCheck.RangeInclusive.Equals(this.check) ||
                  ValueCheck.RangeExclusive.Equals(this.check)) ?
                  " - " + this.checkValue2.ToString() : "");
        }

    }

}
