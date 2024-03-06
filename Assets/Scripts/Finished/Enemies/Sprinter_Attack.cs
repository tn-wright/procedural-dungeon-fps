using UnityEngine;
using System.Collections;

public class Sprinter_Attack : MonoBehaviour 
{
	public int dmgAmount; //damage amount
	public float initTimer; //attack timer
	public AudioClip PunchSound; //sound
	
	private GameObject player; //player object
	private PlayerHealth playerHealth; //player health script
	private AudioSource audSrc; //audio source
	private float dmgTimer; //attack timer
	private Sprinter_Movement move; //movement script
	
	void Awake()
	{
		//initialize variable
		player = GameObject.FindGameObjectWithTag("Player");
		playerHealth = player.GetComponent<PlayerHealth>();
		audSrc = GetComponent<AudioSource>();
		move = GetComponent<Sprinter_Movement>();
		
		dmgTimer = initTimer;
	}
	
	void OnTriggerStay (Collider col) 
	{
		if(!playerHealth.Dead && move.isLatched)
		{
			//if player is in trigger and enemy is latched onto the player
			if(col.tag == "Player")
			{
				//decrement timer
				dmgTimer -= Time.deltaTime;

				//do damage and play sound
				if(dmgTimer <= 0f)
				{
					playerHealth.TakeDmg(dmgAmount);
					audSrc.clip = PunchSound;
					audSrc.Play();
					dmgTimer = initTimer;
				}
				
			}
		}
	}
}
