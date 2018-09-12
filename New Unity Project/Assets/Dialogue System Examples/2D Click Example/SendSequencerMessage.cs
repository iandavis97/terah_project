using UnityEngine;

public class SendSequencerMessage : MonoBehaviour
{
    public void SendToSequencer(string message)
    {
        PixelCrushers.DialogueSystem.Sequencer.Message(message);
    }
}
