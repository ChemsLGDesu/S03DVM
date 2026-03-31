using UnityEngine;
using UnityEngine.AI;

public class EnemySimpleController : MonoBehaviour
{
    public Transform Target;
    private NavMeshAgent agentEnemy;

    void Start()
    {
        agentEnemy = GetComponent<NavMeshAgent>();
        Target = GameObject.FindGameObjectWithTag("Humanoid").transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null && agentEnemy.isOnNavMesh)
        {
            agentEnemy.SetDestination(Target.position);

            agentEnemy.speed = Random.Range(2f, 4f);
            agentEnemy.acceleration = Random.Range(5f, 10f);
            agentEnemy.stoppingDistance = Random.Range(1f, 3f);
            agentEnemy.avoidancePriority = Random.Range(0, 99);
            agentEnemy.angularSpeed = Random.Range(0, 120);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (agentEnemy == null || agentEnemy.path == null) return;

        Vector3[] corners = agentEnemy.path.corners;


        for (int i = 0; i < corners.Length - 1; i++)
        {
            Gizmos.DrawLine(corners[i], corners[i + 1]);
            Gizmos.DrawSphere(corners[i], 0.2f);
        }

    }
}
