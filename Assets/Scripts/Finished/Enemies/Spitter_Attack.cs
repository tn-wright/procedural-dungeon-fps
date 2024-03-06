using UnityEngine;
using System.Collections;

public class Spitter_Attack : MonoBehaviour 
{
	public int dmgAmount;	//damage amount
	public float initTimer; //attack timer
	
	private GameObject player;	//player object
	private PlayerHealth playerHealth; //player health script
	private float dmgTimer; //damage timer
	public GameObject mouth; //spawn point for spitball
	public GameObject spitball; //spitball prefab
	private AudioSource spitSource; //spit audio source
	public AudioClip spitClip; //spit sound
	private Vector3 playPos; //player position
	
	void Awake()
	{
		//initialization of variables
		player = GameObject.FindGameObjectWithTag("Player");
		playerHealth = player.GetComponent<PlayerHealth>();
		spitSource = GetComponentInChildren<AudioSource>();
	}
	
	void OnTriggerStay (Collider col) 
	{
		if(!playerHealth.Dead)
		{
			//player is in trigger
			if(col.tag == "Player")
			{
				//raycast hit variable
				RaycastHit hit;

				//get player position
				playPos = new Vector3 (player.transform.position.x, 0f, player.transform.position.z);
				//look at player
				transform.LookAt(playPos);

				//raycast hists something
				if(Physics.Raycast(mouth.transform.position, transform.forward, out hit))
				{
					//hits player
					if(hit.collider.tag == "Player")
					{
						//decrement timer
						dmgTimer -= Time.deltaTime;

						//do damage
						if(dmgTimer <= 0f)
						{
							//spit and reset timer
							spit();
							dmgTimer = initTimer;
						}
					}
				}
			}
		}
	}

	//spit
	void spit()
	{
		//instantiate a spit ball
		GameObject spit = Instantiate(spitball, mouth.transform.position, mouth.transform.rotation) as GameObject;
		//apply force to shoot spitball
		spit.GetComponent<Rigidbody>().AddForce(mouth.transform.forward*200);
		//play spit sound
		spitSource.PlayOneShot(spitClip);
	}
}
