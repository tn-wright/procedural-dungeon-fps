using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	//timer until shield breaks
	private float timer = 2.5f;
		
	// Update is called once per frame
	void Update () 
	{
		//decrement timer
		timer -= Time.deltaTime;

		if(timer <= 0)
		{
			//foreach child
			foreach (Transform child in transform) 
			{
				//delete it if it is a bullet hole clone
				if(child.name == "Spot(clone)")
				{
					Destroy(child.gameObject);
				}
			}

			//destroy shield
			Destroy(gameObject);
		}
	}
}
