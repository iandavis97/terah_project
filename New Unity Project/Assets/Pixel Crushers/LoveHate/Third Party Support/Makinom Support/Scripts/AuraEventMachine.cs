using UnityEngine;
using Makinom;
using Makinom.Schematics;

namespace PixelCrushers.LoveHate.MakinomSupport
{

    /// <summary>
    /// This script runs a schematic when an aura is triggered.
    /// </summary>
    [AddComponentMenu("Love\u2215Hate/Third Party/Makinom/Aura Event Machine")]
    public class AuraEventMachine : MonoBehaviour, IAuraEventHandler
    {

        public MakinomSchematicAsset schematicAsset;
        public int priority = 0;
        public int inputID = 0;

        public void OnAura(FactionMember other)
        {
            var schematic = new Schematic(schematicAsset);
            schematic.PlaySchematic(priority, null, this.gameObject, other.gameObject, false, MachineUpdateType.Update, inputID);
        }

    }

}

