using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Настройки оружия")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float fireRate = 5f;
    
    private float nextFireTime = 0f;
    private Camera playerCamera;
    
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
            playerCamera = Camera.main;
        if (playerCamera == null)
            playerCamera = FindFirstObjectByType<Camera>();
    }
    
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && CanFire())
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
    
    private bool CanFire()
    {
        return bulletPrefab != null && firePoint != null && playerCamera != null;
    }
    
    private void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        
        Vector3 direction = playerCamera.transform.forward;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }
}