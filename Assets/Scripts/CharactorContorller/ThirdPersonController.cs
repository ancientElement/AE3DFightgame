using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace AEActionSystem
{
    public class ThirdPersonController : MonoBehaviour,IThirdPersonrController
    {
        [Header("Player")] [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        // [Space(10)] [Tooltip("The height the player can jump")]
        // public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        // [Space(10)]
        // [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        // public float JumpTimeout = 0.50f;
        //
        // [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        // public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        private bool PreGrounded = true;

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
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        // private float _jumpTimeoutDelta;
        // private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;

        // private int _animIDGrounded;
        // private int _animIDJump;
        // private int _animIDFreeFall;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }

        private int RotateToCameraForwadCount;
        private float CameraForwad;

        public AEActionConfigTrigger OnGoundedAction; //落地时调用

        public void Init()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }

            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            // _controller.detectCollisions = false;//不发生碰撞
            // _controller.isTrigger = true;
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            // _jumpTimeoutDelta = JumpTimeout;
            // _fallTimeoutDelta = FallTimeout;
        }

        public void OnUpdate(float delta)
        {
            _hasAnimator = TryGetComponent(out _animator);

            GravityUpdate(delta);
            GroundedCheck();
            // ContorlMove(delta);
        }

        public void OnLateUpdate(float delta)
        {
            CameraRotation(delta);
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            // _animIDGrounded = Animator.StringToHash("Grounded");
            // _animIDJump = Animator.StringToHash("Jump");
            // _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
            if (Grounded && !PreGrounded)
            {
                // Debug.Log("落地");
                // OnGroundAction;
                OnGoundedAction.CustomAction?.Invoke(OnGoundedAction.Config);
            }

            PreGrounded = Grounded;
            // update animator if using character
            if (_hasAnimator)
            {
                // _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation(float delta)
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by delta;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : delta;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void GravityUpdate(float delta)
        {
            if (Grounded)
            {
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * delta;
            }
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

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center),
                        FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center),
                    FootstepAudioVolume);
            }
        }

        //方向键控制旋转位移
        public void ContorlMoveRotate(float delta, MotionInfo info)
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed * info.MoveSpeedRate : MoveSpeed * info.MoveSpeedRate;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed =
                new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    delta * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, delta * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;


            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                //计算相对于相机的输入
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;

                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation,
                    ref _rotationVelocity, RotationSmoothTime);

                //旋转模型
                if (info.ApplayRotate)
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            //目标方向
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            // Debug.DrawLine(transform.position, transform.position + targetDirection, Color.red);

            // move the player
            if (info.UseRootMotionMove&&info.RMVelocity.y != 0) _verticalVelocity = info.RMVelocity.y;
            // MoveModel(delta, targetDirection, _speed, _verticalVelocity);
            _controller.Move(targetDirection.normalized * (_speed * delta) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * delta);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        //动作控制旋转位移
        public void ActionMoveRotate(float delta, MotionInfo info)
        {
            //角色转向镜头前方
            if (info.RotateToCameraForward)
            {
                //是否允许在动作中旋转
                if (info.ApplayRotate)
                {
                    RotateToCameraForwadCount = 0;
                }

                if (RotateToCameraForwadCount <= 0)
                {
                    CameraForwad = _mainCamera.transform.eulerAngles.y;
                    RotateToCameraForwadCount = 1;
                }

                //计算相对于相机的输入
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, CameraForwad,
                    ref _rotationVelocity, RotationSmoothTime);

                //旋转模型
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            else
            {
                if (info.DesForwordDirection != 0)
                {
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, info.DesForwordDirection,
                        ref _rotationVelocity, RotationSmoothTime);

                    //旋转模型
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
            }

            //使用RootMotion移动
            if (info.UseRootMotionMove)
            {
                // move the player
                if (info.RMVelocity.y != 0) _verticalVelocity = info.RMVelocity.y;
                _controller.Move(info.RMVelocity * info.MoveSpeedRate * delta +
                                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }
        }

        //进入动作
        public void EnterAction()
        {
            RotateToCameraForwadCount = 0;
        }
    }
}