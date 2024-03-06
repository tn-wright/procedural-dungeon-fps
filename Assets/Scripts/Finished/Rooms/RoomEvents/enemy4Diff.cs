using UnityEngine;
using System.Collections;

public class enemy4Diff : MonoBehaviour {

	public GameObject spawn1;
	public GameObject spawn2;
	public GameObject spawn3;
	public GameObject spawn4;
	public GameObject enemy1;
	public GameObject enemy2;
	public GameObject enemy3;
	public GameObject ammoSpawn;
	public GameObject ammo;
	public GameObject healthSpawn;
	public GameObject health;
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
			
			Instantiate(enemy1, spawn1.transform.position, Quaternion.identity);
			Instantiate(enemy1, spawn2.transform.position, Quaternion.identity);
			Instantiate(enemy2, spawn3.transform.position, Quaternion.identity);
			Instantiate(enemy3, spawn4.transform.position, Quaternion.identity);
			Instantiate(ammo, ammoSpawn.transform.position, Quaternion.identity);
			Instantiate(health, healthSpawn.transform.position, Quaternion.identity);
			roomControl.spawnedEnemies = 4;
			Destroy (gameObject);
		}
	}
}
