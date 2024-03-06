using UnityEngine;
using System.Collections;

public class Sprinter_Movement : MonoBehaviour {

	//player object
	private GameObject player;
	//speed
	float speed = 6f;
	//path in array of vector3's
	Vector3[] path;
	//target
	int targetIndex;
	//boolean to determine if player is in radius to stop movement.
	bool inRadius;
	//playerHealth script
	PlayerHealth health;
	//bool to determine if unit is jumping
	bool jump = false;
	//bool to determine if unit can jump
	bool canJump = true;
	//bool to determine if unit is latched onto the player
	public bool isLatched = false;

	//offset for lookat player
	Vector3 offset = new Vector3(0,-1,0);
	//vector of player's position
	Vector3 playPos;

	void Start()
	{
		//get components
		player = GameObject.FindGameObjectWithTag("Player");
		health = player.GetComponent<PlayerHealth>();
	}
	
	//call pathfinding methods in a fixed update
	void FixedUpdate()
	{
		//lookat player if not latched
		if(!isLatched)
			transform.LookAt(player.transform.position+offset, Vector3.up);

		//if player is not dead and not latched
		if(!health.Dead && !isLatched)
		{	
			//not in radius and can jump, move closer to player
			if (!inRadius && canJump)
			{
				RequestManager.RequestPath(transform.position, player.transform.position, onPathFound);
			}

			//in radius and can jump
			if (inRadius && canJump)
			{
				//raycast hit object
				RaycastHit hit;

				//get player position
				playPos = new Vector3 (player.transform.position.x, 0f, player.transform.position.z);
				//look at player
				transform.LookAt(playPos);

				//if raycast hits
				if(Physics.Raycast(transform.position, transform.forward, out hit))
				{
					//if it hits the player
					if(hit.collider.tag == "Player")
					{
						//stop the path following
						StopCoroutine("followPath");
						jump = true; //toggle jump
					}
				}
			}
		}
		else //player must be dead - stop
		{
			StopCoroutine("followPath");
		}
	}

	void Update()
	{
		//jump
		if(jump)
		{
			//change position for jumping
			transform.LookAt(player.transform.position);
			Vector3 jumpDirection = transform.forward;
			Vector3 jumpForce = new Vector3 (500, 150, 500);
			Vector3 finalJump = new Vector3(jumpDirection.x*jumpForce.x, (jumpDirection.y+1)*jumpForce.y, jumpDirection.z*jumpForce.z);
			GetComponent<Rigidbody>().AddForce(finalJump);
			jump = false;
			canJump = false;
		}

		if(!health.Dead && isLatched)
		{
			health.latch = true;
			StopCoroutine("followPath");
			transform.position = health.face.transform.position;
			transform.rotation = health.face.transform.rotation;
			GetComponent<Rigidbody>().isKinematic = true;
		}
	}

	//toggle in radius
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			inRadius = true;
		}
	}

	//toggle in radius
	void OnTriggerExit(Collider col)
	{
		if(col.tag == "Player")
		{
			inRadius = false;
		}
	}

	//toggle canJump if collides with ground or isLatched if with player
	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Floor")
		{
			canJump = true;
		}
		if(col.gameObject.tag == "Player")
		{
			if(!health.latch)
			{
				isLatched = true;
				//rigidbody.isKinematic = true;
			}
		}
		
	}

	//if a path is found, set path, stop old, and follow new
	public void onPathFound(Vector3[] newPath, bool success)
	{
		if(success && gameObject.activeSelf)
		{
			if(gameObject.activeSelf)
				path = newPath;
			if(gameObject.activeSelf)
				StopCoroutine("followPath");
			if(gameObject.activeSelf)
				StartCoroutine("followPath");
		}
	}

	//follow path coroutine
	IEnumerator followPath()
	{
		//get a current node
		Vector3 currWaypoint = path[0];

		//loop until broken
		while(true)
		{
			//if at current waypoint
			if(transform.position == currWaypoint)
			{
				//move to next waypoint
				targetIndex++;
				if(targetIndex >= path.Length)
				{
					//break if end is reached
					targetIndex = 0;
					path = new Vector3[0];
					yield break;
				}
				currWaypoint = path[targetIndex];
			}
			transform.position = Vector3.MoveTowards(transform.position, currWaypoint, speed*Time.deltaTime);
			yield return null;
		}
	}
}
