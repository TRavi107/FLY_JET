using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FJ
{
    public class CombatActions : MonoBehaviour
    {
        public Transform throwPos;
        public GameObject mainBody;
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

        public void Dead()
        {
            mainBody.SetActive(false);
            GameManager.singleton.KillCount();
            GameManager.singleton.AddScore(20);
            mainBody.transform.SetParent(null);
        }

        public void FallSound()
        {
            AudioManager.instance.Play("Fall");
        }
    }
}
