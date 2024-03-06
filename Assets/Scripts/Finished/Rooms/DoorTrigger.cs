using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour 
{
	private RoomController roomControl;

	private GameObject gameController;
	public GameObject door;

	private bool lerpDoor = false;
	
	Vector3 offset;

	// Use this for initialization
	void Start () 
	{
		gameController = GameObject.FindGameObjectWithTag("GameController");
		roomControl = gameController.GetComponent<RoomController>();
		offset = new Vector3(0, 3, 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(lerpDoor)
		{
			door.transform.position = Vector3.Lerp(door.transform.position, door.transform.position+offset, 5*Time.deltaTime);

			float fY = door.transform.position.y;
			if(fY >= 6)
			{
				lerpDoor = false;
			}
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player" && !roomControl.lockedDoors)
			lerpDoor = true;
	}
}
