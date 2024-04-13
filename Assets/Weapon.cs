using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

public class Weapon : MonoBehaviour
{
    public Image ammoCircle;
    public int damage;
    public int pelletsCount = 1;
    public float sprayMultiplier = 0f;
    public float fireRate;
    public Camera camera;
    public bool isProjectileWeapon = false;
    public GameObject projectile;
    public Transform projectileExit;
    private float nextFire;
    public GameObject hitVFX;
    public int mag = 5;
    public int ammo = 30;
    public int magAmmo = 30;
    public int shootSFXIndex = 0;
    public PlayerPhotonsoundmanger playerPhotonsoundmanger;
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;
    public Animation animation;
    public AnimationClip reload;
    public float recoverPerscent = 0.7f;
    public float recoilUp = 1f;
    public float recoilBack = 0f;
    private Vector3 originalPostion;
    private Vector3 recoilVelocity = Vector3.zero;
    private float recoilLength;
    private float recoverLength;
    private bool recoiling = false;
    public bool recovering;

    void Start()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo + " / " + magAmmo;
        SetAmmo();
        originalPostion = transform.localPosition;
        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPerscent;
    }

    void SetAmmo()
    {
        ammoCircle.fillAmount = (float)ammo / magAmmo;
    }

    void Update()
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

        float triggerValue = Input.GetAxis("FireTrigger");
        bool mousePressed = Input.GetButton("Fire1") || triggerValue > 0.1f; // Adjust trigger sensitivity here

        if (mousePressed && nextFire <= 0 && ammo > 0 && !animation.isPlaying)
        {
            Fire();
        }

        bool keyReload = Input.GetKeyDown(KeyCode.R);
        bool joystickReload = Input.GetKeyDown(KeyCode.JoystickButton2); // 'X' button on an Xbox controller
        if ((keyReload || joystickReload) && mag > 0 && ammo < magAmmo)
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

    void Fire()
    {
        nextFire = 1 / fireRate;
        ammo--;
        magText.text = mag.ToString();
        ammoText.text = ammo + " / " + magAmmo;
        SetAmmo();
        recoiling = true;
        recovering = false;
        playerPhotonsoundmanger.PlayerShootSFX(shootSFXIndex);

        if (isProjectileWeapon)
        {
            ProjectileFire();
        }
        else
        {
            BulletFire();
        }
    }

    void ProjectileFire()
    {
        GameObject myProjectile = PhotonNetwork.Instantiate(projectile.name, projectileExit.position, projectileExit.rotation);
        myProjectile.GetComponent<Explosive>().isLocalExplosive = true;
    }

    void BulletFire()
    {
        for (int i = 0; i < pelletsCount; i++)
        {
            Vector3 sprayOffset = Random.insideUnitCircle * sprayMultiplier;
            sprayOffset.z = 0; // Ensure the z-component is zero for 2D spray
            Ray ray = new Ray(camera.transform.position, camera.transform.forward + sprayOffset);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
            {
                PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);
                // Additional effects like damage handling can be added here
            }
        }
    }

    void Reload()
    {
        animation.Play(reload.name);
        mag--;
        ammo = magAmmo;
        magText.text = mag.ToString();
        ammoText.text = ammo + " / " + magAmmo;
        SetAmmo();
    }

    void Recoil()
    {
        Vector3 finalPosition = new Vector3(originalPostion.x, originalPostion.y + recoilUp, originalPostion.z - recoilBack);
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
