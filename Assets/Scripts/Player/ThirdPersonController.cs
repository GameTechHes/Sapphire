using Fusion;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [OrderBefore(typeof(NetworkTransform))]
    [DisallowMultipleComponent]
    public class ThirdPersonController : NetworkTransform
    {
        [Header("Player")] [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Space(10)] [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")] public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        [Networked] private float _cinemachineTargetYaw { get; set; }
        [Networked] private float _cinemachineTargetPitch { get; set; }

        // player
        [Networked] private float _animationBlend { get; set; }
        [Networked] private float _targetRotation {get; set;}
        [Networked] private float _rotationVelocity {get; set;}
        [Networked] private float _verticalVelocity {get; set;}
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        [Networked] private float _jumpTimeoutDelta { get; set; }
        [Networked] private float _fallTimeoutDelta { get; set; }

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _localInput;
        private NetworkInputData _networkInput;
        [Networked] private float _camRotation { get; set; }

        private const float _threshold = 0.01f;
        private bool _hasAnimator;

        protected override void Awake()
        {
            base.Awake();
            CacheController();
        }

        public override void Spawned()
        {
            base.Spawned();
            CacheController();

            _hasAnimator = TryGetComponent(out _animator);
            if (Object.HasInputAuthority)
            {
                _localInput = FindObjectOfType<StarterAssetsInputs>();
                _localInput.cursorLocked = true;
            }

            AssignAnimationIDs();

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            // Caveat: this is needed to initialize the Controller's state and avoid unwanted spikes in its perceived velocity
            _controller.Move(transform.position);
        }

        private void CacheController()
        {
            if (_controller == null)
            {
                _controller = GetComponent<CharacterController>();
                Assert.Check(_controller != null,
                    $"An object with {nameof(ThirdPersonController)} must also have a {nameof(CharacterController)} component.");
            }
        }

        protected override void CopyFromBufferToEngine()
        {
            // Trick: CC must be disabled before resetting the transform state
            _controller.enabled = false;

            // Pull base (NetworkTransform) state from networked data buffer
            base.CopyFromBufferToEngine();

            // Re-enable CC
            _controller.enabled = true;
        }

        public override void Render()
        {
            base.Render();
            _hasAnimator = TryGetComponent(out _animator);
        }
        
        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData newNetworkInput))
            {
                _networkInput = newNetworkInput;
                CameraRotation();
            }

            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        public void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_networkInput.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                _cinemachineTargetYaw += _networkInput.look.x;
                _cinemachineTargetPitch += _networkInput.look.y;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
            _camRotation = _cinemachineTargetYaw;

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }


        public void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _networkInput.sprint ? SprintSpeed : MoveSpeed;

            if (_networkInput.move == Vector2.zero || _networkInput.aim) targetSpeed = 0.0f;

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Runner.DeltaTime * SpeedChangeRate);

            Vector3 inputDirection = new Vector3(_networkInput.move.x, 0.0f, _networkInput.move.y).normalized;
            
            if (_networkInput.move != Vector2.zero && !_networkInput.aim)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _camRotation;
                var tempRot = _rotationVelocity;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref tempRot,
                    RotationSmoothTime, Mathf.Infinity, Runner.DeltaTime);
                _rotationVelocity = tempRot;

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            
            if (_networkInput.aim)
            {
                _targetRotation = _camRotation;
                var tempRot = _rotationVelocity;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref tempRot,
                    RotationSmoothTime, Mathf.Infinity, Runner.DeltaTime);
                _rotationVelocity = tempRot;
            
                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (targetSpeed * Runner.DeltaTime) +
                            new Vector3(0.0f, _verticalVelocity, 0.0f) * Runner.DeltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, 1.0f);
            }
        }

        public void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }


                if (_networkInput.aim)
                {
                    _networkInput.jump = false;
                }

                // Jump
                if (_networkInput.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Runner.DeltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Runner.DeltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                // input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Runner.DeltaTime;
            }
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }
    }
}