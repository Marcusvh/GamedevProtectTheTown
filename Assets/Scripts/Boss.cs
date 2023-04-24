using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float[] fireballSpeed = {2.5f, -2.5f};
    public float distance = 0.25f;
    public Transform[] fireballs;

    private void Start()
    {
        base.Start();

        hitpoint = Portal.newBossHealth;
        maxHitpoint = Portal.newBossHealth;
    }

    private void Update()
    {
        if(fireballSpeed.Length > 0 && fireballs.Length > 0)
        {
            for (int i = 0; i < fireballSpeed.Length; i++)
            {
                Vector3 spinningDudesPosition = new Vector3(-Mathf.Cos(Time.time * fireballSpeed[i]) * distance,
                    Mathf.Sin(Time.time * fireballSpeed[i]) * distance, 0);

                if (fireballs[i] == null || fireballs.Length == 0)
                {
                    Destroy(GetComponent<Boss>());
                    return;
                }
                else
                {
                    fireballs[i].position = transform.position + spinningDudesPosition;
                }
            }
        }
        
    }
}
