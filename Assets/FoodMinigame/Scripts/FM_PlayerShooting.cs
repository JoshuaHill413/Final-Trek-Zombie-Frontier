using UnityEngine;
using UnityEngine.InputSystem;
 
public class FM_PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
 
    public float fireRate = 4f;
    private float nextFireTime;
 
    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }
 
    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }
}
 