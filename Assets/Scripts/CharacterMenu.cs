using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    // Text fields
    public TMPro.TextMeshProUGUI levelText, hitPointText, currencyText, upgradeCostText, xpText;

    // Logic
    private int currenctCharacterSelection = 0;
    public Image charaterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    // character Selection
    public void OnArrowClick(bool right)
    {
        if(right)
        {
            currenctCharacterSelection++;

            if(currenctCharacterSelection == GameManager.instance.PlayerSprites.Count)
            {
                currenctCharacterSelection = 0;
            }
            OnSelectChanged();
        }
        else
        {
            currenctCharacterSelection--;
            if (currenctCharacterSelection < 0)
            {
                currenctCharacterSelection = GameManager.instance.PlayerSprites.Count - 1;
            }
            OnSelectChanged();
        }
    }

    private void OnSelectChanged()
    {
        charaterSelectionSprite.sprite = GameManager.instance.PlayerSprites[currenctCharacterSelection];
        GameManager.instance.Player.SwapSprite(currenctCharacterSelection);
    }

    // Weapon Upgrade
    public void OnUpgradeclick()
    {
        if(GameManager.instance.TryUpgradeWeapon())
        {
            UpdateMenu();
        }
    }

    // Update the character information
    public void UpdateMenu()
    {
        // Weapon
        weaponSprite.sprite = GameManager.instance.WeaponSprites[GameManager.instance.weapon.Weapondlevel];
        if(GameManager.instance.weapon.Weapondlevel == GameManager.instance.WeaponPrices.Count)
        {
            upgradeCostText.text = "FULLY UPGRADED";
            upgradeCostText.fontSize = 16;           
        }
        else
        {
            upgradeCostText.text = GameManager.instance.WeaponPrices.ToArray()[GameManager.instance.weapon.Weapondlevel].ToString();
        }

        // Meta
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitPointText.text = GameManager.instance.Player.hitpoint.ToString();
        currencyText.text = GameManager.instance.Currency.ToString();


        // xp Bar
        int currLevel = GameManager.instance.GetCurrentLevel();
        if(GameManager.instance.GetCurrentLevel() == GameManager.instance.XpTable.Count)
        {
            xpText.text = GameManager.instance.Experience.ToString() + " Total experience points";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLevelXp = GameManager.instance.GetXPToLevel(currLevel - 1);
            int currLevelXp = GameManager.instance.GetXPToLevel(currLevel);

            int diff = currLevelXp - prevLevelXp;
            int currXpIntoLevel = GameManager.instance.Experience - prevLevelXp;

            float completeRatio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completeRatio, 1, 1);
            xpText.text = currXpIntoLevel.ToString() + " / " + diff;
        }
    }
}
