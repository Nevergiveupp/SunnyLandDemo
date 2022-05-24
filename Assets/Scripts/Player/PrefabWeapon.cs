using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabWeapon : MonoBehaviour {

	public Transform firePoint;
	public GameObject bulletPrefab;
	public PlayerController pc;
	
	// Update is called once per frame
	void Update () {
		Debug.Log("isOnFire=" + pc.isOnFire);
		if (pc.isOnFire && Input.GetButtonDown("Fire1"))
		{
			Shoot();
		}
	}

	public void Shoot ()
	{
		SoundManager.instance.FireAudio();
		Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
	}
}
