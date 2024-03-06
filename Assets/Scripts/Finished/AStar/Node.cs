/********************************************************
 * NAME: Travis Wright									*
 * CLASS: 4373.003										*
 * ASSIGNMENT: Project 4								*
 * FILE: Node.cs										*
 * DESCRIPTION: The class responsible for the individual*
 * 		nodes in the pathfinding grid					*
 * ******************************************************/

using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>
{
	public bool walkable; //can a unit walk on or through this node
	public Vector3 worldPosition; //the world position as a vector3 of this position
	public int gridX; //the x position in the 2d array
	public int gridY; //the y position in the 2d array
	public int gCost; //cost from starting position
	public int hCost; //predicted cost from current square
	public Node parent; //cheapest node leading to this node
	int heapIndex;	//index within the heap structure

	//construcstor
	public Node(bool walk, Vector3 worldPos, int x, int y)
	{
		walkable = walk;
		worldPosition = worldPos;
		gridX = x;
		gridY = y;
	}

	//fcost is sum of gCost and hCost
	public int fCost
	{
		get{
			return hCost+gCost;
		}
	}

	//heapIndex
	public int HeapIndex
	{
		get{
			return heapIndex;
		}
		set{
			heapIndex = value;
		}
	}

	//function that compares costs of one node to another
	public int CompareTo(Node node)
	{
		int compare = fCost.CompareTo(node.fCost);
		if(compare == 0)
		{
			compare = hCost.CompareTo(node.hCost);
		}
		return -compare;
	}
}
