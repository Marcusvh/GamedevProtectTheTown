using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextSpeech : Collidable
{
    public string msg;

    private float cooldown = 4.0f;
    private float lastShout = -4.0f;
    protected override void OnCollide(Collider2D coll)
    {
        if (Time.time - lastShout > cooldown)
        {
            lastShout = Time.time;
            GameManager.instance.Showtext(msg, 25, Color.white, transform.position + new Vector3(0, 0.16f ,0), Vector3.zero, cooldown);
        }
    }
}
