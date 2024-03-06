using UnityEngine;
using System.Collections;

public class BulletHole : MonoBehaviour 
{
	void Update()
	{
		//destroy object after some time
		Destroy(gameObject, 10f);
	}
}
