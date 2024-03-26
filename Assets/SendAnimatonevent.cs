using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SendAnimatonevent : MonoBehaviour
{
    public PlayerPhotonsoundmanger playerPhotonSoundManager;

    public void TriggerFootStepSFX()
    {
        playerPhotonSoundManager.PlayFootStepSFX();
    }
    
}
