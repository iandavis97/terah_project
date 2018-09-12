using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.LoveHate;
using Makinom;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

    [EditorHelp("Check Pleasure", "Which steps will be executed next is decided by a faction member's pleasure value.", "")]
    [NodeInfo("Love\u2215Hate")]
    public class CheckPleasureStep : BaseSchematicCheckNode
    {

        // Actor
        [EditorHelp("Actor", "Select the actor to check.", "")]
        [EditorInfo(isTabPopup = true, tabPopupID = 0)]
        public SchematicObjectSelection actorObject = new SchematicObjectSelection();


        // Value
        [EditorHelp("Check Type", "Checks if the value is equal, not equal, less or greater than a defined value.\n" +
                       "Pleasure is in the range [-100,+100].\n" +
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


        public CheckPleasureStep()
        {
        }

        public override void Execute(Schematic schematic)
        {
            var factionMember = MakinomTools.GetObjectSelectionComponentInChildren<FactionMember>(actorObject, schematic);
            if (factionMember == null)
            {
                Debug.LogWarning("Love/Hate: Check Pleasure - Can't find faction member");
            }
            else
            {
                var pleasure = factionMember.pad.pleasure;
                if (ValueHelper.CheckValue(
                    pleasure,
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
            return actorObject.ToString() + " " +
                check + " " + checkValue.ToString() +
                ((ValueCheck.RangeInclusive.Equals(this.check) ||
                  ValueCheck.RangeExclusive.Equals(this.check)) ?
                  " - " + this.checkValue2.ToString() : "");
        }

    }

}
