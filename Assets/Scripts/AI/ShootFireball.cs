using System.Collections;
using Fusion;
using UnityEngine;

public class ShootFireball : NetworkBehaviour
{
    [SerializeField] private FireBall fireball;
    [SerializeField] private GameObject launchStart;
    private bool _canShoot = true;
    public int maxShoot = 3;
    private int currentShoot = 0;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData input))
        {
            if (input.shoot && Object.HasStateAuthority)
            {
                Attack();
            }
        }
    }

    public void Attack()
    {
        if (_canShoot)
        {
            currentShoot++;
            if (currentShoot == maxShoot)
            {
                _canShoot = false;
                currentShoot = 0;
            }
            StartCoroutine(SendSpell());
        }
    }

    IEnumerator SendSpell()
    {
        FireBall fb = Runner.Spawn(fireball, launchStart.transform.position, transform.rotation);
        if (fb)
        {
            Rigidbody rb = fb.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * 20;

            yield return new WaitForSeconds(3);
            _canShoot = true;
            yield return new WaitForSeconds(2);
            if (fb.Object != null && fb.Object.IsValid)
                Runner.Despawn(fb.Object);
        }
    }
}