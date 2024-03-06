using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour 
{
	public int health = 100; //health amount
	public Color[] healthInd; //array of colors for unit
	private int initialHealth; //starting health
	private RoomController roomControl; //roomController script
	private GameObject controller; //controller object
	private GUIControl gui; //gui control script
	private GameObject player; //player gameObject
	private PlayerHealth playHealth; //playerHealth Script

	private Renderer AIBody; //renderer for a unit


	void Awake()
	{
		//initialization of variables
		controller = GameObject.FindGameObjectWithTag("GameController");
		roomControl = controller.GetComponent<RoomController>();
		AIBody = GetComponentInChildren<Renderer>();
		gui = controller.GetComponent<GUIControl>();
		initialHealth = health;
		player = GameObject.FindGameObjectWithTag("Player");
		playHealth = player.GetComponent<PlayerHealth>();
	}

	void Update()
	{
		//call health Indication
		HealthIndication();

		//if heath == 0
		if(health <= 0)
		{
			//if it is a sprinter
			if(gameObject.name == "Enemy_Sprinter(Clone)")
			{
				//reset latch
				playHealth.latch = false;
			}
			//deactivate
			gameObject.SetActive(false);
			//decrement spawned enemies
			roomControl.spawnedEnemies -= 1;
			//increment score
			gui.score += 1;
		}
	}

	//change color of unit based on health
	void HealthIndication()
	{
		//initial color
		if(health >= (initialHealth*.8))
		{
			AIBody.GetComponent<Renderer>().material.SetColor("_Color", healthInd[0]);
		}
		//60% to 80%
		if(health >= (initialHealth*.6) && health < (initialHealth*.8))
		{
			AIBody.GetComponent<Renderer>().material.SetColor("_Color", healthInd[1]);
		}
		//40% to 60%
		if(health >= (initialHealth*.4) && health < (initialHealth*.6))
		{
			AIBody.GetComponent<Renderer>().material.SetColor("_Color", healthInd[2]);
		}
		//20% to 40%
		if(health >= (initialHealth*.2) && health < (initialHealth*.4))
		{
			AIBody.GetComponent<Renderer>().material.SetColor("_Color", healthInd[3]);
		}
		//0% to 20%
		if(health >= 0 && health < (initialHealth*.2))
		{
			AIBody.GetComponent<Renderer>().material.SetColor("_Color", healthInd[4]);
		}
	}

	public void DamageTaken(int amount)
	{
		//lower health on function call
		health -= amount;
	}
}
