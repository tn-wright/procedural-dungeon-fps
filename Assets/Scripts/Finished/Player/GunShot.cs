using UnityEngine;
using System.Collections;

public class GunShot : MonoBehaviour 
{
	#region GameObject Variables
	public GameObject spot;
	#endregion

	private float reloadTimer = 0;

	#region Integer Variables
	public int ammo = 0;
	public int loadedAmmo = 16;
	public int damageAmount = 10;
	public const int CLIP_SIZE = 16;
	#endregion 

	#region Audio Clips
	public AudioClip Gunshot;
	public AudioClip Dryfire;
	public AudioClip Reload;
	#endregion

	private AudioSource audSrc;

	private GameObject controller;
	private GUIControl gui;

	void Start()
	{
		controller = GameObject.FindGameObjectWithTag("GameController");
		audSrc = GetComponent<AudioSource>();
		gui = controller.GetComponent<GUIControl>();
	}

	// Update is called once per frame
	void Update () 
	{
		#region RaycastHit variable
		RaycastHit hit;
		#endregion

		if(Input.GetKeyDown (KeyCode.R))
		{
			reload();
		}

		if(reloadTimer > 0)
			reloadTimer -= Time.deltaTime;

		#region Click to Shoot
		//Get mouse input
		if(Input.GetMouseButtonDown(0) && reloadTimer <= 0)
		{
			//Check for ammo
			if(loadedAmmo > 0)
			{
				//If ammo is available then use Raycast
				if(Physics.Raycast(transform.position, transform.forward, out hit))
				{
					if(hit.collider.tag == "Enemy")
					{
						EnemyHealth eneHealth = hit.transform.gameObject.GetComponent<EnemyHealth>();
						eneHealth.DamageTaken(damageAmount);
					}

					//Play a sound for the gunshot
					//audSrc.clip = Gunshot;
					audSrc.PlayOneShot(Gunshot);
					
					//Instantiate a spot at the point of collision
					GameObject bulletHole = Instantiate(spot, hit.point, Quaternion.FromToRotation(Vector3.forward * -1, hit.normal)) as GameObject;
					bulletHole.transform.parent = hit.transform;


					//Remove ammo for shot
					loadedAmmo--;
					gui.removeBullet();
				}
			}

			else
			{
				//Tell the player that there is no ammo
				Debug.Log("No ammo!");

				//Play a sound for dry fire
				//audSrc.clip = Dryfire;
				audSrc.PlayOneShot(Dryfire);
			}
		}
		#endregion
	}

	void reload()
	{
		//only reload if there is ammo and the clip is partly empty
		if(ammo > 0 && loadedAmmo < CLIP_SIZE)
		{
			//call UI reload function to update
			gui.reloadBullets();

			//start timer
			reloadTimer = .7f;

			//play sound
			audSrc.PlayOneShot (Reload);

			int deficit = CLIP_SIZE-loadedAmmo;

			//determine if there is enough ammo to fill clip or not
			if(ammo >= deficit)
			{
				ammo -= deficit;
				loadedAmmo = CLIP_SIZE;
			}
			else if (ammo < deficit)
			{
				loadedAmmo += ammo;
				ammo = 0;
			}
		}
	}
}
