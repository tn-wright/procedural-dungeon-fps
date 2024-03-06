using UnityEngine;
using System.Collections;

public class Spitter_Movement : MonoBehaviour {


	public const int RADIUS = 2; //radius
	private GameObject player; //game object for player
	float speed = 6f; //movement speed
	Vector3[] path; //array of nodes for path
	int targetIndex; //next waypoint in path array
	PlayerHealth health; //player health script
	private bool inRange; //player is in range
	private Vector3 playPos; //player position
	public GameObject mouth; //spawn point for spitball spawn
	
	void Start()
	{
		//initialize variables
		player = GameObject.FindGameObjectWithTag("Player");
		health = player.GetComponent<PlayerHealth>();
	}
	
	
	void FixedUpdate()
	{
		//raycsat hit object
		RaycastHit hit;

		//get player position
		playPos = new Vector3 (player.transform.position.x, 0f, player.transform.position.z);
		
		//if player is not dead and they are not in range
		if(!health.Dead && !inRange)
		{
			//look at player
			transform.LookAt(playPos, Vector3.up);

			if (!inRange)
			{
				//move towards player if not in range
				RequestManager.RequestPath(transform.position, player.transform.position, onPathFound);
			}
			
			if (inRange)
			{
				//stop moving if in range
				StopCoroutine("followPath");
			}
		}
		
		if(inRange)
		{
			//if raycast hits
			if(Physics.Raycast(mouth.transform.position, transform.forward, out hit))
			{
				//look at player
				transform.LookAt(playPos, Vector3.up);

				//raycast hits player
				if(hit.collider.tag == "Player")
				{
					//stop moving
					StopCoroutine("followPath");
				}
			}
		}
	}
	
	//start following a found path
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

	//follow path
	IEnumerator followPath()
	{
		Vector3 currWaypoint = path[0];
		
		while(true)
		{
			if(transform.position == currWaypoint)
			{
				targetIndex++;
				if(targetIndex >= path.Length)
				{
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
	
	void OnTriggerStay(Collider col)
	{
		if(col.tag == "Player")
		{
			inRange = true;
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		if(col.tag == "Player")
		{
			inRange = false;
		}
	}
}
