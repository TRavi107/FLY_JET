using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FJ
{
    public class CombatActions : MonoBehaviour
    {
        public Transform throwPos;
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
    }
}
