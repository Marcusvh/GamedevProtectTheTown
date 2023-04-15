using System.Collections;
using System.Collections.Generic;
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
    private Transform playerTransform;
    private Vector3 startingPosition;

    // hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.Player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();

    }
    private void FixedUpdate()
    {
        // is player in range?
        if(Vector3.Distance(playerTransform.position, startingPosition) < chaseLenght)
        {
            if(Vector3.Distance(playerTransform.position, startingPosition) < triggerLenght) chasing = true;
           
            if(chasing)
            {
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
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.Showtext("+" + xpValue + " XP", 30, Color.green, transform.position, Vector3.up, 1.0f);
    }
}
