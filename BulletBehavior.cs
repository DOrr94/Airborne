using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletBehavior : MonoBehaviour {
    
    // If the bullet collides with something
	void OnCollisionEnter(Collision collision)
	{
		//Display a hitmarker here.
		//print ("Hit!");

		var hit = collision.gameObject;
		var hp = hit.GetComponent<Health>();

		if (hp != null) {
			hp.takeDamage(25);
		}

	}
}