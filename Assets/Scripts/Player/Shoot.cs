using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Player
{
    public class Shoot : MonoBehaviour
    {
        public GameObject projectile;
        public GameObject launchStart;
        public int ammoCount = 10;
        public int timeBetweenShots = 1;
        public float launchVelocity = 100f;
        public float projectileLifetime = 1000000000;
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
            crosshair.gameObject.SetActive(false);
        }

        IEnumerator Fire()
        {
            var ball = Instantiate(projectile, launchStart.transform.position,
                launchStart.transform.rotation * projectile.transform.rotation);
            Rigidbody arrow_rb = ball.GetComponent<Rigidbody>();
            arrow_rb.AddForce(launchStart.transform.forward * launchVelocity + launchStart.transform.up * 10.0f);
            Destroy(ball, projectileLifetime);
            yield return new WaitForSeconds(timeBetweenShots);
            _canShoot = true;
        }

        public void OnShoot() //called by player
        {
            if (_canShoot && ammoCount > 0 && _inputs.aim)
            {
                FindObjectOfType<AudioManager>().Play("ShootingBow");
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

        void OnAim(InputValue inputValue)
        {
            crosshair.gameObject.SetActive(inputValue.isPressed);
        }

        public void Update()
        {
            var worldPos = _mainCamera.ScreenToWorldPoint(crosshair.position);
            
            if (Physics.Raycast(worldPos, _mainCamera.transform.forward, out var hit, 100.0f))
            {
                var target = hit.point;
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
        //     if (Physics.Raycast(launchStart.transform.position, launchStart.transform.forward, out var hit, 50.0f))
        //     {
        //         Gizmos.color = Color.red;
        //         Gizmos.DrawSphere(hit.point, 0.1f);
        //     }
        // }
    }
}