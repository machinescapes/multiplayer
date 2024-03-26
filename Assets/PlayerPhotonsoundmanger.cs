using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerPhotonsoundmanger : MonoBehaviour
{
    public AudioSource footStepSource;
    public AudioClip footstepSFX;

    public AudioSource gunShootSource;
    public AudioClip[] allGunShootsSFX;
    public void PlayFootStepSFX()
    {
       GetComponent<PhotonView>().RPC("PlayFootStepSFX_RPC", RpcTarget.All);

        //pich and volume
        
    }
    [PunRPC]
    public void PlayFootStepSFX_RPC()
    {
        footStepSource.clip = footstepSFX;

        //pich and volume
        footStepSource.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
        footStepSource.volume = UnityEngine.Random.Range(0.2f, 0.35f);
        footStepSource.Play();
    }

    public void PlayerShootSFX(int index)
    {
        GetComponent<PhotonView>().RPC("PlayerShootSFX_RPC", RpcTarget.All, index);
    }
    [PunRPC]
    public void PlayerShootSFX_RPC(int index)
    {
        gunShootSource.clip = allGunShootsSFX[index];

        //pich and volume
        gunShootSource.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
        gunShootSource.volume = UnityEngine.Random.Range(0.2f, 0.35f);
        gunShootSource.Play();
    }
}
   