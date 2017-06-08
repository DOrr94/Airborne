using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FiringMechanism : NetworkBehaviour {

	public float magCap = 200.0f;
	public float magSize = 200.0f;

	public float fireRate = 0.5f;
	public float nextFire = 0.0f;
	public float laserBlast = 0.0f;

	public bool isReloading = false;

	public string powerUp;


	GameObject thisPlane;
	public GameObject Bullet, laserPoint, laserBeam;
	public Transform Gun, LaserPointSpawn;

	// Use this for initialization
	void Start () {
		thisPlane = this.gameObject;


	}
	
	// Update is called once per frame
	void Update () {

		if (!isLocalPlayer) {
			return;
		}

		if (Input.GetButton("Fire1") && Time.time > nextFire && isReloading == false)
		{

			//In this section I'll have to implement a check to see what weapon is currently attached to the plane. Maybe if a string matches or a number matches it'll shoot the correct weapon.
			if (powerUp == "Laser")
			{
				if (laserBlast > 300)
				{
					var beam = Instantiate(this.laserBeam, LaserPointSpawn.position, LaserPointSpawn.rotation);
					beam.transform.parent = thisPlane.transform;
					laserBlast = 0.0f;
					Destroy(beam, 0.5f);
				}

				laserBlast++;

			}

			else
			{
				//While mag isn't empty, shoot:
				if (magSize != 0.0f)
				{
					CmdFire ();

                }
				//If mag is empty, prompt reload:
				else if (magSize == 0.0f)
				{
					//Play click noise and display need to reload.
		//			reloadText.enabled = true;

				}
			}
		}

		if (Input.GetButton("Reload"))
		{
			isReloading = true;
		//	audioReload.Play();

			Invoke("Reload", 1.0f);

		//	reloadText.enabled = false;
		}





	}



	void Reload()
	{
		magSize = 200.0f;
		isReloading = false;

	}

    
    [Command]
    void CmdFire()
    {
        nextFire = Time.time + fireRate;
        var round = Instantiate(Bullet, Gun.position, Gun.rotation);

		Physics.IgnoreCollision (round.GetComponent<Collider>(), thisPlane.GetComponent<BoxCollider>());

        NetworkServer.Spawn(round);

		var speed = thisPlane.GetComponent<PlanePilot> ().speed;

		round.GetComponent<Rigidbody> ().velocity = round.transform.forward * (400 + speed);
		Destroy (round, 3.0f);
        magSize--;
    }
    
}
