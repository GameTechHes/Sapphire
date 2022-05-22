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
        public int timeBetweenShots = 1;

        [Networked(OnChanged = nameof(OnAmmoChange))] public int ammoCount { get; set; }
        [Networked] private NetworkBool _canShoot { get; set; }

        private Text _ammoText;
        private GameObject _crosshair;
        private AudioManager _audioManager;
        private Camera _mainCamera;
        private Quaternion _launchRotation;

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
                        if (Object.HasInputAuthority)
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
            Quaternion rotation = transform.rotation * Quaternion.Euler(new Vector3(0, 180, 0)) *
                                  Quaternion.Euler(initialAngleCorrector, 0.5f, 0);
            Runner.Spawn(projectile, launchStart.transform.position, rotation,
                Object.InputAuthority, (runner, obj) => { obj.GetComponent<Arrow>().InitNetworkState(); });

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

        public void AddAmmo(int amount)
        {
            ammoCount += amount;
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_AddAmmo(int amount)
        {
            AddAmmo(amount);
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
                    _launchRotation = rotation;
                }
                else
                {
                    var rotation = Quaternion.LookRotation(_mainCamera.transform.forward);
                    _launchRotation = rotation;
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