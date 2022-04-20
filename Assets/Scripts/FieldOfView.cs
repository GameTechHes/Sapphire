using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public int viewAngle = 30;
    public float detectionRadius = 2;
    public float minTimeBeforeUpdateTarget = 2;
    public float timeBeforeUnfocusPlayer = 10;
    public float playerRange = 3;
    private float time = 0;
    private int layerMask = ~(1 << 3 | 1 << 7);
    private Bot bot;
    private Animator animator;
    private GameObject playerOnFocus;

    enum BotState
    {
        Idle,
        Walking,
        Focus,      //player detected
        Attacking,
        Diing,

    }
    private BotState state = BotState.Idle;

    void Start()
    {
        bot = GetComponent<Bot>();
        animator = GetComponent<Animator>();
        animator.SetBool("closeEnough", false);
        animator.SetBool("playerDetected", false);
        if (viewAngle > 90) viewAngle = 90;

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, detectionRadius);
    }
    bool isInRaycast(Vector3 direction, string tag = "")
    {
        RaycastHit hit;
        Vector3 ray_src = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        if (!Physics.Raycast(ray_src, direction, out hit, direction.magnitude + 1, layerMask))
        {
            Debug.DrawRay(ray_src, direction, Color.green);
            Debug.Log(tag + " Did Hit");
            return true;
        }
        Debug.DrawRay(ray_src, direction, Color.red);
        Debug.Log(tag + " Did Not Hit");
        return false;
    }
    bool isInViewAngle(Vector3 direction)
    {
        float angle = Vector3.Angle(transform.forward, direction);
        if (angle < viewAngle) return true;

        return false;
    }
    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        switch (state)
        {
            case BotState.Idle:
                bot.setAgentSpeed(2);
                animator.SetBool("closeEnough", false);
                animator.SetBool("playerDetected", false);

                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject.tag == "Player")
                    {
                        playerOnFocus = hitCollider.gameObject;
                        Vector3 direction_idle = playerOnFocus.transform.position - transform.position;
                        if (isInViewAngle(direction_idle))
                        {
                            if (isInRaycast(direction_idle, "[IDLE]"))
                            {
                                bot.resetCurrentPath(); //just in case something is going wrong
                                bot.setNewDestination(playerOnFocus.transform.position);
                                animator.SetBool("playerDetected", true);
                                time = 0;
                                print("[IDLE] setting new target");
                                state = BotState.Focus;
                            }
                        }
                    }
                }
                break;

            case BotState.Focus:
                Debug.Assert(playerOnFocus == null);
                Vector3 direction = playerOnFocus.transform.position - transform.position;
                bot.setAgentSpeed(4);
                if (direction.magnitude <= playerRange)
                {
                    print("TOUCH");
                    animator.SetBool("closeEnough", true);
                    bot.resetCurrentPath();
                    state = BotState.Attacking;
                    break;
                }
                if (isInRaycast(direction, "[Focus]"))
                {
                    if (time > minTimeBeforeUpdateTarget)
                    {
                        bot.resetCurrentPath(); // because you can't set a path during another path
                        bot.setNewDestination(playerOnFocus.transform.position);
                        time = 0;
                        print("[FOCUS] setting new target");
                    }
                    else
                    {
                        Debug.Log("Rotating");
                    }
                }

                if (time > timeBeforeUnfocusPlayer)
                {
                    print("[FOCUS] Oh i lost player");
                    state = BotState.Idle;
                }
                break;
            case BotState.Attacking:
                //TODO faire le raycyast pour arrêter de tapper
                animator.SetBool("closeEnough", true);
                Debug.Log("[Attack]");
                Vector3 direction_attack = playerOnFocus.transform.position - transform.position;
                if (direction_attack.magnitude > playerRange)
                {
                    animator.SetBool("closeEnough", false);
                    state = BotState.Idle;
                    break;
                }
                if (!isInRaycast(direction_attack, "[Attack]"))
                {
                    Debug.Log("Unfocussing");
                }
                if (!isInViewAngle(direction_attack))
                {
                    print("Need a rotate man");
                    state = BotState.Idle;

                }
                Quaternion rot = Quaternion.LookRotation(direction_attack);
                this.transform.rotation = Quaternion.Slerp(transform.rotation,rot, 0.8f);
                break;
            default:
                break;
        }
        time += Time.deltaTime;

    }


    /*void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

            if (hitCollider.gameObject.tag == "Player")
            {
                Vector3 direction = hitCollider.gameObject.transform.position - transform.position;

                float angle = Vector3.Angle(transform.forward, direction);
                if (angle < viewAngle)
                {
                    RaycastHit hit;
                    // Does the ray intersect any objects excluding the player layer
                    int layerMask = 1 << 3 | 1 << 6;
                    layerMask = ~layerMask;
                    Vector3 ray_src = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                    if (!Physics.Raycast(ray_src, direction, out hit, direction.magnitude + 1, layerMask))
                    {
                        Debug.DrawRay(ray_src, direction, Color.green);
                        Debug.Log("Did Hit");
                        if (direction.magnitude < 1)
                        {
                            print("TOUCH");
                            animator.SetBool("closeEnough", true);
                            bot.resetCurrentPath();
                            return;
                        }
                        if (!animator.GetBool("playerDetected") || time > minTimeBeforeUpdateTarget)
                        {
                            bot.resetCurrentPath();
                            bot.setNewDestination(hitCollider.gameObject.transform.position);
                            animator.SetBool("playerDetected",true);
                            time = 0;
                            print("setting new target");
                        }
                    }
                    else
                    {
                        Debug.DrawRay(ray_src, direction, Color.red);
                        Debug.Log("Did not Hit");
                        animator.SetBool("closeEnough", false);
                    }

                }
            }
        }
        if (animator.GetBool("playerDetected"))
        {
            time += Time.deltaTime;
            
        }

    }*/
}
