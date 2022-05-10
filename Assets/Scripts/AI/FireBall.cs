using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public GameObject explosionEffet;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        var obj = Instantiate(explosionEffet, transform.position, transform.rotation);
        Destroy(obj, 1);

    }

}