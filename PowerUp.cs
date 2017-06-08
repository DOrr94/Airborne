using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    
    public string title;
    public Texture icon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(0.0f, Time.deltaTime * 10.0f, 0.0f);

    }
}

public class Power
{
    public string title;
    public Texture icon;

    public Power(string str, Texture ico)
    {
        title = str;
        icon = ico;
    }
}