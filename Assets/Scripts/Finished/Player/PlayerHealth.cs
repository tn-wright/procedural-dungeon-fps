using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour 
{
	public const int STARTING_HEALTH = 100; //starting health amount
	public const int STARTING_SHIELD = 100; //starting shield amount
	public float health; //health amount
	public bool Dead; //boolean for death
	public int shield; //shield amount
	public GameObject face; //face gameobject used for sprinter enemies

	//ui
	public Color damageColor; //used for Ui flash
	public float flashTime = 3f; //timer for flash fade
	public Image damageFlashImage; //image that flashes
	public bool damaged; //damaged boolean to control flash

	private GunShot gunShot; //gunshot script
	private CharacterMotor charaMotor; //character motor
	private AudioSource audSrc; //audio source
	public AudioClip deathScream; //sound for death scream
	public AudioClip acidDamage; //acid damage sound
	private float timer = 5f; //timer
	private float shieldTimer = 4.75f; //shield timer
	 
	private float rechargeTimer; //shield recharge timer for each tick

	public bool dmgIndicator; //boolean for damage indicator
	public float indicatorTimer; //indicator timer

	bool poisoned = false; //boolean of poisoned
	float poisonAmount; //damage amount from poison
	float poisonTimer = 5f; //timer for poison
	float poisonTick = 0f; //timer for poison tick
	public Image poisonImage; //image for poison flash
	Color poisonColor; //color for poison flash
	float poisonFlashTime; //timer for flash time

	public bool latch = false; //latch boolean for sprinters

	void Awake()
	{
		//intialize variables
		damageFlashImage = GameObject.FindGameObjectWithTag("DamageFlash").GetComponent<Image>();
		gunShot = GetComponentInChildren<GunShot>();
		charaMotor = GetComponent<CharacterMotor>();
		audSrc = GetComponent<AudioSource>();
		health = STARTING_HEALTH;
		shield = STARTING_SHIELD;
		rechargeTimer = .25f;
		dmgIndicator = false;
		poisonImage = GameObject.FindGameObjectWithTag("PoisonFlash").GetComponent<Image>();
		poisonColor = poisonImage.color;
		poisonImage.color = Color.clear;
	}

	void Update()
	{
		//if the player is dead, fade screen to black
		if(Dead)
		{
			damageFlashImage.color = Color.Lerp (damageFlashImage.color, Color.black, 1*Time.deltaTime);

			timer -= Time.deltaTime;

			//reload level on death
			if(timer <= 0f)
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		}
		else //if player is alive, call damage flash 
		{
			damageFlash();
		}

		//if shield is damaged
		if(shield < STARTING_SHIELD)
		{
			//check shield timer
			if(shieldTimer > 0f)
			{
				//decrement if greater than 0
				shieldTimer -= Time.deltaTime;
			}

			//if less than 0
			if(shieldTimer <= 0f)
			{
				//check recharge timer
				if(rechargeTimer > 0f)
				{
					//decrement
					rechargeTimer -= Time.deltaTime;
				}

				//if less than 0
				if(rechargeTimer <= 0f)
				{
					//reset recharge timer
					rechargeTimer = .25f;

					//jincrement shield
					shield += 5;

					//stop when shield is full
					if(shield > STARTING_SHIELD)
						shield = STARTING_SHIELD;
				}
			}
		}

		//check if poisoned
		if(poisoned)
		{
			if(poisonTimer > 0f)
			{
				poisonTimer -= Time.deltaTime;

				//check poison tick timer
				if(poisonTick > 0f)
				{
					poisonTick -= Time.deltaTime;
				}
				
				if(poisonTick <= 0f)
				{
					//reset poison tick timer
					poisonTick = .5f;

					//lower health directly and flash poison color
					health -= poisonAmount;
					poisonImage.color = poisonColor;
					audSrc.PlayOneShot(acidDamage);
				}
			}
			else
			{
				poisoned = false;
				poisonTick = 0f;
				poisonTimer = 5f;
			}

			poisonImage.color = Color.Lerp (poisonImage.color, Color.clear, flashTime*Time.deltaTime);
		}

		//call death function if dead
		if(!Dead && health <= 0)
		{
			Death ();
		}
	}

	//called when an enemy does damage
	public void TakeDmg(int amount)
	{
		//set shield recharge timer
		shieldTimer = 5.0f;

		//toggle boolean
		damaged = true;

		//toggle damage indicator
		dmgIndicator = true;
		//indicator timer set
		indicatorTimer = 2.0f;

		//if shield is greater than 0
		if (shield > 0)
		{
			//if damage > shield, empty shield
			if(amount >= shield)
			{
				amount -= shield;
				shield = 0;
			}
			else //else subtract damage from shield
			{
				shield -= amount;
			}
		}
		//no shield means health hits 0
		if(shield == 0)
		{
			health -= amount;
		}

		//call death function if health is less than or equal to 0
		if(health <= 0 && !Dead)
		{
			Death();
		}
	}

	//screen flashes red on damage
	void damageFlash()
	{
		if(damaged)
		{
			damageFlashImage.color = damageColor;
		}
		else
		{
			damageFlashImage.color = Color.Lerp (damageFlashImage.color, Color.clear, flashTime*Time.deltaTime);
		}

		damaged = false;

	}

	//health reaches 0 or below
	void Death()
	{
		//toggle dead
		Dead = true;
		//play death scream
		audSrc.PlayOneShot(deathScream);

		//disable movement
		charaMotor.enabled = false;
		//disable shooting
		gunShot.enabled = false;
	}

	public void poison(float damageAmount)
	{
		//toggle poisoned boolean
		poisoned = true;
		//set the damage
		poisonAmount = damageAmount;
	}
}
