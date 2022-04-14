using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    public GameObject projectile;
    public GameObject launchStart;
    public int ammoCount = 10;
    public int timeBetweenShots = 1;
    public float launchVelocity = 100f;
    public float projectileLifetime = 5;
    public Text ammoText;
    private bool _canShoot = true;


    void Start()
    {
        ammoText.text = ammoCount.ToString();
    }

    IEnumerator Fire()
    {
        var ball = Instantiate(projectile, launchStart.transform.position, launchStart.transform.rotation * projectile.transform.rotation);
        ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, -launchVelocity));
        Destroy(ball, projectileLifetime);
        yield return new WaitForSeconds(timeBetweenShots);
        _canShoot = true;
        
    }

    public void OnShoot()
    {
        if (_canShoot && ammoCount > 0)
        {
            _canShoot = false;
            AddAmmo(-1);
            StartCoroutine(Fire());
        }
    }

    public void AddAmmo(int amount)
    {
        ammoCount += amount;
        ammoText.text = ammoCount.ToString();
    }
}