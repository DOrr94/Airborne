using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class PlanePilot : NetworkBehaviour {
    public float speed = 80.0f;
    public float speedModifier = 1.0f;

	public const int maxHealth = 500;
	public int currentHealth = maxHealth;

    public string powerUp;

    public GameObject booster, rocketBracket, NetHolder, netObj;
    public Transform boosterSpawn, rocketBracketSpawnRight, rocketBracketSpawnLeft, netSpawn;

    
    public bool powerUpPickup = false;

    public bool power1used = false;
    public bool power2used = false;
    public bool power3used = false;
    public bool power4used = false;

    
    public AudioListener audioListener;

    //I need to make a GUI class that builds the entire UI.
	//Text magSizeText, reloadText, activePowerup;
	

    AudioSource audioReload, audioBullet;
    public AudioClip bulletSound;

    Texture powerUpImage, naIcon;

    public List<Power> activePowers = new List<Power>();

    GameObject thisPlane;
    
    // Use this for initialization
    void Start () {
        Debug.Log("plane pilot script added to: " + gameObject.name);

    //    magSizeText = GameObject.Find("MagSizeTextField").GetComponent<Text>();

    //    reloadText = GameObject.Find("ReloadWarning").GetComponent<Text>();
 //       reloadText.enabled = false;

  //      activePowerup = GameObject.Find("activePowerup").GetComponent<Text>();


        var aSources = GetComponents<AudioSource>();
        audioReload = aSources[0];
        audioBullet = aSources[1];

        naIcon = Resources.Load("Images/na.png") as Texture; 

        Power p1 = new Power("", naIcon);
        Power p2 = new Power("", naIcon);
        Power p3 = new Power("", naIcon);
        Power p4 = new Power("", naIcon);


        activePowers.Add(p1);
        activePowers.Add(p2);
        activePowers.Add(p3);
        activePowers.Add(p4);


        thisPlane = this.gameObject;
    }



    // Update is called once per frame
    void Update () {

        if (!isLocalPlayer)
        {
            return;
        }

        transform.position += transform.forward * Time.deltaTime * speed * speedModifier;

        speed -= transform.forward.y * Time.deltaTime * 50.0f;

        //Check if speed drops below 35, we can play a stalling sound here.
        if(speed < 35.0f)
        {
            speed = speed - Time.deltaTime;

            if (speed < 0.0f)
            {
                speed = 0.0f;
            }

            transform.Rotate(Time.deltaTime * 10, 0.0f, 0.0f);
        }

        if(speed > 100.0f && speedModifier == 1.0f)
        {
            speed = 100.0f;
        }
        else if (speed > 150.0f && speedModifier > 1)
        {
            speed = 150.0f;
        }

        transform.Rotate( Input.GetAxis("Vertical"), 0.0f, -Input.GetAxis("Horizontal") * 2.0f);
        
        if (Input.GetButton("YawLeft")){
            transform.Rotate(0.0f, Time.deltaTime * -20, 0.0f);
        }
        if (Input.GetButton("YawRight"))
        {
            transform.Rotate(0.0f, Time.deltaTime * 20, 0.0f);
        }



        

        if (Input.GetButton("Ability1") && power1used == false)
        {
            if (activePowers.Count > 0)
            {
                usePower(activePowers[0].title, 0);
                
                
            }
        }

        if (Input.GetButton("Ability2") && power2used == false)
        {
            if (activePowers.Count > 1)
            {
                usePower(activePowers[1].title, 1);

                
            }
        }

        if (Input.GetButton("Ability3") && power3used == false)
        {
            if (activePowers.Count > 2)
            {
                usePower(activePowers[2].title, 2);

                
            }
        }

        if (Input.GetButton("Ability4") && power4used == false)
        {
            if (activePowers.Count > 3)
            {
                usePower(activePowers[3].title, 3);

                
            }
        }

        //Checks if plane goes underground, and prevents it. Can change to make it explode/respawn.
        float terrainHeightCurrent = Terrain.activeTerrain.SampleHeight(transform.position);

        if (terrainHeightCurrent > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x,
                                             terrainHeightCurrent,
                                             transform.position.z);
        }




	}

    //Detects if a trigger was hit, this will trigger a powerup to be added to the plane.
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "power")
        {
            bool alreadyActive = false;

            int indexFirstEmpty = -1;

            string newPowerUpTitle = other.GetComponent<PowerUp>().title;
            Texture newPowerUpTexture = other.GetComponent<PowerUp>().icon;

            if (powerUpPickup == false)
            {
               
               foreach (Power pwr in activePowers)
                    {
                        if (pwr.title == newPowerUpTitle)
                        {
                            alreadyActive = true;
                        }
                        if (pwr.title == "")
                        {
                            indexFirstEmpty = activePowers.IndexOf(pwr);
                        break;
                        }
                    }

                if (alreadyActive == false && indexFirstEmpty > 0)
                {
                    powerUpPickup = true;

                    activePowers[indexFirstEmpty].title = newPowerUpTitle;
                    activePowers[indexFirstEmpty].icon = newPowerUpTexture;

                    equipPower(newPowerUpTitle);


                    if (indexFirstEmpty == 1)
                    {
                        power2used = false;
                    }
                    else if (indexFirstEmpty == 2)
                    {
                        power3used = false;
                    }
                    else if (indexFirstEmpty == 3)
                    {
                        power2used = false;
                    }
                }

                else if (alreadyActive == false && indexFirstEmpty == 0)
                {
                    powerUpPickup = true;

                    activePowers[0].title = newPowerUpTitle;
                    activePowers[0].icon = newPowerUpTexture;

                    equipPower(newPowerUpTitle);


                    power1used = false;
                }
            }
            
            Invoke("notPickingUp", 3.0f);
        }


        updateIcons();


    }

    //Reloads weapon, should probably take argument of which weapon it is to properly reload 
    
    void notPickingUp()
    {
        powerUpPickup = false;
        
    }

    public void usePower(string power, int index)
    {
        if (index == 0)
        {
            power1used = true;
            GameObject.Find("PU_1").GetComponent<RawImage>().texture = naIcon;
        }
        else if (index == 1)
        {
            power2used = true;
            GameObject.Find("PU_2").GetComponent<RawImage>().texture = naIcon;
        }
        else if (index == 2)
        {
            power3used = true;
            GameObject.Find("PU_3").GetComponent<RawImage>().texture = naIcon;
        }
        else if (index == 3)
        {
            power4used = true;
            GameObject.Find("PU_4").GetComponent<RawImage>().texture = naIcon;
        }

        print("using power " + power);

        if (power == "Speed")
        {
            speedModifier = 2;
            powerUp = "Speed 2x";
       //     activePowerup.text = "Speed";
            Invoke("ClearPower", 25);

            var booster = GameObject.FindGameObjectWithTag("Booster");
            booster.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            Destroy(booster, 25);
        }

        if (power == "Laser")
        {
            powerUp = "Laser";
        //    activePowerup.text = "Laser";
            Invoke("ClearPower", 10);
        }

        if (power == "Rocket")
        {
            var rockets = GameObject.FindGameObjectsWithTag("Rocket");

            foreach(GameObject go in rockets)
            {
                go.transform.parent = null;
                go.GetComponent<Rigidbody>().isKinematic = false;
                go.GetComponent<ConstantForce>().relativeForce = new Vector3( 0, 0, 1000);
            }
        }

        if(power == "Net")
        {
            var net = GameObject.FindGameObjectWithTag("NetHolder");
            Destroy(net);

            Instantiate(this.netObj, netSpawn.position, Quaternion.identity);
            
        }

        activePowers[index].title = "";
        activePowers[index].icon = naIcon;
        
    }
    
    //Function is called to clear powerup from plane.
    void ClearPower()
    {
      //  activePowerup.text = "None";
        if (powerUp == "Speed 2x")
        {
            speedModifier = 1.0f;
        }

        else if(powerUp == "Laser")
        {
            GameObject go = GameObject.Find("LaserPointer(Clone)");
            LaserScript other = (LaserScript)go.GetComponent(typeof(LaserScript));
            other.usedLaser();
        }

        powerUp = "";
    }

    void updateIcons()
    {
        GameObject.Find("PU_1").GetComponent<RawImage>().texture = activePowers[0].icon;
        GameObject.Find("PU_2").GetComponent<RawImage>().texture = activePowers[1].icon;
        GameObject.Find("PU_3").GetComponent<RawImage>().texture = activePowers[2].icon;
        GameObject.Find("PU_4").GetComponent<RawImage>().texture = activePowers[3].icon;

    }

    void equipPower(string newPowerUpTitle)
    {
      /*  if (newPowerUpTitle == "Laser")
        {
            var laserPoint = Instantiate(this.laserPoint, LaserPointSpawn.position, LaserPointSpawn.rotation);
            laserPoint.transform.parent = thisPlane.transform;
        }
*/
        if (newPowerUpTitle == "Speed")
        {
            var boosterObj = Instantiate(this.booster, boosterSpawn.position, boosterSpawn.rotation);
            boosterObj.transform.parent = thisPlane.transform;

        }
        if (newPowerUpTitle == "Rocket")
        {
            var rightRocket = Instantiate(this.rocketBracket, rocketBracketSpawnRight.position, rocketBracketSpawnRight.rotation);
            rightRocket.transform.parent = thisPlane.transform;

            var leftRocket = Instantiate(this.rocketBracket, rocketBracketSpawnLeft.position, rocketBracketSpawnLeft.rotation);
            leftRocket.transform.parent = thisPlane.transform;

        }
        if (newPowerUpTitle == "Net")
        {
            var net = Instantiate(this.NetHolder, netSpawn.position, netSpawn.rotation);
            net.transform.parent = thisPlane.transform;
        }
    }

	public void takeDamage(int dmg){

		currentHealth -= dmg;
        
		if (currentHealth < 0) {
			print ("DEAD");
		}
	}
}