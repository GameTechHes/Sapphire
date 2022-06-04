using System.Collections;
using Fusion;
using Sapphire;
using UnityEngine;

public class FieldOfView : NetworkBehaviour
{
    public int viewAngle = 30;
    public float detectionRadius = 2;
    public float minTimeBeforeUpdateTarget = 2;
    public float timeBeforeUnfocusPlayer = 10;
    public float playerRange = 3;
    private float time;
    public LayerMask layerMask;
    private Bot bot;
    private Animator animator;
    private GameObject playerOnFocus;

    public GameObject dieEffet;
    private bool playeffet;
    [Networked] public NetworkBool IsDead { get; set; }

    [SerializeField] private float wanderTimer;

    [SerializeField] private float wanderRadius;

    enum BotState
    {
        Idle,
        Walking,
        Focus, // Player detected
        Attacking,
        Dying,
    }

    private BotState state = BotState.Idle;

    public override void Spawned()
    {
        bot = GetComponent<Bot>();
        animator = GetComponent<Animator>();
        animator.SetBool("attack", false);
        if (viewAngle > 90) viewAngle = 90;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }

    /**
     * Check if the raycast hit the player
     */
    bool IsInRaycast(Vector3 direction)
    {
        var position = transform.position;
        Vector3 raySrc = new Vector3(position.x, position.y + 1, position.z);
        if (!Physics.Raycast(raySrc, direction, out _, direction.magnitude + 1, layerMask))
        {
            Debug.DrawRay(raySrc, direction, Color.green);
            return true;
        }

        Debug.DrawRay(raySrc, direction, Color.red);
        return false;
    }

    /**
     * Check if the angle between the direction and the bot forward is lower than a threshold 
     */
    bool IsInViewAngle(Vector3 direction)
    {
        float angle = Vector3.Angle(transform.forward, direction);
        if (angle < viewAngle) return true;

        return false;
    }

    public override void FixedUpdateNetwork()
    {
        return;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        if (IsDead) return;
        switch (state)
        {
            case BotState.Idle:
                bot.SetAgentSpeed(2);
                bot.SetStoppingDistance(1.0f);
                animator.SetBool("attack", false);

                foreach (var hitCollider in hitColliders)
                {

                    if (hitCollider.gameObject.CompareTag("Player") && hitCollider.gameObject.GetComponent<Knight>() != null)
                    {
                        playerOnFocus = hitCollider.gameObject;

                        Vector3 directionIdle = playerOnFocus.transform.position - transform.position;
                        if (IsInViewAngle(directionIdle) && IsInRaycast(directionIdle))
                        {
                            bot.ResetCurrentPath(); // Just in case something is going wrong
                            bot.SetNewDestination(playerOnFocus.transform.position);
                            time = 0;
                            state = BotState.Focus;
                            break;
                        }
                    }
                }

                //No valid target --> Just chilling
                if (time >= wanderTimer)
                {
                    time = 0;
                    Vector3 randDirection = Random.insideUnitSphere * wanderRadius;
                    randDirection += transform.position;
                    bot.SetNewDestination(randDirection);
                }

                break;

            case BotState.Focus:
                Vector3 direction = playerOnFocus.transform.position - transform.position;
                bot.SetAgentSpeed(4);
                bot.SetStoppingDistance(4.0f);
                if (direction.magnitude <= playerRange)
                {
                    bot.ResetCurrentPath();
                    state = BotState.Attacking;
                    break;
                }

                if (IsInRaycast(direction))
                {
                    if (time > minTimeBeforeUpdateTarget)
                    {
                        bot.ResetCurrentPath(); // Because you can't set a path during another path
                        bot.SetNewDestination(playerOnFocus.transform.position);
                        time = 0;
                    }
                }

                if (time > timeBeforeUnfocusPlayer)
                {
                    state = BotState.Idle;
                }

                break;
            case BotState.Attacking:
                Vector3 directionAttack = playerOnFocus.transform.position - transform.position;
                if (directionAttack.magnitude > playerRange)
                {
                    state = BotState.Idle;
                    break;
                }

                if (!IsInViewAngle(directionAttack))
                {
                    state = BotState.Idle;
                }

                if (bot.canShoot)
                {
                    animator.SetBool("attack", true);
                }
                else
                {
                    animator.SetBool("attack", false);
                }

                bot.Attack();
                Quaternion rot = Quaternion.LookRotation(directionAttack);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.8f);
                break;
        }

        time += Runner.DeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Arrow"))
        {
            RPC_SetIsDead(true);
            bot.ResetCurrentPath();
            animator.SetBool("attack", false);
            animator.SetBool("die", true);
            StartCoroutine(Despawn());
            Invoke("SpawnEffect", 1.0f);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_SetIsDead(NetworkBool isDead)
    {
        IsDead = isDead;
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length +
                                        animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (Object != null && Object.IsValid)
        {
            Runner.Despawn(Object);
        }
    }

    void SpawnEffect()
    {
        // Pourquoi on spawn un effet ?? C'est le même que la fireball, c'est bizarre non ?
        // var obj = Instantiate(dieEffet, transform.position, transform.rotation) as GameObject;
        // Destroy(obj, 2);
    }
}