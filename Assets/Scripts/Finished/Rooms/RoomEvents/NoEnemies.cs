using UnityEngine;
using System.Collections;

public class NoEnemies : MonoBehaviour {

	public GameObject ammoSpawn1;
	public GameObject ammoSpawn2;
	public GameObject healthSpawn1;
	public GameObject healthSpawn2;
	
	public GameObject health;
	public GameObject ammo;
	// Use this for initialization
	
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter (Collider col)
	{
		if(col.tag == "Player")
		{
			Instantiate(ammo, ammoSpawn1.transform.position, Quaternion.identity);
			Instantiate(ammo, ammoSpawn2.transform.position, Quaternion.identity);
			Instantiate(health, healthSpawn1.transform.position, Quaternion.identity);
			Instantiate(health, healthSpawn2.transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}
}
