using UnityEngine;
using System.Collections;

public class MeleeAndAmmo : MonoBehaviour {

	public GameObject spawn1;
	public GameObject spawn2;
	public GameObject ammoSpawn;

	public GameObject meleeEnemy;
	public GameObject ammo;
	private GameObject controller;

	private RoomController roomControl;
	// Use this for initialization

	void Start () {
		controller = GameObject.FindGameObjectWithTag("GameController");
		roomControl = controller.GetComponent<RoomController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col)
	{
		if(col.tag == "Player")
		{
			Instantiate(ammo, ammoSpawn.transform.position, Quaternion.identity);
			Instantiate(meleeEnemy, spawn1.transform.position, Quaternion.identity);
			Instantiate(meleeEnemy, spawn2.transform.position, Quaternion.identity);
			roomControl.spawnedEnemies = 2;
			Destroy (gameObject);
		}
	}
}
