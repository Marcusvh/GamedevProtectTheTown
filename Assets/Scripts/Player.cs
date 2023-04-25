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


    private Animator animator;
    [SerializeField]
    public AudioSource audioSource;

    private Animator anim;

    // For changing animation controller. must be the same order as in the charactor menu.
    public List<RuntimeAnimatorController> newCtr;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
        if(x > 0  || y > 0 || x < 0 || y < 0)
        {
            // nisser har ikke animation
            if(anim.runtimeAnimatorController != null)
                animator.SetBool("Run", true);
        }
        else
        {
            // nisser har ikke animation
            if (anim.runtimeAnimatorController != null)
                animator.SetBool("Run", false);
        }
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
        
        switch (GameManager.instance.PlayerSprites[skinID].name)
        {
            case "Player_0": anim.runtimeAnimatorController = null; break;
            case "Player_1": anim.runtimeAnimatorController = null; break;
            case "Player_Knight_Idle_0": anim.runtimeAnimatorController = newCtr[0];
                    //Resources.Load("Assets/Artwork/Animations/Player/Player_Knight.controller") as RuntimeAnimatorController;
                break;

            case "Player_Rogue_Idle_0": anim.runtimeAnimatorController = newCtr[1];
                    //Resources.Load("Assets/Artwork/Animations/Player/Player_Rogue.controller") as RuntimeAnimatorController;
                break;

            case "Player_Wizard_Idle_0": anim.runtimeAnimatorController = newCtr[2];
                    //Resources.Load("Assets/Artwork/Animations/Player/Player_Wizard.controller") as RuntimeAnimatorController;
                break;
        }
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
