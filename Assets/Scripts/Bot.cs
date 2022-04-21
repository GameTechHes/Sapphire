using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    [SerializeField] private float wanderRadius;

    [SerializeField] private float maxWanderTimer;
    [SerializeField] private float minWanderTimer;

    [SerializeField] private float angleMax;

    private Animator animator;

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
    public void setAgentSpeed(float speed){
        agent.speed = speed;
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetBool("walking", false);
        animator.SetBool("die", false);

    }
    public void setNewDestination(Vector3 dest){
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(dest, out navHit, wanderRadius, NavMesh.AllAreas))
        {
            Vector3 newPos = navHit.position;
            if(!animator.GetBool("playerDetected")){
                animator.SetBool("walking", true);
            }else{
                animator.SetBool("walking", false);
            }
            agent.SetDestination(newPos);
            timer = 0;
            wanderTimer = Random.Range(minWanderTimer, maxWanderTimer);
            print("New target");
        }
    }
    public void resetCurrentPath(){
        agent.ResetPath();
    }
    void Update()
    {
        if (!GetComponent<NavMeshAgent>().enabled) return;
        if (agent.remainingDistance - agent.stoppingDistance < 0.1 )
        {

            if (animator.GetBool("walking")) animator.SetBool("walking", false);
            if (animator.GetBool("playerDetected")) animator.SetBool("playerDetected", false);

            if(!animator.GetBool("attack")){
                timer += Time.deltaTime;
                if (timer >= wanderTimer)
                {
                    Vector3 randDirection = Random.insideUnitSphere * wanderRadius;
                    randDirection += transform.position;
                    this.setNewDestination(randDirection);
                }
            }
        }
        else
        {
            timer = 0;
        }
    }
}