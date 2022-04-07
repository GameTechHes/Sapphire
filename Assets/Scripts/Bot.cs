using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    public static float MAX_SPEED = 2.5f;
    public static float MIN_SPEED = 0.5f;
    public static float speed;

    [SerializeField] private float wanderRadius;

    [SerializeField] private float maxWanderTimer;
    [SerializeField] private float minWanderTimer;

    [SerializeField] private float angleMax;

    private Animator animator;

    private float floorMinX;
    private float floorMaxX;
    private float floorMinZ;
    private float floorMaxZ;

    private NavMeshAgent agent;
    private float wanderTimer;
    private float timer;

    public bool isActive = false;

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2);
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // GameObject floor = GameObject.Find("Floor");
        // floorMinX = floor.GetComponent<MeshFilter>().mesh.bounds.min.x * floor.transform.localScale.x;
        // floorMaxX = floor.GetComponent<MeshFilter>().mesh.bounds.max.x * floor.transform.localScale.x;
        // floorMinZ = floor.GetComponent<MeshFilter>().mesh.bounds.min.z * floor.transform.localScale.z;
        // floorMaxZ = floor.GetComponent<MeshFilter>().mesh.bounds.max.z * floor.transform.localScale.z;
    }

    void Update()
    {
        if (!GetComponent<NavMeshAgent>().enabled) return;
        if (agent.remainingDistance - agent.stoppingDistance < 0.1)
        {
            if (animator.GetBool("walking")) animator.SetBool("walking", false);

            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 randDirection = Random.insideUnitSphere * wanderRadius;
                randDirection += transform.position;
                NavMeshHit navHit;
                if (NavMesh.SamplePosition(randDirection, out navHit, wanderRadius, NavMesh.AllAreas))
                {
                    Vector3 newPos = navHit.position;
                    animator.SetBool("walking", true);
                    agent.SetDestination(newPos);
                    timer = 0;
                    wanderTimer = Random.Range(minWanderTimer, maxWanderTimer);
                    print("New target");
                }
            }
        }
        else
        {
            timer = 0;
        }
    }
}