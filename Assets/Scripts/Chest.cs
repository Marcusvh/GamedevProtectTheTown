using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int currencyAmount = 10;
    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.Currency += currencyAmount;
            GameManager.instance.Showtext("+" + currencyAmount + " currency!", 25, Color.yellow, transform.position, Vector3.up * 50, 1.5f);
        }
    }
}
