using UnityEngine;

namespace Sapphire
{
    public class Knight : Player
    {
        public float aimSpeed = 4.0f;

        private bool PlaySoundBow = true;

        public override void Spawned()
        {
            base.Spawned();
            var spawnPt = GameObject.Find("KnightSpawnPoint");
            if (spawnPt != null)
            {
                ThirdPersonController tpc = GetComponent<ThirdPersonController>();
                if (tpc)
                    tpc.SetTeleportPosition(spawnPt.transform.position);

                Debug.Log($"Player respawned {Object.InputAuthority}");
            }
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
            if (GetInput(out NetworkInputData input))
            {
                _controller.SetBool("Aim", input.aim);
                if (Object.HasInputAuthority)
                {
                    if (input.aim)
                    {
                        _cameraDistance = Mathf.Lerp(_cameraDistance, 2.0f, Runner.DeltaTime * aimSpeed);
                        _cinemachine3RdPersonFollow.CameraDistance = _cameraDistance;
                        _cinemachine3RdPersonFollow.CameraSide = Mathf.Lerp(_cinemachine3RdPersonFollow.CameraSide,
                            1.0f,
                            Runner.DeltaTime * aimSpeed);

                        if (PlaySoundBow)
                        {
                            PlaySoundBow = false;
                            FindObjectOfType<AudioManager>().Play("AimingBow");
                        }
                    }
                    else
                    {
                        _cameraDistance = Mathf.Lerp(_cameraDistance, 4.0f, Runner.DeltaTime * aimSpeed);
                        _cinemachine3RdPersonFollow.CameraDistance = _cameraDistance;
                        _cinemachine3RdPersonFollow.CameraSide = Mathf.Lerp(_cinemachine3RdPersonFollow.CameraSide,
                            0.5f,
                            Runner.DeltaTime * aimSpeed);

                        PlaySoundBow = true;
                    }
                }
            }
            else
            {
                _cameraDistance = Mathf.Lerp(_cameraDistance, 4.0f, Runner.DeltaTime * aimSpeed);
                if (_cinemachine3RdPersonFollow != null)
                {
                    _cinemachine3RdPersonFollow.CameraDistance = _cameraDistance;
                    _cinemachine3RdPersonFollow.CameraSide = Mathf.Lerp(_cinemachine3RdPersonFollow.CameraSide, 0.5f,
                        Runner.DeltaTime * aimSpeed);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Spell"))
            {
                if (Object.HasInputAuthority)
                {
                    var ran = Random.Range(1, 4);
                    FindObjectOfType<AudioManager>().Play("Hurt_" + ran);
                    RPC_AddHealth(-FireBall.DAMAGE);
                }
            }
        }
    }
}