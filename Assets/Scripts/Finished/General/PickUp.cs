using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour 
{
	//amount of ammo to restore
	private int ammo = 36;

	//player game object
	private GameObject player;
	//gun shot script
	private GunShot gunShotScript;

	//audio clip on pickup
	public AudioClip ammoPickUp;

	void Start ()
	{
		//initialize variable
		player = GameObject.FindGameObjectWithTag("Player");
		gunShotScript = player.GetComponentInChildren<GunShot>();
	}

	void Update()
	{
		//rotate objects
		transform.Rotate(Vector3.up);
	}

	void OnTriggerEnter(Collider col)
	{
		//if player collides
		if(col.gameObject == player)
		{
			//play sound and add ammo
			gunShotScript.ammo = gunShotScript.ammo + ammo;
			AudioSource.PlayClipAtPoint(ammoPickUp, transform.position);
			Destroy(gameObject);
		}
	}
}
