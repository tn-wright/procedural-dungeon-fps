/********************************************************
 * NAME: Travis Wright									*
 * CLASS: 4373.003										*
 * ASSIGNMENT: Project 4								*
 * FILE: Grid.cs										*
 * DESCRIPTION: used to create the grid of pathfinding	*
 * 		nodes and then fill said grid. Also has methods	*
 * 		to use the grid.								*
 * ******************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour 
{
	public Transform player;	//player transform object
	public Vector2 gridSize;	//a vector 2 that determines grid size
	public float nodeSize;		//radius of a single node
	public LayerMask unwalkableMask;	//a layermask used to determine walkability

	//2d array for grid of nodes
	public Node[,] grid;

	//size in x and y direction
	int gridSizeX, gridSizeY;
	//diameter of a node
	float nodeDiameter;

	//initialization
	void Awake()
	{
		nodeDiameter = nodeSize*2;
		gridSizeX = Mathf.RoundToInt(gridSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridSize.y/nodeDiameter);

	}

	//find player's transform and update grid on fixed interval
	void FixedUpdate()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		createGrid ();
	}

	//maxsize variable
	public int maxSize
	{
		get{
			return gridSizeX*gridSizeY;
		}
	}

	//create the grid 
	void createGrid()
	{
		//build array
		grid = new Node[gridSizeX, gridSizeY];

		//loop through both x and y
		for (int x=0; x<gridSizeX; x++)
		{
			for (int y=0; y<gridSizeY; y++)
			{
				//get world x coordinate of current node
				int xCoord = Mathf.RoundToInt((gridSize.x/-2) + nodeSize + (nodeDiameter*x));
				//get world y coordinate of current node
				int yCoord = Mathf.RoundToInt((gridSize.y/-2) + nodeSize + (nodeDiameter*y));
				//the world position at those coordinates
				Vector3 worldPoint = new Vector3(xCoord, 0, yCoord);
				//check for collisions with node to determine walkability
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeSize, unwalkableMask));
				//add node to grid
				grid[x,y] = new Node(walkable, worldPoint, x, y);
			}
		}
	}

	//get a list of neighboring nodes
	public List<Node> getNeighbors(Node n)
	{
		//build list
		List<Node> neighbors = new List<Node>();

		//loop through all adjacent nodes
		for(int x = -1; x<=1; x++)
		{
			for(int y = -1; y<=1; y++)
			{
				//if its the current node, continue
				if(x==0 && y==0)
					continue;

				//get grid x and y of current node
				int checkX = n.gridX + x;
				int checkY = n.gridY + y;

				//verify current node is in the grid
				if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
					neighbors.Add (grid[checkX,checkY]);
			}
		}

		//return list
		return neighbors;
	}

	//convert a world position to a node
	public Node positionToNode(Vector3 worldPosition)
	{
		//get x and y
		int x = Mathf.RoundToInt((worldPosition.x - (gridSize.x/-2) - nodeSize) / nodeDiameter);
		int y = Mathf.RoundToInt((worldPosition.z - (gridSize.y/-2) - nodeSize) / nodeDiameter);

		//return the node
		return grid[x,y];
	}
}
