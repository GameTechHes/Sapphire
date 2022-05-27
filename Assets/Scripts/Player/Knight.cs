using UnityEngine;

namespace Sapphire
{
    public class Knight : Player
    {
        public float aimSpeed = 4.0f;

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
                    }
                    else
                    {
                        _cameraDistance = Mathf.Lerp(_cameraDistance, 4.0f, Runner.DeltaTime * aimSpeed);
                        _cinemachine3RdPersonFollow.CameraDistance = _cameraDistance;
                        _cinemachine3RdPersonFollow.CameraSide = Mathf.Lerp(_cinemachine3RdPersonFollow.CameraSide,
                            0.5f,
                            Runner.DeltaTime * aimSpeed);
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

        public override void Spawned()
        {
            base.Spawned();
            if (Object.HasInputAuthority)
                UIManager.Instance.SetUIPosition();
        }
    }
}