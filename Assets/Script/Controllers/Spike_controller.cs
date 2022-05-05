using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FJ
{
    public class Spike_controller : MonoBehaviour, IPooledObj
    {
        private Transform player;
        public float speed, Fall_dis;
        private bool fall, go_down = false;


        void IStart()
        {
            if (transform.position.x - gameObject.GetComponent<BoxCollider2D>().size.x / 2 > transform.parent.position.x + transform.GetComponentInParent<BoxCollider2D>().size.x / 2)
            {
                this.gameObject.SetActive(false);
            }

            player = GameObject.FindWithTag("Hero").transform;
            if (Random.Range(0, 10) == 1)
            {
                fall = true;
            }

        }


        void Update()
        {
            if (fall)
            {
                if (Mathf.Abs(transform.position.y - player.position.y) < Fall_dis)
                {
                    go_down = true;
                }
            }

            if (go_down)
            {
                transform.parent = null;
                transform.Translate(Vector2.down * Time.deltaTime * speed);
            }
            if (this.transform.position.y + 10 < GameManager.singleton.screenBottom.position.y)
            {
                gameObject.SetActive(false);
            }

        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == GameObject.FindWithTag("Hero"))
            {
                GameManager.singleton.TakeDamage(10);
                this.gameObject.SetActive(false);
                this.transform.SetParent(null);
            }
        }

        public void PooledObjStart()
        {
            IStart();
        }
    }
}
