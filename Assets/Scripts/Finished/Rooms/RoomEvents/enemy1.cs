using UnityEngine;
using System.Collections;

public class enemy1 : MonoBehaviour {
	
	public GameObject spawn1;
	public GameObject enemy;
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
	
			Instantiate(enemy, spawn1.transform.position, Quaternion.identity);
			roomControl.spawnedEnemies = 1;
			Destroy (gameObject);
		}
	}
}
