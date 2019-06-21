using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
public class changeSfx : MonoBehaviour
{

    public void OnBeginTypewriter() // Assign to typewriter's OnBegin() event.
    {
        // Look up character's audio clip:
        var actorName = DialogueManager.currentConversationState.subtitle.speakerInfo.nameInDatabase;
        var clipName = DialogueManager.masterDatabase.GetActor(actorName).LookupValue("AudioClip");
        var clip = Resources.Load<AudioClip>(clipName);

        // Assign to typewriter:
        var typewriter = GetComponent<AbstractTypewriterEffect>();
        typewriter.audioClip = clip;
  }
}
