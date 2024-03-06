/********************************************************
 * NAME: Travis Wright									*
 * CLASS: 4373.003										*
 * ASSIGNMENT: Project 4								*
 * FILE: Pathfinding.cs									*
 * DESCRIPTION: Used for all pathfinding by enemy AI to	*
 * 		find, simplify, and follow a path using 		*
 * 		cotoutines.										*
 * ******************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour 
{
	//requestmanager instance
	RequestManager requestManager;

	//grid instance
	Grid grid;

	//initialization
	void Awake()
	{
		//get grid component
		grid = GetComponent<Grid>();
		//get requestmanager component
		requestManager = GetComponent<RequestManager>();
	}

	//start the findpath method
	public void startFindPath(Vector3 startPos, Vector3 endPos)
	{
		//start coroutine
		StartCoroutine(findPath (startPos, endPos));
	}

	//the coroutine to find a path for the calling unit
	IEnumerator findPath(Vector3 startPos, Vector3 endPos)
	{
		//array of vector 3s that represents a path
		Vector3[] waypoints = new Vector3[0];
		//boolean used by unit to determine if end goal was reached
		bool success = false;

		//start node
		Node startNode = grid.positionToNode(startPos);
		//end node
		Node endNode = grid.positionToNode(endPos);

		//Heap used for sorting and finding neighbors - holds all nodes adjacent to visited nodes
		Heap<Node> openSet = new Heap<Node>(grid.maxSize);
		//hashset for all nodes already visited
		HashSet<Node> closedSet = new HashSet<Node>();
		//add the starting node to the open set
		openSet.add(startNode);

		//while there is a node in the open set
		while(openSet.count > 0)
		{
			//get the next node out of the heap
			Node currNode = openSet.removeFirst();
			//add current node to clsoed set
			closedSet.Add(currNode);

			//if endNode has been found
			if(currNode == endNode)
			{
				success = true; //end node is found
				break;			//break out of loop
			}

			//for each neighbor to the current node
			foreach(Node n in grid.getNeighbors(currNode))
			{
				//go to next neighbor if current is not walkable or already in closed set
				if(!n.walkable || closedSet.Contains(n))
					continue;

				//get cost to current neighbor
				int newToNeighbor = currNode.gCost + getDistance(currNode, n);
				//if the new cost is cheaper than the cost already on the neighbor node
				if(newToNeighbor < n.gCost || !openSet.contains(n))
				{
					//reset its values and parent node
					n.gCost = newToNeighbor;
					n.hCost = getDistance(n, endNode);
					n.parent = currNode;

					//add to open set if not already there
					if(!openSet.contains(n))
				   		openSet.add(n);
					else
						openSet.updateItem(n);
				}
			}
		}
		//return coroutine after loops end
		yield return null;

		//if the end is found
		if(success)
		{
			//call path building method
			waypoints = path(startNode, endNode);
		}
		//end path request processing
		requestManager.finishedPath(waypoints, success);

	}

	//build a path from start to end
	Vector3[] path(Node start, Node end)
	{
		//create list of nodes for path
		List<Node> path = new List<Node>();
		//starty at the end node
		Node currentNode = end;

		//while the currNode is not the starting point
		while(currentNode != start)
		{
			//add to path and move to next node
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}

		//add start
		path.Add(start);

		//call path simplifier method
		Vector3[] waypoints = simplfyPath(path);

		//reverse order
		Array.Reverse(waypoints);

		//return list
		return waypoints;
	}

	//takes out unnecessary nodes in the path
	Vector3[] simplfyPath(List<Node> path)
	{
		//list of nodes for path
		List<Vector3> waypoints = new List<Vector3>();
		//create a vector 2 to represent a current direction of movement
		Vector2 oldDirection = Vector2.zero;

		//loop through nodes in path
		for(int i=1; i<path.Count; i++)
		{
			//get new direction, equal to facing of unit
			Vector2 newDirection = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);

			//if the directions are not equal, add new waypoint to simple path
			if(newDirection != oldDirection)
			{
				waypoints.Add(path[i-1].worldPosition);
			}
			//reset direction to new direction
			oldDirection = newDirection;
		}
		//return new list as array
		return waypoints.ToArray();
	}

	//calculate distance between 2 nodes
	int getDistance(Node a, Node b)
	{
		int xDst = Mathf.Abs(a.gridX - b.gridX);
		int yDst = Mathf.Abs(a.gridY - b.gridY);

		//diagonals cost 14 while cardinal directions cost 10
		if(xDst > yDst)
			return 14*yDst + 10*(xDst-yDst);
		else
			return 14*xDst + 10*(yDst-xDst);
	}
}
