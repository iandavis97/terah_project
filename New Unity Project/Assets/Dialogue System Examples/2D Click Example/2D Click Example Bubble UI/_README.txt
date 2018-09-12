/*
This example scene demonstrates one way of showing a cartoon bubble above
the NPC's head for dialogue. Each NPC has a world space canvas and a simple
script that positions it above the NPC's head relative to the player when a
conversation starts. Although each NPC has its own instance, you could
write a small script to instantiate the world space canvas at runtime if you
prefer.

The cartoon bubble relies on two things:

1. An Override Dialogue UI Controls component on the bubble UI itself.

2. On the Dialogue Manager's regular dialogue UI (in this scene, it's
Dialogue Manager > Canvas > Generic Unity UI Dialogue UI), the Find Actor
Overrides checkbox is ticked. This tells the dialogue UI to look for 
overrides on NPCs.
*/