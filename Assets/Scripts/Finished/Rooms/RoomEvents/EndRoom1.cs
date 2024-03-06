using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndRoom1 : MonoBehaviour {

	public bool victory;

	private Text victoryText;
	
	private float endTimer = 5f;
	
	// Use this for initialization
	void Start () 
	{
		victoryText = GameObject.FindGameObjectWithTag("victoryText").GetComponent<Text>();

		victoryText.enabled = false;

		print (victoryText.text);
		Debug.Log ("test");

		victory = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(victory)
		{
			victoryText.enabled = true;
			
			endTimer -= Time.deltaTime;
			
			if(endTimer <= 0)
			{
				Application.LoadLevel(0);
			}
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			victory = true;
		}
	}
}
