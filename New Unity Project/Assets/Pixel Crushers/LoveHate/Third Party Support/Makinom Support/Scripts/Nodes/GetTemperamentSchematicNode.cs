using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Makinom;
using PixelCrushers.LoveHate;
using PixelCrushers.LoveHate.MakinomSupport;

namespace Makinom.Schematics.Nodes
{

    [EditorHelp("Get Temperament", "Stores a faction member's temperament in a string variable.", "")]
    [NodeInfo("Love\u2215Hate")]
    public class GetTemperamentStep : BaseSchematicNode
    {

        // Actor
        [EditorHelp("Actor", "Select the actor to check.", "")]
        [EditorInfo(isTabPopup = true, tabPopupID = 0)]
        public SchematicObjectSelection actorObject = new SchematicObjectSelection();


        [EditorHelp("Variable Origin", "Select the origin of the variables:\n" +
                       "- Local: Local variables are only used in a running event and don't interfere with global variables. " +
                       "The variable will be gone once the event ends.\n" +
                       "- Global: Global variables are persistent and available everywhere, everytime. " +
                       "They can be saved in save games.\n" +
                       "- Object: Object variables are bound to objects in the scene by an object ID. " +
                       "They can be saved in save games.", "")]
        public VariableOrigin origin = VariableOrigin.Global;

        // object variables
        [EditorHelp("Use Object", "Use the 'Object Variables' component of game objects to change the object variables.\n" +
                       "The changes will be made on every 'Object Variables' component that is found. " +
                       "If no component is found, no variables will be changed.\n" +
                       "If disabled, you need to define the object ID used to change the object variables.", "")]
        [EditorInfo(separator = true)]
        [EditorLayout("origin", VariableOrigin.Object)]
        public bool useObject = true;

        [EditorInfo(separator = true, titleLabel = "Object")]
        [EditorLayout("useObject", true, autoInit = true)]
        public SchematicObjectSelection variableObject;

        [EditorHelp("Object ID", "Define the object ID of the object variables.\n" +
                       "If the object ID doesn't exist yet, it will be created.", "")]
        [EditorInfo(expandWidth = true)]
        [EditorLayout(elseCheckGroup = true, endCheckGroup = true, endGroups = 2)]
        public string objectID = "";

        // variable key
        [EditorInfo(separator = true, titleLabel = "Variable Key")]
        public StringValue key = new StringValue();

        public GetTemperamentStep()
        {
        }

        public override void Execute(Schematic schematic)
        {
            var factionMember = MakinomTools.GetObjectSelectionComponentInChildren<FactionMember>(actorObject, schematic);
            if (factionMember == null)
            {
                Debug.LogWarning("Love/Hate: Get Temperament - Can't find faction member");
            }
            else
            {
                SetVariableValue(schematic, factionMember.pad.GetTemperament().ToString(), origin, useObject, variableObject, objectID, key);
            }
            schematic.NodeFinished(this.next);
        }

        public override string GetNodeDetails()
        {
            return actorObject.ToString();
        }

        private void SetVariableValue(Schematic schematic, string value,
                                            VariableOrigin origin, bool useObject,
                                            SchematicObjectSelection variableObject, string objectID,
                                            StringValue key)
        {
            switch (origin)
            {
                case VariableOrigin.Global:
                    SetVariableValue(Maki.Game.Variables, value, key);
                    break;
                case VariableOrigin.Local:
                    SetVariableValue(schematic.Variables, value, key);
                    break;
                case VariableOrigin.Object:
                    if (useObject)
                    {
                        SetVariableValue(schematic.Variables, value, key);
                    }
                    else
                    {
                        SetVariableValue(Maki.Game.GetObjectVariables(objectID), value, key);
                    }
                    break;
                case VariableOrigin.ObjectID:
                    SetVariableValue(Maki.Game.GetObjectVariables(objectID), value, key);
                    break;
                default:
                    Debug.LogWarning("Love/Hate: Can't handle variable origin " + origin + " to set variable");
                    break;
            }
        }

        private void SetVariableValue(VariableHandler handler, string value, StringValue key)
        {
            var keyName = key.GetValue(null);
            handler.Set(keyName, value);
        }

    }

}
