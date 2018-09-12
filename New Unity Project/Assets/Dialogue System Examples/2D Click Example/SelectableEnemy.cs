using UnityEngine;
using PixelCrushers.DialogueSystem;

public class SelectableEnemy : MonoBehaviour
{
    void OnMouseUp()
    {
        DialogueLua.SetVariable("Selection", name);
        Sequencer.Message("EnemySelected");
    }
}
