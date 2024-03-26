using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using Unity.VisualScripting;



public class Weapon : MonoBehaviour
{

    public Image ammoCircle;



    public int damage;

    public int pelletsCount = 1;
    public float sprayMultiplier = 0f;

    public float fireRate;
    public Camera camera;

    [Header("Projecttile wepon setings")]
    public bool isProjectileWeapon = false;
    public GameObject projectile;
    public  Transform projectileExit;
    private float nextFire;

    [Header("VFX")]
    public GameObject hitVFX;

    [Header("ammo")]
    public int mag = 5;
    public int ammo = 30;
    public int magAmmo = 30;

    [Header("SFX")]
    public int shootSFXIndex = 0;
    public PlayerPhotonsoundmanger playerPhotonsoundmanger;


    [Header("UI")]
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;


    [Header("Animation")]
    public Animation animation;
    public AnimationClip reload;

    [Header("Recol seting")]
    //[Range(0, 1)]
   // public float recoilPersent = 0.3f;
    [Range(0, 2)]
    public float recoverPerscent = 0.7f;
    [Space]
    public float recoilUp = 1f;
     public float recoilBack = 0f;




    private Vector3 originalPostion;
    private Vector3 recoilVelocity = Vector3.zero;

    private float recoilLength;
    private float recoverLength;


    private bool recoiling = false;
    public bool recovering;

    void SetAmmo()
    {
        ammoCircle.fillAmount = (float)ammo / magAmmo;
    }



    

    public void Start()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo + " / " + magAmmo;
        SetAmmo();

        originalPostion = transform.localPosition;
        
        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPerscent;
    }


    // Update is called once per frame
    void Update()
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }



        if (Input.GetButton("Fire1") && nextFire <= 0 && ammo > 0 && animation.isPlaying == false)
        
        {
            nextFire = 1 / fireRate;
            ammo--; 

            magText.text = mag.ToString();
            ammoText.text = ammo + " / " + magAmmo;

            SetAmmo();

            if (isProjectileWeapon)
            {
                ProjectileFire();
            }
            else
            {
                Fire();
            }

            
        }

        if (Input.GetKeyDown(KeyCode.R) &&  mag > 0)
        {
            Reload();
        }

        if (recoiling)
        {
            Recoil();
        }

        if (recovering)
        {
            Recovering();
        }

    }

    
    

    void ProjectileFire()
    {
        GameObject myprojecttile =PhotonNetwork.Instantiate(projectile.name, projectileExit.position, projectileExit.rotation);
        myprojecttile.GetComponent<Explosive>().isLocalExplosive = true;
        playerPhotonsoundmanger.PlayerShootSFX(shootSFXIndex);
    }

    void Reload()
    {
        animation.Play(reload.name);    

        if(mag > 0)
        {
           
            if (mag > 0) 
            {

                mag--;

                ammo = magAmmo;
            }
        }
        magText.text = mag.ToString();
        ammoText.text = ammo + " / " + magAmmo;
        SetAmmo();

    }

    void Fire()
    {

        recoiling = true;
        recovering = false;
        playerPhotonsoundmanger.PlayerShootSFX(shootSFXIndex);
        for (int i = 0; i < pelletsCount; i++)
        {
            Vector3 sprayOffsct = Random.insideUnitCircle * sprayMultiplier;
            sprayOffsct.z = 0;


            Ray ray = new Ray(camera.transform.position, camera.transform.forward + sprayOffsct);

            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
            {
                PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);
                if (hit.transform.gameObject.GetComponent<Heath>())
                {
                    // damgte score PhotonNetwork.LocalPlayer.AddScore(damage);

                    if (damage >= hit.transform.gameObject.GetComponent<Heath>().health)
                    {
                        // kill score
                        RoomManger.instance.kills++;
                        RoomManger.instance.SetHashes();
                        PhotonNetwork.LocalPlayer.AddScore(1);
                    }
                    hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
                }
            }
        }

        // adds score ever time fired
        //PhotonNetwork.LocalPlayer.AddScore(1);


        

        
    }


    void Recoil()
    {
        Vector3 finalPosition = new Vector3(originalPostion.x, y:originalPostion.y + recoilUp, z:originalPostion.z - recoilBack);

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLength);


        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = true;
        }
    }


    void Recovering()
    {
        Vector3 finalPosition = originalPostion;

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLength);


        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = false;
        }
    }
}
