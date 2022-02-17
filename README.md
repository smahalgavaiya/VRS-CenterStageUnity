# Virtual-FTC Constructor Kit

This repo contains a constructor kit for building the field, robots, etc.

Full documentation coming soon!

(For now, here's a video explanation of the systems: https://www.dropbox.com/s/puz5622lbuob8fx/video1667860698.mp4?dl=0)

# Getting Started

(In your own branch) either open the FieldTemplate scene or create a new scene using the VRS Field template (available in the "New Scene" window).

# Understanding the Basics

The intent of this kit is to provide modular parts that make it easy to design field layouts that work "right out of the box". 

The key aspects of the kit are these:

* Scoring Object Types
* Score Object Locations
* Scoring Guides
* Goals
* Indexes
	* Round Index
	* Score Index
	* Material Index
* Events (for tracking "one-off" events like a button press)
* Drives (for translating vector value events into motion, such as a thumbstick)

# Scoring Object Types

Scoring Object Types can be created two ways:

1. From the main menu, select VRS > Scoring > Create Scoring Object Type

This is the preferred way since it walks you through all that is needed to create a Scoring Object Type

2. Right click in the Project Window to create an asset, and select Scoring > Scoring Object Type from the top of the pop-up menu. The Scoring Object Types need to live in the same folder 

## What's in a Scoring Object Type?

Right now Scoring Object Types have an associated prefab. 

*Why don't we just use the prefab?*

The Scoring Object Type is a flexible data container that allows for future information that might be relevant to the game. For instance, there might be a type that uses multiple prefabs, chosen randomly. Or we might want to include information that restricts the type to a certain round, or set of rounds. By building the system around a flexible data container, we allow for these future changes. 

## What else uses the Scoring Object Type?

Multiple systems dynamically draw on the Scoring Object Types in the SpawnableObjects folder. See Scoring Guides and Scoring Object Locations below.

# Scoring 