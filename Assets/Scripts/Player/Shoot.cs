using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

namespace Sapphire
{
    public class Shoot : NetworkBehaviour
    {
        public float initialAngleCorrector = 4.3f;
        public GameObject projectile;
        public GameObject launchStart;
        public int timeBetweenShots = 1;
        public float projectileLifetime = 1000.0f;

        [Networked] public int ammoCount { get; set; }
        [Networked] private bool _canShoot { get; set; }

        private Text _ammoText;
        private GameObject _crosshair;
        private AudioManager _audioManager;
        private Camera _mainCamera;

        private void Awake()
        {
            _ammoText = GameObject.Find("AmmoCounter").GetComponent<Text>();
            _crosshair = GameObject.Find("Crosshair");
            _audioManager = FindObjectOfType<AudioManager>();
        }

        public override void Spawned()
        {
            ammoCount = 100;
            _canShoot = true;
            if (Object.HasInputAuthority)
            {
                _ammoText.text = ammoCount.ToString();
                _mainCamera = Camera.main;
                _crosshair.gameObject.SetActive(false);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData input))
            {
                if (input.shoot && input.aim)
                {
                    if (_canShoot && ammoCount > 0)
                    {
                        _audioManager.Play("ShootingBow");
                        _canShoot = false;
                        AddAmmo(-1);
                        StartCoroutine(Fire());
                    }
                }

                if (Object.HasInputAuthority)
                {
                    _crosshair.SetActive(input.aim);
                }
            }
        }

        IEnumerator Fire()
        {
            // TODO: use RPC and Runner.Spawn
            Quaternion rotation = launchStart.transform.rotation * Quaternion.Euler(new Vector3(0, 180, 0)) *
                                  Quaternion.Euler(initialAngleCorrector, 0.5f, 0);
            var ball = Instantiate(projectile, launchStart.transform.position,
                rotation);
            Destroy(ball, projectileLifetime);
            yield return new WaitForSeconds(timeBetweenShots);

            // TODO: use RPC to update networked value
            _canShoot = true;
        }

        public void AddAmmo(int amount)
        {
            ammoCount += amount;
            if (Object.HasInputAuthority)
                _ammoText.text = ammoCount.ToString();
        }

        public override void Render()
        {
            if (Object.HasInputAuthority)
            {
                var worldPos = _mainCamera.ScreenToWorldPoint(_crosshair.transform.position);

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