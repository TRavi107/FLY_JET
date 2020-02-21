using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FJ
{
    public class PlatController : MonoBehaviour, IPooledObj
    {
        [SerializeField] private float speed;
        private string[] direction = { "right", "left" };
        private string dirr;
        private Transform screenEdge_right;
        private Transform screenEdge_left;
        private Rigidbody2D myrigidbody;
        [SerializeField] public Transform myEdge_right;
        [SerializeField] public Transform myEdge_left;
        [SerializeField] public Transform myEdge_Top;
        [SerializeField] GameObject ninjaPrefab, Spikes;
        // Start is called before the first frame update
        void Awake()
        {
            screenEdge_left = GameObject.FindWithTag("ScrLeft").transform;
            screenEdge_right = GameObject.FindWithTag("ScrRight").transform;
        }
        void IStart()
        {
            int index = Random.Range(0, 2);
            myrigidbody = GetComponent<Rigidbody2D>();
            dirr = direction[index];
            speed = Random.Range(1, 2);
            if (Random.Range(1, 1) == 1)
            {
                Spawn_enemy();
            }
            if (Random.Range(1, 4) == 1)
            {
                Spawn_PowerUps();
            }
            if (Random.Range(1, 6) == 1)
            {
                Vector2 position = new Vector2((Random.Range(myEdge_left.position.x, myEdge_right.position.x)),
                                                this.transform.position.y - Spikes.GetComponent<BoxCollider2D>().size.y / 2 - gameObject.GetComponent<BoxCollider2D>().size.y / 4);

                for (int i = 0; i < Random.Range(2, 5); i++)
                {
                    position.x += Spikes.GetComponent<BoxCollider2D>().size.x / 2;
                    //Instantiate(Spikes,position,Quaternion.identity, transform);
                    ObjectPooler.singleton.SpawnFromPool("Spikes", position, Quaternion.identity, transform);
                }
            }
        }

        private void Spawn_PowerUps()
        {
            int pow_index = Random.Range(0, GameManager.singleton.power_ups.Length);
            //Instantiate(power_ups[pow_index], new Vector3(Random.Range(myEdge_right.position.x, myEdge_left.position.x), transform.position.y + 0.7f,
            //transform.position.z), Quaternion.identity, transform);
            ObjectPooler.singleton.SpawnFromPool(GameManager.singleton.power_ups[pow_index], new Vector3(Random.Range(myEdge_right.position.x, myEdge_left.position.x), transform.position.y + 0.7f,
                               transform.position.z), Quaternion.identity, transform);
        }



        // Update is called once per frame
        void Update()
        {

        }
        void FixedUpdate()
        {
            if (myEdge_left.position.x <= screenEdge_left.position.x || myEdge_right.position.x >= screenEdge_right.position.x)
            {
                speed *= -1;
            }

            this.transform.Translate(speed * Time.deltaTime, 0, 0);
            if (this.transform.position.y + 15 < GameManager.singleton.screenBottom.position.y)
            {
                gameObject.SetActive(false);
            }


        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Hero")
            {
                other.gameObject.transform.SetParent(this.transform);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag == "Hero")
            {
                other.gameObject.transform.SetParent(null);
            }
        }

        void Spawn_enemy()
        {
            //GameObject a = Instantiate(ninjaPrefab) as GameObject;
            Vector3 pos = new Vector3(Random.Range(myEdge_left.position.x,
                                    myEdge_right.position.x),
                               myEdge_Top.position.y +0.54f,
                               transform.position.z);
            GameObject a = ObjectPooler.singleton.SpawnFromPool("Ninja_Boy",pos,Quaternion.identity,this.transform);

            a.transform.SetParent(transform);
            a.transform.position = pos;
        }

        public void PooledObjStart()
        {
            IStart();
        }
    }
}
