using System.Collections;
using Fusion;
using UnityEngine;

public class ShootFireball : NetworkBehaviour
{
    [SerializeField] private FireBall fireball;
    [SerializeField] private GameObject launchStart;
    private bool _canShoot = true;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData input))
        {
            if (input.shoot)
            {
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
        if (fb.Object != null && fb.Object.IsValid)
            Runner.Despawn(fb.Object);
    }
}