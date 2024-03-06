using UnityEngine;
using System.Collections;

public class RoomNode : IHeapItem<RoomNode>{

	public bool filled;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;
	public int gCost;
	public int hCost;

	public RoomNode parent;
	public int doorCount; 

	int heapIndex;

	public bool hasUp = false;
	public bool hasDown = false;
	public bool hasLeft = false;
	public bool hasRight = false;

	public bool isStart = false;
	public bool isEnd = false;

	public RoomNode(Vector3 pos, int x, int y)
	{
		filled = false;
		worldPosition = pos;
		gridX = x;
		gridY = y;
	}

	public int fCost
	{
		get{
			return hCost+gCost;
		}
	}

	public int HeapIndex
	{
		get{
			return heapIndex;
		}
		set{
			heapIndex = value;
		}
	}
	
	public int CompareTo(RoomNode node)
	{
		int compare = fCost.CompareTo(node.fCost);
		if(compare == 0)
		{
			compare = hCost.CompareTo(node.hCost);
		}
		return -compare;
	}
}
