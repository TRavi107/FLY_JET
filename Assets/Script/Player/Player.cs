using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FJ
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private CharacterController2D controller;
        [SerializeField] private Vector2 speed;
        [SerializeField] private float shootRate;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform scrLeft;
        [SerializeField] private Transform scrRight;
        [SerializeField] private Transform shotPos;
        private Joystick joystick;
        private float lastShot, now;
        private Vector2 move;
        public Animator myAnimator;
        private Rigidbody2D myRigidBody;
        public bool isDead;
        public bool canFly;
        private bool dying;
        // Start is called before the first frame update
        void Awake()
        {
            myAnimator = GetComponent<Animator>();
            myRigidBody = GetComponent<Rigidbody2D>();
            lastShot = now = Time.time;
        }
        void Start()
        {
            scrLeft = GameManager.singleton.screenLeft;
            scrRight = GameManager.singleton.screenRight;
            joystick = GameManager.singleton.joystick;
            isDead = false;
            dying = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (isDead &&!dying)
            {
                Debug.Log(isDead);
                myAnimator.SetTrigger("Death");
                dying = true;

                return;
            }
            now = Time.time;
            if (joystick.Horizontal > 0.2 || Input.GetAxis("Horizontal")>0.2)
            {
                move.x = 1;
            }
            else if (joystick.Horizontal < -0.2 || Input.GetAxis("Horizontal") < -0.2)
            {
                move.x = -1;
            }
            else
            {
                move.x = 0;
            }

            if (joystick.Vertical > 0.2 || Input.GetAxis("Vertical") > 0.2)
            {
                move.y = 1;
            }
            else
            {
                move.y = 0;
            }

            controller.Turn(move.x);
            SetAnimation();
            if (myRigidBody.velocity.y < -35)
            {
                isDead = true;
            }

        }
        void FixedUpdate()
        {
            if (!dying)
            {
                if (move != Vector2.zero)
                {
                    if (move.x < 0 && (this.transform.position.x <= scrLeft.position.x))
                    {
                        //Teleport To other side
                        transform.position = new Vector3(scrRight.position.x + 0.2f, transform.position.y);
                    }
                    else if (move.x > 0 && (this.transform.position.x >= scrRight.position.x))
                    {
                        //Teleport To other side
                        transform.position = new Vector3(scrLeft.position.x - 0.2f, transform.position.y);
                    }
                    else
                    {
                        if (!canFly)
                        {
                            move.y = 0;
                        }
                        controller.Move(move * speed * Time.deltaTime, false, false);
                    }

                }
            }
        }

        void SetAnimation()
        {
            if (myRigidBody.velocity.y > 0.01 || myRigidBody.velocity.y < -0.01)
            {
                myAnimator.SetBool("fly", true);
                GameManager.singleton.UpdateFuel();
                myAnimator.SetBool("run", false);
            }
            else
            {
                myAnimator.SetBool("fly", false);
                if (myRigidBody.velocity.x > 0.01 || myRigidBody.velocity.x < -0.01)
                {
                    myAnimator.SetBool("run", true);
                }
                else
                {
                    myAnimator.SetBool("run", false);

                }
            }
        }
        public void Shot()
        {
            if (now - lastShot >= shootRate)
            {
                lastShot = now;
                myAnimator.SetTrigger("shoot");
            }

        }

        public void ShootBullet()
        {
            //Instantiate(bulletPrefab, shotPos.position, transform.rotation);
            ObjectPooler.singleton.SpawnFromPool("Bullets", shotPos.position, transform.rotation);
            AudioManager.instance.Play("Shoot");
        }

        public void LoadGameOverScene()
        {
            GameManager.singleton.LoadGameOverScene();
        }
    }
}