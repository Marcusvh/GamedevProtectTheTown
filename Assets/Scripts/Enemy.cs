using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Mover
{
    // XP 
    public int xpValue = 1;

    // Logic
    public float triggerLenght = 0.3f;
    public float chaseLenght = 1;
    private bool chasing;
    private bool collidingWithPlayer;

    // positioning
    private Transform playerTransform;
    private Vector3 startingPosition;

    // Sound
    private float lastPlayed;
    private float playTime = 0.2f;
    [SerializeField]
    public AudioSource audioSource;
    private GameObject boss;

    // hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    // animaton
    private Animator animator;
    protected override void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastPlayed > playTime)
        {
            audioSource.PlayOneShot(audioSource.clip, 1);
            lastPlayed = Time.time;
        }

        base.ReceiveDamage(dmg);
    }


    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.Player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();

        animator = GetComponent<Animator>();
        boss = GameObject.Find("boss_0");
    }
    private void FixedUpdate()
    {
        // is player in range?
        if(Vector3.Distance(playerTransform.position, startingPosition) < chaseLenght)
        {
            if(Vector3.Distance(playerTransform.position, startingPosition) < triggerLenght) chasing = true;
           
            if(chasing)
            {
                animator.SetBool("Run", true);
                if(!collidingWithPlayer)
                {
                    UpdateMoter((playerTransform.position - transform.position).normalized);
                }
            }
            else
            {
                UpdateMoter(startingPosition - transform.position);
            }           
        }
        else
        {
            UpdateMoter(startingPosition - transform.position);
            chasing = false;
            animator.SetBool("Run", false);
        }

        // check for overlaps
        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            if (hits[i].tag == "Fighter" && hits[i].name == "Player") collidingWithPlayer = true;

            hits[i] = null;
        }
    }

    protected override void Death()
    {
        if (gameObject.name == "boss_0")
        {          
            int childs = boss.transform.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                Destroy(boss.transform.GetChild(i).gameObject);
            }
            boss.GetComponent<BoxCollider2D>().enabled = false;
        }

        chasing = false;
        triggerLenght = 0.000f;
        chaseLenght = 0.000f;
        xSpeed = 0;
        ySpeed = 0;
        hitbox.enabled = false;
        animator.SetTrigger("Death");
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.Showtext("+" + xpValue + " XP", 30, Color.green, transform.position, Vector3.up, 1.0f);
    }
}
