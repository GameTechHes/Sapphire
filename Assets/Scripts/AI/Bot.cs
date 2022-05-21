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
    private bool _canShoot = true;

    private Animator animator;

    private NavMeshAgent agent;
    private float wanderTimer;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetBool("walking", false);
        animator.SetBool("die", false);
    }

    void Update()
    {
        if (!GetComponent<NavMeshAgent>().enabled) return;

        animator.SetFloat("animationBlend", agent.velocity.magnitude);
        if (agent.remainingDistance - agent.stoppingDistance < 100 && !animator.GetBool("die")) //0.1
        {
            if (animator.GetBool("walking")) animator.SetBool("walking", false);
            if (animator.GetBool("playerDetected")) animator.SetBool("playerDetected", false);

            if (!animator.GetBool("attack"))
            {

                timer += Time.deltaTime;
                if (timer >= wanderTimer)
                {
                    Vector3 randDirection = Random.insideUnitSphere * wanderRadius;
                    randDirection += transform.position;
                    SetNewDestination(randDirection);
                }
            }
            else
            {
                Attack();
            }
        }
        else
        {
            timer = 0;
        }
    }

    public void SetNewDestination(Vector3 dest)
    {
        if (animator.GetBool("die")) return;
        if (NavMesh.SamplePosition(dest, out var navHit, wanderRadius, NavMesh.AllAreas))
        {
            Vector3 newPos = navHit.position;
            if (!animator.GetBool("playerDetected"))
            {
                animator.SetBool("walking", true);
            }
            else
            {
                animator.SetBool("walking", false);
                Attack();
            }

            agent.SetDestination(newPos);
            timer = 0;
            wanderTimer = Random.Range(minWanderTimer, maxWanderTimer);
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
        if (_canShoot)
        {
            StartCoroutine(SendSpell());
            _canShoot = false;
        }
    }
    IEnumerator SendSpell()
    {

        FireBall fb = Runner.Spawn(fireball, launchStart.transform.position, transform.rotation);
        Rigidbody rb = fb.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * 20;

        yield return new WaitForSeconds(3);
        _canShoot = true;
        yield return new WaitForSeconds(2);
        fb.RPC_DespawnArrow();
    }
}