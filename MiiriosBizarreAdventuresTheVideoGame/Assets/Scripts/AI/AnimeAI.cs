﻿/*
 * Used for the weeb enemies in Level 4. Identical to GUV AI but with different voice lines and 
 * the distance at which the player can be detected due to the larger stage
 */

using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AnimeAI : MonoBehaviour, Shootable
{
    //State 
    private int HP = 3;
    private NavMeshAgent ThisGuv;
    private bool Dead;
    private bool FallOver = false;
    private AudioSource thisAudioSource;
    
    //Projectile 
    public int force = 100;
    public GameObject attack;

    //Audio
    private AudioClip[] AttackSFX;
    private AudioClip[] DeadSFX;
    private AudioClip[] HurtSFX;

    private BoxCollider[] colliders;
    //Cache
    private Vector3 playerlocation;

    private bool haslos = false;

    void Start()
    {
        thisAudioSource = gameObject.GetComponent<AudioSource>();
        ThisGuv = gameObject.GetComponent<NavMeshAgent>();

        colliders = GetComponentsInChildren<BoxCollider>();
        playerlocation = GameObject.FindGameObjectWithTag("Player").transform.position;
        ThisGuv.SetDestination(playerlocation);
        StartCoroutine("B1");

        ThisGuv.updateRotation = false;

        //audio
        AttackSFX = Resources.LoadAll<AudioClip>("Anime/Attack");
        DeadSFX = Resources.LoadAll<AudioClip>("Anime/Dead");
        HurtSFX = Resources.LoadAll<AudioClip>("Anime/Hurt");
    }

    // Update is called once per frame
    void Update()
    {
        if (!Dead)
        {
            playerlocation = GameObject.FindGameObjectWithTag("Player").transform.position;
            ThisGuv.SetDestination(playerlocation);
        }
        Ray ray = new Ray(transform.position, playerlocation - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.transform.tag == "Player")
            {
                haslos = true;
            }
            else
            {
                haslos = false;
            }
        } 
        
    }

    private void LateUpdate()
    {
        if (!Dead)
        {
            transform.LookAt(playerlocation);
        }
    }

    public void GetShot() {
        HP--;
        CheckIfDead();
    }

    private void CheckIfDead()
    {
        if (HP <= 0)
        {
            Dead = true;
            ThisGuv.enabled = false;
            Die();
        } else
        {
            thisAudioSource.PlayOneShot(HurtSFX[Random.Range(0, HurtSFX.Length)]);
        }
    }

    private void Die()
    {
        if (!FallOver)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y-0.51f, transform.position.z);
            thisAudioSource.PlayOneShot(DeadSFX[Random.Range(0, DeadSFX.Length)]);
            transform.Rotate(0, 0, 90);
            FallOver = true;
            StopCoroutine("B1");
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine("SaveMemory");
        }
    }

    IEnumerator B1() {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f,2.5f));
            for (int i = 0; i < 3; i++)
            {
                if (haslos)
                {
                    StartCoroutine(B1Attack());
                    yield return new WaitForSeconds(0.2f);
                }
            }
        } 
    }

    IEnumerator B1Attack()
    {
        GameObject CurrnetAttack = Instantiate(attack, transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
        CurrnetAttack.GetComponent<Rigidbody>().AddForce((playerlocation-transform.position)*force);
        thisAudioSource.PlayOneShot(AttackSFX[Random.Range(0, AttackSFX.Length)]);
        yield return new WaitForSeconds(2);
        Destroy(CurrnetAttack);
    }

    IEnumerator SaveMemory()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}