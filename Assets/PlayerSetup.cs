using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.InputSystem;
public class PlayerSetup : MonoBehaviour
{
   public Movement movement;

    public GameObject camra;

    public string nickname;
    public TextMeshPro nicknameText;

    public Transform TPweaponHolder;


    public void IslocalPlayer()
    {

        TPweaponHolder.gameObject.SetActive(false);
        movement.enabled = true;
        camra.SetActive(true);

    }


    [PunRPC]
    public void SetTPWeapon(int _weaponIndex)
    {
       foreach(Transform _weapon in TPweaponHolder)
            {
                _weapon.gameObject.SetActive(false);
        }

       TPweaponHolder.GetChild(_weaponIndex).gameObject.SetActive(true);
    }




    [PunRPC]
    public void SetNickname(string _name)
    {
        nickname = _name;

        nicknameText.text = nickname;
    }


}
