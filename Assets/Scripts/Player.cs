using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;
    private float lastPlayed;
    private float playTime = 1.0f;
    [SerializeField]
    public AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive) return;

        if (Time.time - lastPlayed > playTime)
        {
            audioSource.PlayOneShot(audioSource.clip, 1);
            lastPlayed = Time.time;
        }

        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitpointChange();
    }

    protected override void Death()
    {
        isAlive = false;
        GameManager.instance.deathMenuAnimator.SetTrigger("Show");
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if(isAlive)
            UpdateMoter(new Vector3(x, y, 0));
    }

    internal void Heal(int healingAmount)
    {
        if (hitpoint == maxHitpoint)
            return;

        hitpoint += healingAmount;
        if (hitpoint > maxHitpoint)
        {
            hitpoint = maxHitpoint;
        }
        GameManager.instance.Showtext("+ " + healingAmount + " HP", 25, Color.magenta, transform.position, Vector3.up * 30, 1.0f);
        GameManager.instance.OnHitpointChange();
    }

    internal void SwapSprite(int skinID)
    {
        spriteRenderer.sprite = GameManager.instance.PlayerSprites[skinID];
    }
    public void OnLevelUp()
    {
        maxHitpoint++;
        hitpoint = maxHitpoint;
    }
    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
        {
            OnLevelUp();
        }
    }

    internal void Respawn()
    {
        Heal(maxHitpoint);
        isAlive = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
    }
}
