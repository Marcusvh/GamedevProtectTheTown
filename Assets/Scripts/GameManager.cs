using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(Player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud.gameObject);
            Destroy(menu.gameObject);
            return;
        }
        instance = this;
        // calls all methodes in LoadState
        // for at reset player state så skal de to nedestående kommenterets, spillet skal køres og så skal de nedestående udkommenteret igen.
        // De to gør at player staten bliver husket fra sesion til sesion.
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Ressources
    public List<Sprite> PlayerSprites;
    public List<Sprite> WeaponSprites;
    public List<int> WeaponPrices;
    public List<int> XpTable;

    // References
    public Player Player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public Animator deathMenuAnimator;
    public GameObject hud;
    public GameObject menu;

    //Logic
    public int Currency;
    public int Experience;

    // floatingText
    public void Showtext(string msg, int fontsize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontsize, color, position, motion, duration);
    }

    // Upgrade weapon
    public bool TryUpgradeWeapon()
    {
        // is the weapon max lvl
        if (WeaponPrices.Count <= weapon.Weapondlevel) return false;

        if(Currency >= WeaponPrices[weapon.Weapondlevel])
        {
            Currency -= WeaponPrices[weapon.Weapondlevel];
            weapon.UpgradeWeapon();
            return true;
        }
        return false;
    }

    // Hitpoint Bar
    public void OnHitpointChange()
    {
        float ratio = (float) Player.hitpoint / (float) Player.maxHitpoint;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
    }

    // Experience system
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while(Experience >= add)
        {
            add += XpTable[r];
            r++;

            if (r == XpTable.Count) return r;
        }
        return r;
    }
    public int GetXPToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while(r < level)
        {
            xp += XpTable[r];
            r++;
        }
        return xp;
    }
    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        Experience += xp;
        if (currLevel < GetCurrentLevel())
        {
            OnLevelUp();
        }
    }

    private void OnLevelUp()
    {
        Debug.Log("lvl up");
        Player.OnLevelUp();
        OnHitpointChange();
    }

    /*
     * INT PreferedSkin
     * INT currency
     * INT XP
     * INT weapondLevel
     * 
     */
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        Player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    // Death menu and respawn

    public void Respawn()
    {
        deathMenuAnimator.SetTrigger("Hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        Player.Respawn();
    }
    public void SaveState()
    {
        string s = "";

        s += "0" + "|";
        s += Currency.ToString() + "|";
        s += Experience.ToString() + "|";
        s += weapon.Weapondlevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;

        if (!PlayerPrefs.HasKey("SaveState")) return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // Currency
        Currency = int.Parse(data[1]);

        //XP
        Experience = int.Parse(data[2]);
        if (GetCurrentLevel() != 1)
            Player.SetLevel(GetCurrentLevel());

        // change weapon lvl
        weapon.SetWeaponLevel(int.Parse(data[0]));
    }

}
