using UnityEngine;
using System.Collections;

public class Spitball : MonoBehaviour {

	//damage amount
	private float damageAmount = 2.5f;

	//gameobject for players
	GameObject player;

	//playerealth script
	PlayerHealth health;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		health = player.GetComponent<PlayerHealth>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//if collides with player
	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == "Player")
		{
			//deal damage
			health.poison(damageAmount);
		}

		//destroy spitball
		Destroy (gameObject);
	}
}
