using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Sets the conversation's title to the name of the conversant.
/// </summary>
public class SetConversationTitle : MonoBehaviour
{

    public UnityEngine.UI.Text title;

    void OnConversationStart(Transform actor)
    {
        title.text = OverrideActorName.GetActorName(DialogueManager.CurrentConversant);
    }
}
