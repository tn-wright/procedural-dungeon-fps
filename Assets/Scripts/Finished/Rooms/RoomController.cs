using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomController : MonoBehaviour {

	RoomGrid roomGrid;
	//private Transform player;

	public GameObject[] roomArray;
	public GameObject[] eventArray;
	public GameObject playerSpawn;
	public GameObject playerPrefab;
	public GameObject endRoom;

	public int spawnedEnemies = 0;
	public bool lockedDoors = false;

	int xMax, yMax;

	//variables that mark the range for a set kind of room with in the room array
	//NOMENCALTURE: d# is the number of doors, followed by the door directions,
	//U = Up, D = Down, L = left, R = Right. The min and max represent first and last
	//element of that form within the array
	private int startIndex = 1;
	private int endIndex = 2;
	private int d1UMin = 3, d1UMax = 5;
	private int d1DMin = 6, d1DMax = 8;
	private int d1LMin = 9, d1LMax = 11;
	private int d1RMin = 12, d1RMax = 14;
	private int d2UDMin = 15, d2UDMax = 17;
	private int d2LRMin = 18, d2LRMax = 20;
	private int d2ULMin = 21, d2ULMax = 23;
	private int d2URMin = 24, d2URMax = 26;
	private int d2DRMin = 27, d2DRMax = 29;
	private int d2DLMin = 30, d2DLMax = 32;
	private int d3UDLMin = 33, d3UDLMax = 35;
	private int d3UDRMin = 36, d3UDRMax = 38;
	private int d3ULRMin = 39, d3ULRMax = 41;
	private int d3DLRMin = 42, d3DLRMax = 44;
	private int d4Min = 45, d4Max = 47;

	void Awake()
	{
		roomGrid = GetComponent<RoomGrid>();
		roomGrid.createGrid();
		xMax = Mathf.RoundToInt(roomGrid.gridSize.x/(roomGrid.nodeSize*2));
		yMax = Mathf.RoundToInt(roomGrid.gridSize.y/(roomGrid.nodeSize*2));
		formatGrid();
		fillGrid();
		Instantiate (playerPrefab, playerSpawn.transform.position, Quaternion.identity);
		//player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update()
	{
		if(spawnedEnemies > 0)
		{
			lockedDoors = true;
		}
		else
		{
			lockedDoors = false;
		}
	}
	

	void formatGrid()
	{
		roomGrid.roomGrid[0,0].hasUp = true;
		roomGrid.roomGrid[0,1].hasDown = true;

		roomGrid.roomGrid[0,0].hasRight = true;
		roomGrid.roomGrid[1,0].hasLeft = true;

		roomGrid.roomGrid[xMax-1,yMax-1].hasLeft = true;
		roomGrid.roomGrid[xMax-2,yMax-1].hasRight = true;

		roomGrid.roomGrid[xMax-1,yMax-1].hasDown = true;
		roomGrid.roomGrid[xMax-1,yMax-2].hasUp = true; 


		bool canReachEnd = false;

		do
		{
			for(int x = 0; x < xMax; x++) //grid size dependent
			{
				for(int y = 0; y < yMax; y++) //grid size dependent
				{
					int max = 3;

					bool setLeft = true;
					bool setRight = true;
					bool setUp = true;
					bool setDown = true;

					if(x==0)
					{
						max -= 1;
						setLeft = false;
					}
					else if(x==xMax-1) //grid size dependent
					{
						max -= 1;
						setRight = false;
					}

					if(y==0)
					{
						max -= 1;
						setDown = false;
					}
					else if(y==yMax-1) //grid size dependent
					{
						max -= 1;
						setUp = false;
					}

					int rand = Random.Range(1, max+1);
					int toGen = rand;

					if(roomGrid.roomGrid[x,y].hasUp)
					{
						toGen -= 1;
						setUp = false;
					}
					if(roomGrid.roomGrid[x,y].hasDown)
					{
						toGen -= 1;
						setDown = false;
					}
					if(roomGrid.roomGrid[x,y].hasLeft)
					{
						toGen -= 1;
						setLeft = false;
					}
					if(roomGrid.roomGrid[x,y].hasRight)
					{
						toGen -= 1;
						setRight = false;
					}

					roomGrid.roomGrid[x,y].doorCount = rand;

					if(toGen < 0)
						roomGrid.roomGrid[x,y].doorCount -= toGen;

					if(toGen > 0)
					{
						int counter = toGen;
						
						int randomInt = Random.Range (1, max+1);
						
						while(counter > 0)
						{
							if(randomInt == 1 && setUp)
							{
								roomGrid.roomGrid[x,y].hasUp = true;
								roomGrid.roomGrid[x,y+1].hasDown = true;
								setUp = false;
								counter--;
							}
							else if(randomInt == 2 && setDown)
							{
								roomGrid.roomGrid[x,y].hasDown = true;
								roomGrid.roomGrid[x,y-1].hasUp = true;
								setDown = false;
								counter--;							
							}
							else if(randomInt == 3 && setLeft)
							{
								roomGrid.roomGrid[x,y].hasLeft = true;
								roomGrid.roomGrid[x-1,y].hasRight = true;
								setLeft = false;
								counter--;							
							}
							else if(randomInt == 4 && setRight)
							{
								roomGrid.roomGrid[x,y].hasRight = true;
								roomGrid.roomGrid[x+1,y].hasLeft = true;
								setRight = false;
								counter--;							
							}
							
							randomInt = Random.Range (1,5);
						}
					}
				}
			}

			canReachEnd = findPathToEnd(roomGrid.roomGrid[0,0]);

		}while(!canReachEnd);
	}

	void fillGrid()
	{
		for(int x = 0; x < xMax; x++) //grid size
		{
			for(int y = 0; y < yMax; y++) //grid size
			{
				RoomNode currNode = roomGrid.roomGrid[x,y];
				int randomIndex = 1;
				int eventIndex = 1;

				//so many ifs!
				if(x==0 && y==0)
				{
					fillNode (roomArray[startIndex], currNode, eventArray[eventIndex] , randomIndex);
				}
				else if(x==xMax-1 && y==yMax-1)
				{
					fillNode (roomArray[endIndex], currNode, endRoom, randomIndex);
				}
				//Room has all four doors
				else if(currNode.hasUp && currNode.hasDown && currNode.hasLeft && currNode.hasRight)
				{
					randomIndex = Random.Range(d4Min, d4Max+1);
					eventIndex = Random.Range (0, eventArray.Length);
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
				//room lacks only right door
				else if(currNode.hasUp && currNode.hasDown && currNode.hasLeft && !currNode.hasRight)
				{
					randomIndex = Random.Range(d3UDLMin, d3UDLMax+1);
					eventIndex = Random.Range (0, eventArray.Length);
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
				//room lacks only left door
				else if(currNode.hasUp && currNode.hasDown && !currNode.hasLeft && currNode.hasRight)
				{
					randomIndex = Random.Range(d3UDRMin, d3UDRMax+1);
					eventIndex = Random.Range (0, eventArray.Length);
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
				//room lacks only bottom door
				else if(currNode.hasUp && !currNode.hasDown && currNode.hasLeft && currNode.hasRight)
				{
					randomIndex = Random.Range(d3ULRMin, d3ULRMax+1);
					eventIndex = Random.Range (0, eventArray.Length);
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
				//room lacks only top door
				else if(!currNode.hasUp && currNode.hasDown && currNode.hasLeft && currNode.hasRight)
				{
					randomIndex = Random.Range(d3DLRMin, d3DLRMax+1);
					eventIndex = Random.Range (0, eventArray.Length);
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
				//node has up and down
				else if(currNode.hasUp && currNode.hasDown && !currNode.hasLeft && !currNode.hasRight)
				{
					randomIndex = Random.Range(d2UDMin, d2UDMax+1);
					eventIndex = Random.Range (0, eventArray.Length);
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
				//has up and left
				else if(currNode.hasUp && !currNode.hasDown && currNode.hasLeft && !currNode.hasRight)
				{
					randomIndex = Random.Range(d2ULMin, d2ULMax+1);
					eventIndex = Random.Range (0, eventArray.Length);
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
				//has down and left
				else if(!currNode.hasUp && currNode.hasDown && currNode.hasLeft && !currNode.hasRight)
				{
					randomIndex = Random.Range(d2DLMin, d2DLMax+1);
					eventIndex = Random.Range (0, eventArray.Length);
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
				//has up and right
				else if(currNode.hasUp && !currNode.hasDown && !currNode.hasLeft && currNode.hasRight)
				{
					randomIndex = Random.Range(d2URMin, d2URMax+1);
					eventIndex = Random.Range (0, eventArray.Length);
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
				//has down and right
				else if(!currNode.hasUp && currNode.hasDown && !currNode.hasLeft && currNode.hasRight)
				{
					randomIndex = Random.Range(d2DRMin, d2DRMax+1);
					eventIndex = Random.Range (0, eventArray.Length);
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
				//has left and right
				else if(!currNode.hasUp && !currNode.hasDown && currNode.hasLeft && currNode.hasRight)
				{
					randomIndex = Random.Range(d2LRMin, d2LRMax+1);
					eventIndex = Random.Range (0, eventArray.Length);
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
				//only has up
				else if(currNode.hasUp && !currNode.hasDown && !currNode.hasLeft && !currNode.hasRight)
				{
					randomIndex = Random.Range(d1UMin, d1UMax+1);
					eventIndex = Random.Range (0, eventArray.Length);
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
				//only has down
				else if(!currNode.hasUp && currNode.hasDown && !currNode.hasLeft && !currNode.hasRight)
				{
					randomIndex = Random.Range(d1DMin, d1DMax+1);
					eventIndex = Random.Range (0, eventArray.Length);
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
				//only has left
				else if(!currNode.hasUp && !currNode.hasDown && currNode.hasLeft && !currNode.hasRight)
				{
					randomIndex = Random.Range(d1LMin, d1LMax+1);
					eventIndex = Random.Range (0, eventArray.Length);
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
				//only has right
				else if(!currNode.hasUp && !currNode.hasDown && !currNode.hasLeft && currNode.hasRight)
				{
					randomIndex = Random.Range(d1RMin, d1RMax+1);
					eventIndex = Random.Range (0, eventArray.Length);
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
				//should never happen, but for error - 0 doors or some other combonation not found above
				else
				{
					fillNode(roomArray[randomIndex], currNode, eventArray[eventIndex], randomIndex);
				}
			}
		}
	}

	bool findPathToEnd(RoomNode n)
	{
		bool success = false;

		List<RoomNode> openSet = new List<RoomNode>();
		HashSet<RoomNode> closedSet = new HashSet<RoomNode>();
		openSet.Add(n);

		while(openSet.Count > 0)
		{
			RoomNode currNode = openSet[0];
			for(int i = 1; i < openSet.Count; i++)
			{
				if(openSet[i].fCost < currNode.fCost || openSet[i].fCost == currNode.fCost && openSet[i].hCost < currNode.hCost)
				{
					currNode = openSet[i];
				}
			}

			openSet.Remove (currNode);
			closedSet.Add(currNode);

			if(currNode == roomGrid.roomGrid[xMax-1,yMax-1]) //grid size dependent
				return true;

			foreach(RoomNode neighbor in roomGrid.getNeighbors(currNode))
			{
				if(closedSet.Contains(neighbor))
					continue;

				int newMovementCostToNeighbour = currNode.gCost + getDistance(currNode, neighbor);

				if (newMovementCostToNeighbour < neighbor.gCost || !openSet.Contains(neighbor)) 
				{
					neighbor.gCost = newMovementCostToNeighbour;
					neighbor.hCost = getDistance(neighbor, roomGrid.roomGrid[xMax-1,yMax-1]); //grid size dependent
					neighbor.parent = currNode;
					
					if (!openSet.Contains(neighbor))
						openSet.Add(neighbor);
				}
			}
		}



		return success;
	}

	int getDistance(RoomNode a, RoomNode b)
	{
		int dstX = Mathf.Abs(a.gridX - b.gridX);
		int dstY = Mathf.Abs(a.gridY - b.gridY);

		return (10*dstY + 10*dstX);
	}

	public void fillNode(GameObject room, RoomNode node, GameObject event1, int roomIndex)
	{
		Instantiate(room, node.worldPosition, Quaternion.identity);

		if(node != roomGrid.roomGrid[0,0] && roomIndex%3 != 0)
		{
			Instantiate(event1, node.worldPosition, Quaternion.identity);
		}
	}
}
