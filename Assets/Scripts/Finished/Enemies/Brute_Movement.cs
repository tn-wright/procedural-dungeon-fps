using UnityEngine;
using System.Collections;

public class Brute_Movement : MonoBehaviour {
	
	public const int RADIUS = 2;
	private GameObject player;
	float speed = 5.25f;
	Vector3[] path;
	int targetIndex;
	Grid grid;
	private GameObject controller;
	bool bX;
	bool bY;
	bool inRadius;
	PlayerHealth health;
	public bool shieldUp = false; 
	private float shieldTimer = 2.5f;
	
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		health = player.GetComponent<PlayerHealth>();
	}

	void Update()
	{
		if(shieldUp)
		{
			shieldTimer -= Time.deltaTime;

			if(shieldTimer <= 0)
			{
				shieldUp = false;
				shieldTimer = 3f;
			}
		}
	}
	
	
	void FixedUpdate()
	{
		print (inRadius);

		if(!health.Dead && !shieldUp)
		{
			if (!inRadius)
			{
				//StopCoroutine("followPath");
				RequestManager.RequestPath(transform.position, player.transform.position, onPathFound);
			}
			
			if (inRadius)
			{
				StopCoroutine("followPath");
			}
		}
		else
		{
			StopCoroutine("followPath");
		}
	}
	
	
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

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			inRadius = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if(col.tag == "Player")
		{
			inRadius = false;
		}
	}
}
