using UnityEngine;
using System.Collections;

public class HealthPickUp : MonoBehaviour {

	//value of health restored
	private int health = 25;

	//player gameobject
	private GameObject player;
	//playerhealth script
	private PlayerHealth playHealth;

	//audio clip on pickup
	public AudioClip healthPickUp;

	// Use this for initialization
	void Start () {
		//initialize variables
		player = GameObject.FindGameObjectWithTag("Player");
		playHealth = player.GetComponent<PlayerHealth>();
	}
	
	// Update is called once per frame
	void Update () {
		//rotate object
		transform.Rotate(Vector3.up);
	}

	void OnTriggerEnter(Collider col)
	{
		//if player enters object and is hurt
		if(col.gameObject == player && playHealth.health < 100)
		{
			//play sound and restore health
			playHealth.health += health;
			AudioSource.PlayClipAtPoint(healthPickUp, transform.position);
			Destroy(gameObject);
		}
	}
}
