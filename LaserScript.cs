using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print("Laser online. Ready to fire.");
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void usedLaser()
    {
        Destroy(gameObject);
    }
}
