using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Vector3 fireDirection = new Vector3(0, 0, 1.00f);
    public float shootForce = 20.00f;
    public float fireRate = 2.00f;

    private float nextFireTime;
    void Start()
    {
  
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;
   
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(fireDirection.normalized * shootForce, ForceMode.Impulse);
        }
    }
    private void OnDrawGizmos()
    {
        if (firePoint == null) return;

        Gizmos.color = Color.cyan;

        Vector3 directionVisual = fireDirection.normalized * (shootForce * 0.10f);
        Gizmos.DrawRay(firePoint.position, directionVisual);
        Gizmos.DrawSphere(firePoint.position + directionVisual, 0.15f);
    }
}
