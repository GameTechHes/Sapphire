using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ShootFireball : NetworkBehaviour
{

    [SerializeField] private FireBall fireball;
    [SerializeField] private GameObject launchStart;
    private bool _canShoot = true;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    public override void FixedUpdateNetwork()
    {
         if (GetInput(out NetworkInputData input))
            {
                 if (input.shoot){
                     Debug.Log("CLICKED");
                     Attack();
                 }
            }

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
