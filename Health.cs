using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

	public const int maxHealth = 500;

	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;

	public RectTransform healthBar;

	GameObject thisPlane;

	Text healthText;

	// Use this for initialization
	void Start () {
		thisPlane = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void takeDamage(int dmg){

		if (!isServer) {
			return;
		}

		currentHealth -= dmg;

		healthText.text = currentHealth.ToString ();

		if (currentHealth < 0) {

			currentHealth = maxHealth;
			RpcRespawn();

		}


	}

	void OnChangeHealth (int health)
	{
		healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
	}

	[ClientRpc]
	void RpcRespawn()
	{
		if (isLocalPlayer)
		{
			// move back to zero location
			transform.position = Vector3.zero;
		}
	}
}
