using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomGrid : MonoBehaviour {

	public bool displayGridGizmos;
	public Vector2 gridSize;
	public float nodeSize;

	public RoomNode[,] roomGrid;

	int gridSizeX, gridSizeY;
	float nodeDiameter;

	void Start()
	{
		createGrid();
	}

	public int maxSize
	{
		get{
			return gridSizeX*gridSizeY;
		}
	}

	public void createGrid()
	{

		//Debug.Log ("nodeSize: " + nodeSize);
		nodeDiameter = nodeSize*2;
		//Debug.Log ("nodeDiameter: " + nodeDiameter);
		gridSizeX = Mathf.RoundToInt(gridSize.x/nodeDiameter);
		//Debug.Log ("gridSizeX: " + gridSizeX);
		gridSizeY = Mathf.RoundToInt(gridSize.y/nodeDiameter);
		//Debug.Log ("gridSizeY: " + gridSizeY);

		//print (gridSizeX + " " + gridSizeY);
		roomGrid = new RoomNode[gridSizeX, gridSizeY];
		

		
		for (int x=0; x<gridSizeX; x++)
		{
			for (int y=0; y<gridSizeY; y++)
			{
				int xCoord = (x*30) + ((gridSizeX/2)*(-30));
				int yCoord = (y*30) + ((gridSizeY/2)*(-30));
				Vector3 worldPoint = new Vector3(xCoord, 0, yCoord);
				roomGrid[x,y] = new RoomNode(worldPoint, x, y);
				//Debug.Log ("Node: " + x + ", " + y + ": " + roomGrid[x,y]);
			}
		}
	}

	public List<RoomNode> getNeighbors(RoomNode n)
	{
		List<RoomNode> neighbors = new List<RoomNode>();

		if(n.hasUp)
			neighbors.Add(roomGrid[n.gridX, n.gridY+1]);
		if(n.hasDown)
			neighbors.Add(roomGrid[n.gridX, n.gridY-1]);
		if(n.hasLeft)
			neighbors.Add(roomGrid[n.gridX-1, n.gridY]);
		if(n.hasRight)
			neighbors.Add(roomGrid[n.gridX+1, n.gridY]);
			
		
		return neighbors;
	}

	public RoomNode positionToNode(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x + gridSize.x/2) / gridSize.x;
		float percentY = (worldPosition.z + gridSize.y/2) / gridSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);
		
		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		
		return roomGrid[x,y];
	}


	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x*(nodeSize*2), 1, gridSize.y*(nodeSize*2)));
		
		if(roomGrid != null && displayGridGizmos)
		{
			foreach (RoomNode n in roomGrid)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
			}
		}
	}
}
