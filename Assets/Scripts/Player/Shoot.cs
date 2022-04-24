using System;
using System.Collections;
using Cinemachine;
using StarterAssets;
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
    public Transform crosshair;
    
    private bool _canShoot = true;
    private Camera _mainCamera;
    private StarterAssetsInputs _inputs;


    void Start()
    {
        ammoText.text = ammoCount.ToString();
        _mainCamera = Camera.main;
        _inputs = GetComponent<StarterAssetsInputs>();
    }

    IEnumerator Fire()
    {
        var ball = Instantiate(projectile, launchStart.transform.position,
            launchStart.transform.rotation * projectile.transform.rotation);
        ball.GetComponent<Rigidbody>()
            .AddForce(launchStart.transform.forward * launchVelocity + launchStart.transform.up * 10.0f);
        Destroy(ball, projectileLifetime);
        yield return new WaitForSeconds(timeBetweenShots);
        _canShoot = true;
    }

    public void OnShoot()
    {
        if (_canShoot && ammoCount > 0 && _inputs.aim)
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

    public void Update()
    {
        var worldPos = _mainCamera.ScreenToWorldPoint(crosshair.position);
        var camRayHits = Physics.RaycastAll(worldPos, _mainCamera.transform.forward, 100.0f);
        if (camRayHits.Length >= 1)
        {
            var target = camRayHits[0].point;
            var direction = (target - launchStart.transform.position).normalized;
            var rotation = Quaternion.LookRotation(direction);
            launchStart.transform.rotation = rotation;
        }
        else
        {
            var rotation = Quaternion.LookRotation(_mainCamera.transform.forward);
            launchStart.transform.rotation = rotation;
        }
    }

    // private void OnDrawGizmos()
    // {
    //     var elems = Physics.RaycastAll(launchStart.transform.position, launchStart.transform.forward, 50.0f);
    //     if (elems.Length >= 1)
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawSphere(elems[0].point, 0.1f);
    //     }
    // }
}