using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public int viewAngle;
    public float detectionRadius;
    // Start is called before the first frame update
    void Start()
    {
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
                Vector2 fw2D = new Vector2(transform.forward.x, transform.forward.z);
                float angle = Vector2.Angle(fw2D, direction2D);
                if (angle < viewAngle){
                    print("SEEN !!!");
                }
            }
        }
    }
}
