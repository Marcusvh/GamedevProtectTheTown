using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Portal : Collidable
{
    public string[] sceneNames;

    public static int newBossHealth = 10;

    protected override void OnCollide(Collider2D coll)
    {
        if(coll.name == "Player")
        {
            if (SceneManager.GetActiveScene().name == "Dungeon1")
            {
                newBossHealth = (int)Unity.Mathematics.math.round((double)newBossHealth * 1.2);
                Debug.Log(newBossHealth);
            } 
            // TP the player
            GameManager.instance.SaveState();
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            SceneManager.LoadScene(sceneName);
        }
    }
}
