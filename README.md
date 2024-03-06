# Procedural Dungeon FPS
Project for Topics in Game Development - Programming and Scripting, Spring 2015

This is a simple FPS game featuring dynamic level generation. The goal of the game is to reach the final room of the level, which is located in the opposite corner of a 5x5 grid of rooms. The player must manage their health and ammunition while fighting a variety of enemy types. 

On level load, the level is created from a set of prefab rooms. A simple pathfinding algorithm is used to ensure that a continuous path exists from the starting room to the final room of the level. After the rooms are loaded in, the enemys which each room spawns are also randomly determined. 
