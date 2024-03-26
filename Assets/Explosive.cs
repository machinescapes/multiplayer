using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class Explosive : MonoBehaviour
{
    [HideInInspector]
    public bool isLocalExplosive = false;


    private bool alreadyExploded = false;
    [Header("stats")]
    public float explosionRadius = 5f;
    public int damage = 30;

    [Header("Fire Setings")]
    public float fireForce;


    [Header("VFX")]
    public GameObject exposionVFX;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * fireForce);

    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isLocalExplosive)
        { return;}
            
        PhotonNetwork.Instantiate(exposionVFX.name, transform.position, Quaternion.identity);
        Explode();
    }

    void Explode()
    {

        if (alreadyExploded)
            return;
        alreadyExploded = true;
        foreach (var collider in Physics.OverlapSphere(transform.position, explosionRadius))
            {
            
           
            if (collider.transform.gameObject.GetComponent<Heath>())
            {
                // damgte score PhotonNetwork.LocalPlayer.AddScore(damage);

                if (collider.transform.gameObject.GetComponent<Heath>() && collider.transform.gameObject.GetComponent<Heath>().isLocalPlayer == false)
                {
                    // kill score
                    RoomManger.instance.kills++;
                    RoomManger.instance.SetHashes();
                    PhotonNetwork.LocalPlayer.AddScore(1);
                }
                collider.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }
        PhotonNetwork.Destroy(gameObject);
    }
}
