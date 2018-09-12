using UnityEngine;
using PixelCrushers.DialogueSystem;

// This script repositions the NPC's override dialogue UI when showing a
// line. It also shows the left or right tail of the cartoon bubble 
// depending on the position of the listener.
public class PositionDialogueUI : MonoBehaviour
{

    public Transform characterTransform;
    public RectTransform dialogueCanvas;
    public RectTransform leftTail;
    public RectTransform rightTail;
    public float leftRightOffset; // Offset of the tail's point from the bubble's edge.

    void Start()
    {
        if (characterTransform == null) Debug.LogWarning("Character Transform is unassigned!", this);
        if (dialogueCanvas == null) Debug.LogWarning("Dialogue Canvas is unassigned!", this);
        if (leftTail == null) Debug.LogWarning("Left Tail is unassigned!", this);
        if (rightTail == null) Debug.LogWarning("Right Tail is unassigned!", this);
    }

    void OnConversationLine(Subtitle subtitle)
    {
        if (characterTransform == null || subtitle.speakerInfo.transform == null || 
            subtitle.listenerInfo.transform == null || subtitle.speakerInfo.transform != characterTransform) return;

        // Determine if the player is to the left or right:
        var isLeftOfListener = (characterTransform.position.x < subtitle.listenerInfo.transform.position.x);

        // Show the corresponding tail for the bubble:
        leftTail.gameObject.SetActive(isLeftOfListener);
        rightTail.gameObject.SetActive(!isLeftOfListener);

        // Move the bubble so the tail is above the NPC.
        var xOffset = (isLeftOfListener ? 1 : -1) * leftRightOffset * characterTransform.localScale.x;
        dialogueCanvas.position = new Vector3(characterTransform.position.x + xOffset, dialogueCanvas.position.y, dialogueCanvas.position.z);
    }

}
