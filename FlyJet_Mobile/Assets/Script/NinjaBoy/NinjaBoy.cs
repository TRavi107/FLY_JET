using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FJ
{
    public class NinjaBoy : MonoBehaviour, IPooledObj
    {
        private Animator myAnimator;
        private Rigidbody2D myBody;
        private bool facingright;
        private float lastTurned;
        private float Last_RangedAttack;
        private float Last_MeleeAttack, now;

        [SerializeField] public float speed;
        [SerializeField] private float turnTime;
        [SerializeField] private Transform eyePos;
        [SerializeField] private float Stop_Distance;
        [SerializeField] private float MeleeAttack_Duration;
        [SerializeField] private float RangedAttack_Duration;
        [SerializeField] private GameObject kunaiPrefab;
        [SerializeField] private Transform throwPos;
        [SerializeField] private bool facingRight;

        private bool turn;
        public enum State
        {
            Looking,Combat,throwk,meeleAttack
        }
        public State currentState;

        // Start is called before the first frame update
        void Awake()
        {
            myBody = GetComponent<Rigidbody2D>();
            myAnimator = GetComponent<Animator>();
            SetValues();
        }

        void SetValues()
        {
            Last_MeleeAttack = Time.time;
            Last_RangedAttack = Time.time;
            lastTurned = Time.time;
            now = Time.time;
            currentState = State.Looking;
            facingRight = true;
            
        }

        // Update is called once per frame
        void Update()
        {
            StateBehaviour();
        }
        void FixedUpdate()
        {
            now = Time.time;

            //LookAround();
            if (this.transform.position.y + 10 < GameManager.singleton.screenBottom.position.y)
            {
                gameObject.SetActive(false);
            }

        }

        void StateBehaviour()
        {
            RaycastHit2D hit = Physics2D.Raycast(eyePos.position, transform.right, 20);
            Debug.DrawRay(eyePos.position, new Vector3(transform.right.x,0.1f,0)*20, Color.red);

            switch (currentState)
            {
                case State.Looking:
                    //Debug.DrawRay(eyePos.position, transform.right*20, Color.red);
                    if(hit.collider != null && hit.collider.gameObject.CompareTag("Hero"))
                    {
                        currentState = State.Combat;
                        if(hit.collider.gameObject.transform.parent != transform.parent)
                        {
                            myAnimator.SetTrigger("throw");
                        }
                    }
                    else
                    {
                        if (now - lastTurned >= turnTime)
                        {
                            lastTurned = now;
                            Flip();
                        }
                    }

                    break;
                case State.Combat:
                    if (hit.collider == null || !hit.collider.gameObject.CompareTag("Hero"))
                    {
                        currentState = State.Looking;
                        lastTurned = now;
                        break;
                    }

                    if (hit.collider.gameObject.transform.parent == transform.parent)
                    {
                        if (GameObject.FindWithTag("Hero").GetComponent<Player>().isDead)
                        {
                            break;
                        }
                        currentState = State.meeleAttack;
                        myAnimator.SetBool("MeeleAttack", true);
                    }
                    break;

                case State.meeleAttack:
                    if (hit.collider == null || !hit.collider.gameObject.CompareTag("Hero"))
                    {
                        currentState = State.Looking;
                        myAnimator.SetBool("playerSeen", false);
                        myAnimator.SetBool("Run", false);
                        myAnimator.SetBool("MeeleAttack", false);
                        myAnimator.SetBool("attack", false);
                        lastTurned = now;
                        break;
                    }

                    if (Vector2.Distance(transform.position, hit.collider.transform.position) <= Stop_Distance)
                    {
                        myAnimator.SetBool("attack",true);
                        myAnimator.SetBool("Run", false);
                    }
                    else
                    {
                        myAnimator.SetBool("Run", true);
                        myAnimator.SetBool("attack", false);
                    }
                    break;
            }
        }

        private void Flip()
        {
            this.transform.Rotate(0, 180, 0);
            if (this.transform.rotation.y > 180)
            {
                facingRight = false;
            }
            else
            {
                facingRight = true;
            }
        }

        protected void Chase_Player(GameObject Player)
        {

        }

        public void Throw()
        {
            //Instantiate(kunaiPrefab, throwPos.position, transform.rotation);
            ObjectPooler.singleton.SpawnFromPool("Kunai", throwPos.position, transform.rotation);
            AudioManager.instance.Play("Kunai");
        }
        public void Attack()
        {
            GameManager.singleton.TakeDamage(20);
            AudioManager.instance.Play("Attack");
        }
        public void TakeDamage(float bulRot)
        {
            myAnimator.SetTrigger("dead");
            lastTurned -= 5;
        }
        public void Dead()
        {
            this.gameObject.SetActive(false);
            GameManager.singleton.KillCount();
            GameManager.singleton.AddScore(20);
            this.transform.SetParent(null);
        }

        public void FallSound()
        {
            AudioManager.instance.Play("Fall");
        }

        public void PooledObjStart()
        {
            SetValues();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Hero"))
            {
                myAnimator.SetTrigger("squeeze");
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Hero"))
            {
                StopCoroutine("MoveAndAttack");
                Flip();
                lastTurned = now;
                myAnimator.SetBool("headRun", false);
            }

        }

        IEnumerator MoveAndAttack(Vector3 pos)
        {
            myAnimator.SetBool("headRun", true);
            while (Math.Abs(transform.position.x-pos.x) <= 3)
            {
                if (facingRight)
                {
                    transform.position += Vector3.right*2 * Time.deltaTime;

                }
                else
                {
                    transform.position += Vector3.left*2* Time.deltaTime;

                }
                lastTurned = now;
                yield return null;
            }
            //Flip();
            myAnimator.SetBool("headRun", false);
        }
    }
}
