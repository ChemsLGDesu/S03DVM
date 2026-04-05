using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 7f;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    private void OnCollisionEnter(Collision collision)
    {    
        if (collision.gameObject.CompareTag("Humanoid"))
        {
            Debug.Log("¡El proyectil impactó al jugador!");          
            Destroy(gameObject);
        }
    }
}
