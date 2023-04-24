using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    // Damage struct

    public int[] DamagePoint = { 1, 2, 3, 4, 5, 6, 7, 8 };
    public float[] pushForce = { 2.0f, 2.2f, 2.5f, 2.7f, 3.0f, 3.2f, 3.5f, 3.8f };

    // upgrade
    public int Weapondlevel = 0;
    public SpriteRenderer spriteRender;

    // swing
    private Animator anim;
    private float Cooldown = 0.4f;
    private float LastSwing;

    protected override void Start()
    {
        base.Start();       
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if(Input.GetKey(KeyCode.Space))
        {
            if(Time.time - LastSwing > Cooldown)
            {
                LastSwing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if(coll.tag == "Fighter")
        {
            if(coll.name == "Player") return;

            // dmg object then send it to the fighter we hit
            Damage dmg = new Damage()
            {   
                damageAmount = DamagePoint[Weapondlevel],
                origin = transform.position,
                pushForce = pushForce[Weapondlevel]
            };
            coll.SendMessage("ReceiveDamage", dmg, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void Swing()
    {
        anim.SetTrigger("Swing");
    }
    public void UpgradeWeapon()
    {
        Weapondlevel++;
        spriteRender.sprite = GameManager.instance.WeaponSprites[Weapondlevel];
       
        // change stats
    }
    public void SetWeaponLevel(int level)
    {
        Weapondlevel = level;
        spriteRender.sprite = GameManager.instance.WeaponSprites[Weapondlevel];
    }
}
