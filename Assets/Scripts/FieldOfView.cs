using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public int viewAngle = 30;
    public float detectionRadius = 2;
    public float minTimeBeforeUpdateTarget = 2;
    public float timeBeforeUnfocusPlayer = 10;
    private float time = 0;
    private Bot bot;
    void Start()
    {
        bot = GetComponent<Bot>();
        if (viewAngle > 90) viewAngle = 90;

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, detectionRadius);
    }
    // Update is called once per frame
    void Update()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Player")
            {
                Vector3 direction = hitCollider.gameObject.transform.position - transform.position;
                if (direction.magnitude < 1)
                {
                    print("TOUCH");
                    bot.resetCurrentPath();
                    return;
                }
                float angle = Vector2.Angle(transform.forward, direction);
                if (angle < viewAngle)
                {
                    RaycastHit hit;
                    // Does the ray intersect any objects excluding the player layer
                    int layerMask = 1<<7;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
                    {
                        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                        Debug.Log("Did Hit");
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                        Debug.Log("Did not Hit");
                    }
                    if (!bot.getHasSeenPlayer() || time > minTimeBeforeUpdateTarget)
                    {
                        bot.resetCurrentPath();
                        bot.setNewDestination(hitCollider.gameObject.transform.position);
                        bot.setHasSeenPlayer(true);
                        time = 0;
                        print("setting new target");
                    }
                    print("SEEN !!!");
                }
                else
                {

                }
            }
        }
        if (bot.getHasSeenPlayer())
        {
            time += Time.deltaTime;
            if (time > timeBeforeUnfocusPlayer)
            {
                bot.setHasSeenPlayer(false);
                print("Oh i lost player");
            }
        }
    }
}
