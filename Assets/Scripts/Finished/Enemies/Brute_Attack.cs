using UnityEngine;
using System.Collections;

public class Brute_Attack : MonoBehaviour 
{
	public int dmgAmount = 5; //damage amount for attack
	public float initTimer; //timer between attacks
	public AudioClip PunchSound; //sound
	
	private GameObject player; //player object
	private PlayerHealth playerHealth; //health script
	private AudioSource audSrc; //audio source
	private float dmgTimer; //dmg timer
	Vector3 offset = new Vector3(0, -1, 0); //offset for lookat for player

	public GameObject shield; //shield prefab
	public float shieldTimer = 5f; //timer for shield
	public GameObject shieldPoint; //point for shield spawn

	private Brute_Movement move; //movement script
	
	void Awake()
	{
		//initialize variables
		player = GameObject.FindGameObjectWithTag("Player");
		playerHealth = player.GetComponent<PlayerHealth>();
		audSrc = GetComponent<AudioSource>();
		move = GetComponent<Brute_Movement>();
		
		dmgTimer = initTimer;
	}

	void Update()
	{
		//player not dead
		if(!playerHealth.Dead)
		{
			//no shield is up
			if(!move.shieldUp)
			{
				//look at player
				Vector3 lookPos = new Vector3(player.transform.position.x, player.transform.position.y - 1f, player.transform.position.z);
				transform.LookAt(lookPos, Vector3.up);
			}

			//decrement shield timer
			shieldTimer -= Time.deltaTime;

			//if shield timer <= 0 and one is not up
			if(shieldTimer <= 0 && !move.shieldUp)
			{
				//spawn shield
				move.shieldUp = true;
				Instantiate(shield, shieldPoint.transform.position, shieldPoint.transform.rotation);
				//shieldObject.transform.parent = transform;
				shieldTimer = 10f;
			}
		}
	}

	//if player stays in radius of trigger
	void OnTriggerStay (Collider col) 
	{
		//player not dead and no shield
		if(!playerHealth.Dead && !move.shieldUp)
		{
			//player in trigger
			if(col.tag == "Player")
			{
				//look at player
				transform.LookAt(player.transform.position+offset, Vector3.up);

				//decrement timers
				dmgTimer -= Time.deltaTime;
				shieldTimer -= Time.deltaTime;

				//do damage
				if(dmgTimer <= 0f)
				{
					//player takes damage and play punch sound and reset timer
					playerHealth.TakeDmg(dmgAmount);
					audSrc.clip = PunchSound;
					audSrc.Play();
					dmgTimer = initTimer;
				}

				//spawn shield
				if(shieldTimer <= 0f)
				{
					Instantiate(shield, shieldPoint.transform.position, gameObject.transform.rotation);
				}
			}
		}
	}
}
