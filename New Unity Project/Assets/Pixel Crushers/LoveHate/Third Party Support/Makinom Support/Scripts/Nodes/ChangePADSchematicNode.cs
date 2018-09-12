using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Makinom;
using PixelCrushers.LoveHate;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

    [EditorHelp("Change PAD", "Changes a faction member's PAD values.", "")]
    [NodeInfo("Love\u2215Hate")]
    public class ChangePADSteps : BaseSchematicNode
    {

        // Actor
        [EditorHelp("Actor", "Select the actor committing the deed.", "")]
        [EditorInfo(titleLabel = "Actor")]
        public SchematicObjectSelection actorObject = new SchematicObjectSelection();


        // Happiness
        [EditorInfo(titleLabel = "Change Happiness By", separator = true)]
        public FloatValue valueHappiness = new FloatValue();

        [EditorHelp("Operator", "Select how the happiness value will be changed:\n" +
                       "- Add: The value will be added to the current happiness value.\n" +
                       "- Sub: The value will be subtracted from the current happiness value.\n" +
                       "- Set: The happiness value will be set to the value." +
                       "Happiness is in the range [-100,+100].", "")]
        [EditorInfo(isEnumToolbar = true, toolbarWidth = 50)]
        public FloatOperator opHappiness = FloatOperator.Add;



        // Happiness
        [EditorInfo(titleLabel = "Change Pleasure By", separator = true)]
        public FloatValue valuePleasure = new FloatValue();

        [EditorHelp("Operator", "Select how the pleasure value will be changed:\n" +
                       "- Add: The value will be added to the current pleasure value.\n" +
                       "- Sub: The value will be subtracted from the current pleasure value.\n" +
                       "- Set: The pleasure value will be set to the value." +
                       "Pleasure is in the range [-100,+100].", "")]
        [EditorInfo(isEnumToolbar = true, toolbarWidth = 50)]
        public FloatOperator opPleasure = FloatOperator.Add;



        // Happiness
        [EditorInfo(titleLabel = "Change Arousal By", separator = true)]
        public FloatValue valueArousal = new FloatValue();

        [EditorHelp("Operator", "Select how the arousal value will be changed:\n" +
                       "- Add: The value will be added to the current arousal value.\n" +
                       "- Sub: The value will be subtracted from the current arousal value.\n" +
                       "- Set: The arousal value will be set to the value." +
                       "Arousal is in the range [-100,+100].", "")]
        [EditorInfo(isEnumToolbar = true, toolbarWidth = 50)]
        public FloatOperator opArousal = FloatOperator.Add;



        // Happiness
        [EditorInfo(titleLabel = "Change Dominance By", separator = true)]
        public FloatValue valueDominance = new FloatValue();

        [EditorHelp("Operator", "Select how the dominance value will be changed:\n" +
                       "- Add: The value will be added to the current dominance value.\n" +
                       "- Sub: The value will be subtracted from the current dominance value.\n" +
                       "- Set: The dominance value will be set to the value." +
                       "Dominance is in the range [-100,+100].", "")]
        [EditorInfo(isEnumToolbar = true, toolbarWidth = 50)]
        public FloatOperator opDominance = FloatOperator.Add;

        public ChangePADSteps()
        {
        }

        public override void Execute(Schematic schematic)
        {
            var factionMember = MakinomTools.GetObjectSelectionComponentInChildren<FactionMember>(actorObject, schematic);
            if (factionMember == null)
            {
                Debug.LogWarning("Love/Hate: Change PAD - Can't find faction member");
            }
            else
            {
                factionMember.pad.happiness = GetNewValue(factionMember.pad.happiness, opHappiness, valueHappiness, schematic);
                factionMember.pad.pleasure = GetNewValue(factionMember.pad.pleasure, opPleasure, valuePleasure, schematic);
                factionMember.pad.arousal = GetNewValue(factionMember.pad.arousal, opArousal, valueArousal, schematic);
                factionMember.pad.dominance = GetNewValue(factionMember.pad.dominance, opDominance, valueDominance, schematic);
            }
            schematic.NodeFinished(this.next);
        }

        private float GetNewValue(float originalValue, FloatOperator op, FloatValue value, Schematic schematic)
        {
            var newValue = originalValue;
            switch (op)
            {
                case FloatOperator.Set:
                    newValue = value.GetValue(null);
                    break;
                case FloatOperator.Add:
                    newValue += value.GetValue(null);
                    break;
                case FloatOperator.Sub:
                    newValue -= value.GetValue(null);
                    break;
            }
            return Mathf.Clamp(newValue, -100, 100);
        }

        public override string GetNodeDetails()
        {
            return actorObject.ToString();
        }

    }

}
