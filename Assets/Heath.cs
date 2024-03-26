using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;






public class Heath : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public int health;
    public bool isLocalPlayer;


    [Header("UI")]
    public RectTransform healthBar; 
    private float originalHealthBarSize;
    private bool hasDied;


    private void Start()
    {
        originalHealthBarSize = healthBar.sizeDelta.x;
        
    }

    private void Update()
    {
        
        
    }



    [PunRPC]
    public void TakeDamage(int _damage)
    {
        if (hasDied)
        {
               return;
        }
        health -= _damage;


        healthBar.sizeDelta = new Vector2(x: originalHealthBarSize * health / 100f, y: healthBar.sizeDelta.y);

        healthText.text = health.ToString();

        if (health <= 0)
        {
            hasDied = true;
            if (isLocalPlayer)
            {
                RoomManger.instance.spawnPlayer();
                RoomManger.instance.deaths++;
                RoomManger.instance.SetHashes();
            }

                
                
            
            Destroy(gameObject);

        }
    }
}
