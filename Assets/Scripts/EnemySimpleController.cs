using UnityEngine;
using UnityEngine.AI;

public class EnemySimpleController : MonoBehaviour
{
    public Transform Target;
    private NavMeshAgent agentEnemy;

    public float detectionRange = 10f;
    public float stopDistance = 1.50f;
    void Start()
    {
        agentEnemy = GetComponent<NavMeshAgent>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Humanoid");
        if (playerObj != null) Target = playerObj.transform;

        if (agentEnemy != null)
        {
            agentEnemy.speed = Random.Range(2f, 4f);
            agentEnemy.acceleration = Random.Range(5f, 10f);
            agentEnemy.stoppingDistance = stopDistance;
        }
    }

    void Update()
    {
        DistancePlayer();
    }
    public void DistancePlayer()
    {
        if (Target == null || !agentEnemy.isOnNavMesh) return;

        float distanceToPlayer = Vector3.Distance(transform.position, Target.position);

        if (distanceToPlayer <= detectionRange)
        {
            agentEnemy.isStopped = false;
            agentEnemy.SetDestination(Target.position);
        }
        else
        {
            agentEnemy.isStopped = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Humanoid"))
        {
            Debug.Log("Enemigo alcanzó al jugador y vuela alto.");
            Destroy(gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (agentEnemy == null || agentEnemy.path == null) return;

        Gizmos.color = Color.red;
        Vector3[] corners = agentEnemy.path.corners;

        if (corners.Length < 2) return;

        for (int i = 0; i < corners.Length - 1; i++)
        {
            Gizmos.DrawLine(corners[i], corners[i + 1]);
            Gizmos.DrawSphere(corners[i], 0.20f);
        }
    }
}
