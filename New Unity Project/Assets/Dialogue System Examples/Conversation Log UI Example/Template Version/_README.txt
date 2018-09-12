/*
This variation instantiates a separate GameObject for each line of dialogue,
which gets around the limitation of 65536 vertices in a single large text
element.

To use it, replace your UnityUIDialogueUI with the included TemplatedConversationLogUI.
The easiest way to do this while retaining all the UI element assignments you've
made to UnityUIDialogueUI is:

1. Switch the Inspector to Debug mode using the three bar menu in the upper right.

2. Drag the TemplatedConversationLogUI.cs script into the UnityUIDialogueUI's 
   Script field.

3. Switch the Inspector back to Normal mode.

The TemplatedConversationLogUI adds some extra fields at the bottom of its inspector
that you'll need to assign. See the tooltips on each one, or the example scene, for
instructions.

With each line, the UI will instantiate a copy of the NPC Subtitle elements or
the PC Subtitle elements (depending on the speaker). Only the Line element is
absolutely required.
*/