using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public int viewAngle;
    public float detectionRadius;
    private float time = 0;
    private Bot bot;
    void Start()
    {
        bot = GetComponent<Bot>();
        if(viewAngle > 90) viewAngle = 90;
        
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
            if(hitCollider.gameObject.tag == "Player"){
                print("Detected");
                Vector3 direction = hitCollider.gameObject.transform.position - transform.position;
                Vector2 direction2D = new Vector2(direction.x,direction.z);
                if(direction2D.magnitude < 1) {
                    print("TOUCH");
                    bot.resetCurrentPath();
                    return;
                }
                Vector2 fw2D = new Vector2(transform.forward.x, transform.forward.z);
                float angle = Vector2.Angle(fw2D, direction2D);
                if (angle < viewAngle){
                    if(!bot.getHasSeenPlayer() || time > 2){
                        bot.resetCurrentPath();
                        bot.setNewDestination( hitCollider.gameObject.transform.position);
                        bot.setHasSeenPlayer(true);
                        time = 0;
                        print("setting new target");
                    }else{
                        time += Time.deltaTime;
                        if(time > 10){
                            bot.setHasSeenPlayer(false);
                            print("Oh i lost player");
                        }
                    }
                    //TODO call hitCollider.gameObject.setDest()
                    //Refresh path every x time
                    //reset current path
                    print("SEEN !!!");
                }
            }
        }
    }
}
