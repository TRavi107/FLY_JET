using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FJ
{
    public class BulletController : MonoBehaviour,IPooledObj
    {
        [SerializeField] private float speed;

        void IStart()
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        }

        void Update()
        {
            if (this.transform.position.y + 10 < GameManager.singleton.screenBottom.position.y)
            {
                gameObject.SetActive(false);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Hero")
            {
                GameManager.singleton.TakeDamage(10);
                this.gameObject.SetActive(false);
                this.transform.SetParent(null);
            }
            if (other.gameObject.tag == "Ninja_boy")
            {
                this.gameObject.SetActive(false);
                this.transform.SetParent(null);
                other.gameObject.GetComponent<NinjaBoy>().TakeDamage(this.gameObject.transform.rotation.y);
            }
        }

        public void PooledObjStart()
        {
            IStart();
        }
    }
}
