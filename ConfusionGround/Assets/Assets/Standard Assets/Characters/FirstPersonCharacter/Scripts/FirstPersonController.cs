namespace UnityStandardAssets.Characters.FirstPerson
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityStandardAssets.CrossPlatformInput;
    using UnityStandardAssets.Utility;

    [RequireComponent(typeof(CharacterController)), RequireComponent(typeof(AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        private AudioSource m_AudioSource;
        private Camera m_Camera;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        [SerializeField]
        private AudioClip[] m_FootstepSounds;
        [SerializeField]
        private FOVKick m_FovKick = new FOVKick();
        [SerializeField]
        private float m_GravityMultiplier;
        [SerializeField]
        private CurveControlledBob m_HeadBob = new CurveControlledBob();
        private Vector2 m_Input;
        [SerializeField]
        private bool m_IsWalking;
        private bool m_Jump;
        [SerializeField]
        private LerpControlledBob m_JumpBob = new LerpControlledBob();
        private bool m_Jumping;
        [SerializeField]
        private AudioClip m_JumpSound;
        [SerializeField]
        private float m_JumpSpeed;
        [SerializeField]
        private AudioClip m_LandSound;
        [SerializeField]
        private MouseLook m_MouseLook;
        private Vector3 m_MoveDir = Vector3.zero;
        private float m_NextStep;
        private Vector3 m_OriginalCameraPosition;
        private bool m_PreviouslyGrounded;
        [SerializeField]
        private float m_RunSpeed;
        [Range(0f, 1f), SerializeField]
        private float m_RunstepLenghten;
        private float m_StepCycle;
        [SerializeField]
        private float m_StepInterval;
        [SerializeField]
        private float m_StickToGroundForce;
        [SerializeField]
        private bool m_UseFovKick;
        [SerializeField]
        private bool m_UseHeadBob;
        [SerializeField]
        private float m_WalkSpeed;
        private float m_YRotation;

        public void _init()
        {
            if ((this.m_Camera != null) && (this.m_MouseLook != null))
            {
                this.m_MouseLook.Init(base.transform, this.m_Camera.transform);
            }
        }

        private void FixedUpdate()
        {
            float num;
            RaycastHit hit;
            this.GetInput(out num);
            Vector3 normalized = (Vector3) ((base.transform.forward * this.m_Input.y) + (base.transform.right * this.m_Input.x));
            Physics.SphereCast(base.transform.position, this.m_CharacterController.radius, Vector3.down, out hit, this.m_CharacterController.height / 2f);
            normalized = Vector3.ProjectOnPlane(normalized, hit.normal).normalized;
            this.m_MoveDir.x = normalized.x * num;
            this.m_MoveDir.z = normalized.z * num;
            if (this.m_CharacterController.isGrounded)
            {
                this.m_MoveDir.y = -this.m_StickToGroundForce;
                if (this.m_Jump)
                {
                    this.m_MoveDir.y = this.m_JumpSpeed;
                    this.PlayJumpSound();
                    this.m_Jump = false;
                    this.m_Jumping = true;
                }
            }
            else
            {
                this.m_MoveDir += (Vector3) ((Physics.gravity * this.m_GravityMultiplier) * Time.fixedDeltaTime);
            }
            this.m_CollisionFlags = this.m_CharacterController.Move((Vector3) (this.m_MoveDir * Time.fixedDeltaTime));
            this.ProgressStepCycle(num);
            this.UpdateCameraPosition(num);
        }

        private void GetInput(out float speed)
        {
            float axis = CrossPlatformInputManager.GetAxis("Horizontal");
            float y = CrossPlatformInputManager.GetAxis("Vertical");
            bool isWalking = this.m_IsWalking;
            this.m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
            speed = !this.m_IsWalking ? this.m_RunSpeed : this.m_WalkSpeed;
            this.m_Input = new Vector2(axis, y);
            if (this.m_Input.sqrMagnitude > 1f)
            {
                this.m_Input.Normalize();
            }
            if (((this.m_IsWalking != isWalking) && this.m_UseFovKick) && (this.m_CharacterController.velocity.sqrMagnitude > 0f))
            {
                base.StopAllCoroutines();
                base.StartCoroutine(this.m_IsWalking ? this.m_FovKick.FOVKickDown() : this.m_FovKick.FOVKickUp());
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
            if ((this.m_CollisionFlags != CollisionFlags.Below) && ((attachedRigidbody != null) && !attachedRigidbody.isKinematic))
            {
                attachedRigidbody.AddForceAtPosition((Vector3) (this.m_CharacterController.velocity * 0.1f), hit.point, ForceMode.Impulse);
            }
//			print (hit.gameObject.name);
			if (hit.gameObject.tag.Equals("door")) {
//				GameObject obj = hit.transform.parent.transform.Find ("Door_Slider").gameObject;
				if (hit.gameObject.GetComponent<Animator> ()) {
					Animator ani = hit.gameObject.GetComponent<Animator> ();
					ani.SetTrigger ("Open");
				}
			}
        }

        private void PlayFootStepAudio()
        {
            if (this.m_CharacterController.isGrounded)
            {
                int index = UnityEngine.Random.Range(1, this.m_FootstepSounds.Length);
                this.m_AudioSource.clip = this.m_FootstepSounds[index];
                this.m_AudioSource.PlayOneShot(this.m_AudioSource.clip);
                this.m_FootstepSounds[index] = this.m_FootstepSounds[0];
                this.m_FootstepSounds[0] = this.m_AudioSource.clip;
            }
        }

        private void PlayJumpSound()
        {
            this.m_AudioSource.clip = this.m_JumpSound;
            this.m_AudioSource.Play();
        }

        private void PlayLandingSound()
        {
            this.m_AudioSource.clip = this.m_LandSound;
            this.m_AudioSource.Play();
            this.m_NextStep = this.m_StepCycle + 0.5f;
        }

        private void ProgressStepCycle(float speed)
        {
            if ((this.m_CharacterController.velocity.sqrMagnitude > 0f) && ((this.m_Input.x != 0f) || (this.m_Input.y != 0f)))
            {
                this.m_StepCycle += (this.m_CharacterController.velocity.magnitude + (speed * (!this.m_IsWalking ? this.m_RunstepLenghten : 1f))) * Time.fixedDeltaTime;
            }
            if (this.m_StepCycle > this.m_NextStep)
            {
                this.m_NextStep = this.m_StepCycle + this.m_StepInterval;
                this.PlayFootStepAudio();
            }
        }

        private void RotateView()
        {
            this.m_MouseLook.LookRotation(base.transform, this.m_Camera.transform);
        }

        private void Start()
        {
            this.m_CharacterController = base.GetComponent<CharacterController>();
            this.m_Camera = Camera.main;
            this.m_OriginalCameraPosition = this.m_Camera.transform.localPosition;
            this.m_FovKick.Setup(this.m_Camera);
            this.m_HeadBob.Setup(this.m_Camera, this.m_StepInterval);
            this.m_StepCycle = 0f;
            this.m_NextStep = this.m_StepCycle / 2f;
            this.m_Jumping = false;
            this.m_AudioSource = base.GetComponent<AudioSource>();
            this.m_MouseLook.Init(base.transform, this.m_Camera.transform);
        }

        private void Update()
        {
            this.RotateView();
            if (!this.m_Jump)
            {
                this.m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
            if (!this.m_PreviouslyGrounded && this.m_CharacterController.isGrounded)
            {
                base.StartCoroutine(this.m_JumpBob.DoBobCycle());
                this.PlayLandingSound();
                this.m_MoveDir.y = 0f;
                this.m_Jumping = false;
            }
            if ((!this.m_CharacterController.isGrounded && !this.m_Jumping) && this.m_PreviouslyGrounded)
            {
                this.m_MoveDir.y = 0f;
            }
            this.m_PreviouslyGrounded = this.m_CharacterController.isGrounded;
        }

        private void UpdateCameraPosition(float speed)
        {
            if (this.m_UseHeadBob)
            {
                Vector3 localPosition;
                if ((this.m_CharacterController.velocity.magnitude > 0f) && this.m_CharacterController.isGrounded)
                {
                    this.m_Camera.transform.localPosition = this.m_HeadBob.DoHeadBob(this.m_CharacterController.velocity.magnitude + (speed * (!this.m_IsWalking ? this.m_RunstepLenghten : 1f)));
                    localPosition = this.m_Camera.transform.localPosition;
                    localPosition.y = this.m_Camera.transform.localPosition.y - this.m_JumpBob.Offset();
                }
                else
                {
                    localPosition = this.m_Camera.transform.localPosition;
                    localPosition.y = this.m_OriginalCameraPosition.y - this.m_JumpBob.Offset();
                }
                this.m_Camera.transform.localPosition = localPosition;
            }
        }
    }
}
