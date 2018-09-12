using UnityEngine;
using Makinom;
using Makinom.Schematics;

namespace PixelCrushers.LoveHate.MakinomSupport
{

    /// <summary>
    /// Plays schematics when various Love/Hate events occurs on a faction member.
    /// </summary>
    [AddComponentMenu("Love\u2215Hate/Third Party/Makinom/Love Hate Event Machine")]
    [RequireComponent(typeof(FactionMember))]
    public class LoveHateEventMachine : MonoBehaviour,
        IWitnessDeedEventHandler, IShareRumorsEventHandler, IEnterAuraEventHandler,
        IGreetEventHandler, IGossipEventHandler
    {

        public MakinomSchematicAsset witnessDeedSchematicAsset = null;
        public MakinomSchematicAsset shareRumorsSchematicAsset = null;
        public MakinomSchematicAsset enterAuraSchematicAsset = null;
        public MakinomSchematicAsset greetSchematicAsset = null;
        public MakinomSchematicAsset gossipSchematicAsset = null;

        public int priority = 0;
        public int inputID = 0;

        protected void PlaySchematic(MakinomSchematicAsset schematicAsset, GameObject startingObject)
        {
            if (schematicAsset == null) return;
            var schematic = new Schematic(schematicAsset);
            schematic.PlaySchematic(priority, null, this.gameObject, startingObject, false, MachineUpdateType.Update, inputID);
        }

        public void OnWitnessDeed(Rumor rumor)
        {
            var factionManager = FindObjectOfType<FactionManager>();
            if (rumor == null || factionManager == null) return;
            Maki.Game.Variables.Set("rumorTag", rumor.tag);
            Maki.Game.Variables.Set("rumorPleasure", rumor.pleasure);
            Maki.Game.Variables.Set("rumorActor", factionManager.GetFaction(rumor.actorFactionID).name);
            Maki.Game.Variables.Set("rumorTarget", factionManager.GetFaction(rumor.targetFactionID).name);
            PlaySchematic(witnessDeedSchematicAsset, null);
        }

        public void OnShareRumors(FactionMember other)
        {
            if (other == null) return;
            PlaySchematic(shareRumorsSchematicAsset, other.gameObject);
        }

        public void OnEnterAura(AbstractAuraTrigger aura)
        {
            if (aura == null) return;
            PlaySchematic(enterAuraSchematicAsset, aura.gameObject);
        }

        public void OnGreet(FactionMember other)
        {
            if (other == null) return;
            PlaySchematic(greetSchematicAsset, other.gameObject);
        }

        public void OnGossip(FactionMember other)
        {
            if (other == null) return;
            PlaySchematic(gossipSchematicAsset, other.gameObject);
        }

    }

}
