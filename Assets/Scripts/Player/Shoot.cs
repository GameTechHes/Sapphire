using System.Collections;
using Fusion;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Sapphire
{
    public class Shoot : NetworkBehaviour
    {
        public float initialAngleCorrector = 4.3f;
        public Arrow projectile;
        public GameObject launchStart;
        public float timeBetweenShots = 1.0f;

        [Networked(OnChanged = nameof(OnAmmoChange))]
        public int ammoCount { get; set; }

        [Networked] private NetworkBool _canShoot { get; set; }

        private Text _ammoText;
        private GameObject _crosshair;
        private AudioManager _audioManager;
        private Camera _mainCamera;
        private Quaternion _launchRotation;

        public override void Spawned()
        {
            ammoCount = 100;

            _canShoot = true;

            if (Object.HasInputAuthority)
            {
                _ammoText = GameObject.Find("AmmoCounter").GetComponent<Text>();
                _ammoText.text = ammoCount.ToString();
                _mainCamera = Camera.main;
                _crosshair = GameObject.Find("Crosshair");
                _crosshair.gameObject.SetActive(false);
                _audioManager = FindObjectOfType<AudioManager>();
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
                        if (Object.HasInputAuthority)
                        {
                            _audioManager.Play("ShootingBow");
                            Quaternion rotation = launchStart.transform.rotation *
                                                  Quaternion.Euler(new Vector3(0, 180, 0)) *
                                                  Quaternion.Euler(initialAngleCorrector, 0, 0);
                            RPC_SpawnArrow(launchStart.transform.position, rotation);
                            StartCoroutine(Fire());
                        }

                        _canShoot = false;
                        ammoCount -= 1;
                    }
                }

                if (Object.HasInputAuthority)
                {
                    _crosshair.SetActive(input.aim);
                }
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        void RPC_SpawnArrow(Vector3 position, Quaternion rotation)
        {
            Runner.Spawn(projectile, position, rotation, null,
                (runner, o) => { o.GetComponent<Arrow>().InitNetworkState(); });
        }

        IEnumerator Fire()
        {
            yield return new WaitForSeconds(timeBetweenShots);
            RPC_SetCanShoot(true);
        }

        public static void OnAmmoChange(Changed<Shoot> changed)
        {
            changed.Behaviour.OnAmmoChange();
        }

        private void OnAmmoChange()
        {
            if (Object.HasInputAuthority)
                _ammoText.text = ammoCount.ToString();
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_SetCanShoot(NetworkBool canShoot)
        {
            _canShoot = canShoot;
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_AddAmmo(int amount)
        {
            ammoCount += amount;
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

        private void OnDrawGizmos()
        {
            if (Physics.Raycast(launchStart.transform.position, launchStart.transform.forward, out var hit, 100.0f))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(hit.point, 0.1f);
            }
        }
    }
}