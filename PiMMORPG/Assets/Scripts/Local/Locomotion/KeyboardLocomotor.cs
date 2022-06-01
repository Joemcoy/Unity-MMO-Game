using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using PiMMORPG.Client;
using PiMMORPG.Models;

using tFramework.Factories;

namespace Scripts.Local.Locomotion
{
    using Triggers;
    using Scripts.Local.Helper;
    using Network.Requests.GameClient;

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    public class KeyboardLocomotor : NetworkTriggerBase
    {
        Animator animator;
        CharacterController controller;
        CameraRotator rotator;
        PiBaseClient client;
        public float Last;

        public bool Running = false;
        float RunState = 0f, HState = 0f, VState = 0f, SState = 0f;

        public bool Online = false;
        public float Interval = .5f;
        public float WalkSpeed = 3f, WalkBackwardSpeed = 2f, RunSpeed = 8f, RunBackwardSpeed = 6f;
        public float WalkSideSpeed = 2f, RunSideSpeed = 4f;
        public float Damping = 0.05f, Gravity = 9.10f, LerpT = 0.2f;
        public Vector3 Position = Vector3.zero;
        public Quaternion Rotation = Quaternion.identity;

        int HorizontalPara = Animator.StringToHash("Horizontal");
        int VerticalPara = Animator.StringToHash("Vertical");
        int RunPara = Animator.StringToHash("Run");

        void Start()
        {
            if (Online)
                client = PiBaseClient.Current;

            if(IsLocal)
            {
                Position = transform.position;
                Rotation = transform.rotation;
            }

            animator = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();

            if (Camera.main != null)
                rotator = Camera.main.GetComponent<CameraRotator>();
        }

        public float H, V;
        private float LH, LV;
        //public Quaternion Rotation;

        void Update()
        {
            if (!IsLoaded) return;

            if (IsLocal && (Last += Time.deltaTime) >= Interval)
            {
                Last = 0f;

                if (UIHelper.HasFieldFocused()) return;
                var Running = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                
                H = Input.GetAxis("Horizontal");
                V = Input.GetAxis("Vertical");

                if (Online && client != null && PiBaseClient.IsLoaded && client.Socket.Connected)
                {
                    var Packet = new SyncCharacterRequest();
                    Packet.Position = new Position(transform);
                    Packet.Horizontal = H;
                    Packet.Vertical = V;
                    client.Socket.Send(Packet);

                    if(this.Running != Running)
                    {
                        client.Socket.Send(new ToggleRunningRequest { Running = Running });
                    }
                }

                this.Running = Running;
            }
        }

        public float mag = 0f;
        void FixedUpdate()
        {
            if (!IsLoaded) return;

            if (IsLocal)
            {
                var Vector = (Vector3.forward * V + Vector3.right * H);
                if (Vector.sqrMagnitude > .1f)
                {
                    if (rotator != null)// && rotator.rotating)
                    {
                        var Rotation = rotator.transform.rotation;
                        Rotation.x = 0;
                        Rotation.z = 0;

                        transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, LerpT);
                    }

                    //animator.SetBool("Walking", true);
                    var Transformed = transform.TransformDirection(Vector) * CalcSpeed() * Time.deltaTime;
                    controller.Move(Transformed);

                    Position = transform.position;
                    Rotation = transform.rotation;
                }
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, Position, LerpT);
                transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, LerpT);
            }
            if (Vector3.Distance(transform.position, Position) > 10f)
                transform.position = Position;

            animator.SetFloat(HorizontalPara, MathHelper.Lerp(ref HState, H, LerpT));
            animator.SetFloat(VerticalPara, MathHelper.Lerp(ref VState, V, LerpT));
            animator.SetFloat(RunPara, MathHelper.Lerp(ref RunState, Running ? 1f : 0f, LerpT));
            controller.Move(Vector3.down * Gravity);
        }

        private float CalcSpeed()
        {
            var s = 0f;
            if (Mathf.Abs(H) + Math.Abs(V) != 0)
                if (H == 0)
                    s = V < 0 ? WalkBackwardSpeed : (Running ? RunSpeed : WalkSpeed);
                else
                    s = WalkSideSpeed;
            else
                s = 0f;
            return MathHelper.Lerp(ref SState, s, LerpT);
        }

        public void Move(float Horizontal, float Vertical, Quaternion Rotation)
        {
            LH = H = Horizontal;
            LV = V = Vertical;

            Vector3 Vector = Vector3.forward * Horizontal + Vector3.right * Vertical;

            /*float Result = 0f;
            RunState = Mathf.SmoothDamp(RunState, Running ? 1f : 0f, ref Result, Damping);

            animator.SetFloat(HorizontalPara, Vector.x);//, 0.15f, Time.deltaTime);
            animator.SetFloat(VerticalPara, Vector.z);//, 0.15f, Time.deltaTime);
            animator.SetFloat(RunPara, RunState);//, 0.15f, Time.deltaTime);

            float Speed = Vector.z > 0 ? (Running ? RunSpeed : WalkSpeed) : (Running ? RunBackwardSpeed : WalkBackwardSpeed);
            Vector3 Transformed = transform.TransformDirection(Vector) * Speed;

            Vector3 Smoth = Vector * Speed, Velocity = Vector3.zero;
            //Transformed = Vector3.SmoothDamp(Transformed, Smoth, ref Velocity, .15f);
            if (!Local || rotator != null && rotator.rotating)
                transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, .15f);
            controller.Move(Transformed * Time.deltaTime);*/
        }
    }
}