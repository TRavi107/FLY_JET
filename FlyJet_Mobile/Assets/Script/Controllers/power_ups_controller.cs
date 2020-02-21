using UnityEngine;

namespace FJ
{
    public class power_ups_controller : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == GameObject.FindWithTag("Hero"))
            {
                if (this.gameObject == GameObject.FindWithTag("jetpack"))
                {
                    GameManager.singleton.fuel = 100;
                    GameManager.singleton.UpdateFuelTxt();
                    AudioManager.instance.Play("Jet");
                }
                else if (this.gameObject == GameObject.FindWithTag("coin"))
                {
                    GameManager.singleton.CollectCoins();
                }
                this.gameObject.SetActive(false);
                this.transform.SetParent(null);
            }
        }
        void Update()
        {
            if (this.transform.position.y + 10 < GameManager.singleton.screenBottom.position.y)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
