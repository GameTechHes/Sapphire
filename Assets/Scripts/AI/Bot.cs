using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Fusion;

public class Bot : NetworkBehaviour
{
    [SerializeField] private float wanderRadius;
    [SerializeField] private float maxWanderTimer;
    [SerializeField] private float minWanderTimer;

    [SerializeField] private FireBall fireball;
    [SerializeField] private GameObject launchStart;
    public bool canShoot = true;

    private Animator animator;

    private NavMeshAgent agent;
    private float wanderTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetBool("die", false);
    }

    void Update()
    {
        if (!GetComponent<NavMeshAgent>().enabled) return;

        animator.SetFloat("animationBlend", agent.velocity.magnitude);
       
    }
    public void SetStoppingDistance(float dist){
        agent.stoppingDistance = dist;
    }
    public void SetNewDestination(Vector3 dest)
    {
        if (animator.GetBool("die")) return;
        if (NavMesh.SamplePosition(dest, out var navHit, wanderRadius, NavMesh.AllAreas))
        {
            Vector3 newPos = navHit.position;
            agent.SetDestination(newPos);
        }
    }

    public void SetAgentSpeed(float speed)
    {
        agent.speed = speed;

    }

    public void ResetCurrentPath()
    {
        agent.ResetPath();
    }



    public void Attack()
    {
        if (canShoot)
        {
            StartCoroutine(SendSpell());
            canShoot = false;
            animator.SetBool("attack", true);

        }
    }
    IEnumerator SendSpell()
    {
        yield return new WaitForSeconds(0.5f); //just to sync with animation
        FireBall fb = Runner.Spawn(fireball, launchStart.transform.position, transform.rotation);
        Rigidbody rb = fb.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * 20;

        yield return new WaitForSeconds(3);
        canShoot = true;
        yield return new WaitForSeconds(2);
        fb.RPC_Despawn();
    }
}