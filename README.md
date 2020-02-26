# Dulce-Periculum

WORK IN PROGRESS

A simple computer game, where the user has to save the village and defeat all enemies.

There is a village with villagers that are walking. If a villager sees a enemy, he tries to run away.
There are goblins and a ghost. 
Each goblin is armed with an axe. Some of them are stealing stuff from the village, some are attacking the player, other are walking around their base point.
If they have stolen stuff, they run to their base point. If the player come to their base point, they attack him.
The ghost is walking around its base point. If the player is near, it attacks him using magic.

AI for NPCs is implemented via behavior trees. All assets has been downloaded, those references are in doc/"Downloaded assets.txt".

Control:
- WASD ... moving
- Left mouse button ... if (the sword is sheathed) Unsheathe the sword; else Attack;
- Right mouse button ... A spell for the sword. Its power is increased, an visual effect appears.
- Mouse wheel ... Sheathe the sword.
- P, Escape ... The pause menu.