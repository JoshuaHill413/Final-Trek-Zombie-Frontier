using UnityEngine;
using UnityEngine.InputSystem;

public class Josh_AutoFireController : MonoBehaviour
{
    public float fireRate = 8f;
    public Transform firePoint;
    public AudioClip shootSound;
    private float nextFireTime = 0f;
    private bool isFiring = true;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isFiring && Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + (1f / fireRate);
        }
    }

    void Shoot()
    {
        audioSource.PlayOneShot(shootSound);

        GameObject bullet = Josh_BulletPooler.Instance.GetBullet();

        if (bullet == null) return;

        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = transform.rotation;

        Josh_Bullet bulletScript = bullet.GetComponent<Josh_Bullet>();
        bulletScript.Launch(transform.up);
    }

    public void SetFiring(bool state)
    {
        isFiring = state;
    }
}