using UnityEngine;
using UnityEngine.UI;
using Sapphire;
using Fusion;

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
    private bool isDead = false;

    private Text SbireText;

    private float wanderTimer;
    private float timer;

    [SerializeField] private float wanderRadius;
    [SerializeField] private float maxWanderTimer;
    [SerializeField] private float minWanderTimer;
    enum BotState
    {
        Idle,
        Walking,
        Focus, // Player detected
        Attacking,
        Dying,
    }

    private BotState state = BotState.Idle;

    void Start()
    {
        bot = GetComponent<Bot>();
        animator = GetComponent<Animator>();
        animator.SetBool("attack", false);
        animator.SetBool("playerDetected", false);
        if (viewAngle > 90) viewAngle = 90;

        SbireText = GameObject.Find("SbiresCounter").GetComponent<Text>();
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        if (isDead) return;
        switch (state)
        {
            case BotState.Idle:
                bot.SetAgentSpeed(2);
                animator.SetBool("attack", false);

                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject.CompareTag("Player") && hitCollider.gameObject.GetComponent<Player>().PlayerType == PlayerType.KNIGHT)
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
                timer += Time.deltaTime;
                if (timer >= wanderTimer)
                {
                    Vector3 randDirection = Random.insideUnitSphere * wanderRadius;
                    randDirection += transform.position;
                    bot.SetNewDestination(randDirection);
                }
                break;

            case BotState.Focus:
                Vector3 direction = playerOnFocus.transform.position - transform.position;
                bot.SetAgentSpeed(4);
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
                if(bot.canShoot) {
                    animator.SetBool("attack", true);
                }else{
                    animator.SetBool("attack", false);
                }
                bot.Attack();
                Quaternion rot = Quaternion.LookRotation(directionAttack);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.8f);
                break;


        }

        time += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Arrow"))
        {
            isDead = true;
            Debug.Log("Toucher");
            bot.ResetCurrentPath();
            animator.SetBool("attack", false);
            animator.SetBool("die", true);
            Destroy(this.gameObject, animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            Invoke("spawnEffet", 1.0f);

            int nbSbire = int.Parse(SbireText.text);
            nbSbire--;
            SbireText.text = nbSbire.ToString();

        }
    }

    void spawnEffet()
    {
        Debug.Log("Invoke!!!!");
        var obj = Instantiate(dieEffet, transform.position, transform.rotation) as GameObject;
        Destroy(obj, 2);
    }



}