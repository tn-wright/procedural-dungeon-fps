using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIControl : MonoBehaviour 
{
	//text items
	public Text scoreGUI;
	public Text ammoGUI;
	public Text startText;

	//used for bullet indicators
	public Image[] bulletArray;
	private int currentBullet = 15;

	//health and shield sliders
	public Slider healthBar;
	public Slider shieldBar;

	//player game object
	private GameObject player;

	//playerhealth script
	private PlayerHealth playHealth;
	//gunshot script
	private GunShot gunShot;

	//score integer
	public int score = 0;

	//start text timer
	private float startTextTimer = 5f; 

	// Use this for initialization
	void Start () 
	{
		//initilize variables
		player = GameObject.FindGameObjectWithTag("Player");
		playHealth = player.GetComponent<PlayerHealth>();
		gunShot = player.GetComponentInChildren<GunShot>();

		//set slider values
		healthBar.value = 100;
		shieldBar.value = 100;

		//enable start text
		startText.enabled = true;
		 
		//enable bullet images
		for(int i=0; i<16; i++)
		{
			bulletArray[i].enabled = true;

		}
	}

	
	// Update is called once per frame
	void Update () 
	{
		//set score and ammo text on update
		scoreGUI.text = "Score: " + score;
		ammoGUI.text = "Ammo: " + gunShot.loadedAmmo + " | " + gunShot.ammo;

		//set sliders to current values
		healthBar.value = playHealth.health;
		shieldBar.value = playHealth.shield;

		//decrement starttexttimer
		if(startTextTimer > 0f)
		{
			startTextTimer -= Time.deltaTime;
		}

		//disable start text when timer is over
		if(startTextTimer <= 0f)
		{
			startText.enabled = false;
		}

	}

	//removes a single bullet Image when a single shot is fired
	public void removeBullet()
	{
		//get current bullet
		Image currBull = bulletArray[currentBullet];
		//disable it
		currBull.enabled = false;
		//change active bullet
		currentBullet--;
	}

	public void reloadBullets()
	{
		//check ammo
		if(gunShot.ammo + gunShot.loadedAmmo >= 16)
		{
			//reset all immages if enough ammo exists
			for(int i=0; i<16; i++)
				bulletArray[i].enabled = true;
			currentBullet = 15;
		}
		else
		{
			//reset only some if there is not enough ammo
			int start = gunShot.loadedAmmo;
			int end = gunShot.loadedAmmo + gunShot.ammo;
			for(int i=start; i<end; i++)
				bulletArray[i].enabled = true;
			currentBullet = end-1;
		}




	}
}
