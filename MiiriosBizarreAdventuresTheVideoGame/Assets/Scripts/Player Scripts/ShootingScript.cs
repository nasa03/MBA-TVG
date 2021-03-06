﻿/*
 *  Script used to shoot. draws ray from player to cross hair, first collider that is hit is returned
 *  if it's shootable it is shot, otherwise nothing happens
 */

using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public Animator gunanimator;
    public AudioSource AS;
    public AudioClip GunShotSFX;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            gunanimator.Play("Shoot");
            AS.PlayOneShot(GunShotSFX);
            BulletScript();
        }


    }

    public void BulletScript()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward * 100000, out hit))
        {
            Shootable ObjectThatWasShot = hit.collider.GetComponent<Shootable>();

            if (ObjectThatWasShot != null)
            {
                ObjectThatWasShot.GetShot();
            }
        }
    }
}
