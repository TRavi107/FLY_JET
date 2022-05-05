using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace FJ
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreTxt;
        [SerializeField] private float fuelDecreaserateS;
        [SerializeField] public string[] platforms;
        [SerializeField] public string[] power_ups;
        public GameObject player;
        [SerializeField] private Transform spawnPos;
        [SerializeField] public Transform screenLeft;
        [SerializeField] public Transform screenRight;
        [SerializeField] public Transform screenBottom;
        [SerializeField] public Joystick joystick;
        public int score;
        private float health;
        [SerializeField] private BarController fuelBar;
        [SerializeField] private BarController healthBar;
        [SerializeField] private TextMeshProUGUI coinsTxt;
        [SerializeField] private TextMeshProUGUI killsTxt;
        public int coinsCollected;
        public int killsCount;
        public float fuel;
        public static GameManager singleton;
        public GameObject pauseMenu;
        public GameObject bloodEffect;
        public GameObject ground;
        public GameStats tempostats;
        public bool isDead;

        // Start is called before the first frame update

        void Awake()
        {
            singleton = this;

        }
        void Start()
        {
            for (int i = 0; i < 4; i++)
            {
                SpawnPlatforms();
            }
            player = GameObject.FindWithTag("Hero");
            health = 100;
            healthBar.SetMaxAmount(100);
            fuelBar.SetMaxAmount(100);
            coinsCollected = 0;
            killsCount = 0;
            tempostats.coinsCount = 0;
            tempostats.killsCount = 0;
            tempostats.score = 0;
            isDead = false;
            player.GetComponent<Player>().canFly = true;
            pauseMenu.gameObject.SetActive(false);
            AudioManager.instance.Play("HappyTune");
            AudioManager.instance.Stop("benSound");
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector2.Distance(player.transform.position, spawnPos.position) < 10)
            {
                SpawnPlatforms();
            }
            if (health <= 0 )
            {
                player.GetComponent<Player>().isDead = true;
            }
            if (fuel <= 0)
            {
                player.GetComponent<Player>().canFly = false;
            }
            else
            {
                player.GetComponent<Player>().canFly = true;
            }

            if (fuel > 100)
            {
                fuel = 100;
                UpdateFuelTxt();
            }
            if (health > 100)
            {
                health = 100;
                healthBar.SetAmount(health);
            }
        }

        public void AddScore(int amount)
        {
            score += amount;
            fuel += 5;
            UpdateFuelTxt();
            scoreTxt.text = score.ToString();
        }
        public void CollectCoins()
        {
            coinsCollected += 1;
            coinsTxt.text = coinsCollected.ToString();
            AddScore(10);
            AudioManager.instance.Play("Coin");
        }
        public void KillCount()
        {
            killsCount += 1;
            killsTxt.text = killsCount.ToString();
            AddScore(10);
        }

        public void TakeDamage(int amount)
        {
            health -= amount;
            if (health < 0 )
            {
                health = 0;
            }
            healthBar.SetAmount(health);
            AudioManager.instance.Play("Hurt");
            GameObject effectobj = Instantiate(bloodEffect, player.transform);
            effectobj.GetComponent<ParticleSystem>().Play();
            EZCameraShake.CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, 1f);
            //StartCoroutine(blinkEffect());
        }

        IEnumerator blinkEffect()
        {
            float[] aValue = { 255f, 255 / 2f, 0f };
            int index = 0;
            for (int i = 0; i < 7; i++)
            {
                Color tmp = player.GetComponent<SpriteRenderer>().color;
                tmp.a = aValue[index];
                player.GetComponent<SpriteRenderer>().color = tmp;
                index++;
                if (index > 2)
                    index = 0;

                print(player.GetComponent<SpriteRenderer>().color);
                yield return new WaitForSeconds(1f);
            }
        }
        public void UpdateFuel()
        {
            fuel -= fuelDecreaserateS;
            UpdateFuelTxt();
        }
        public void UpdateFuelTxt()
        {
            if (fuel <= 0)
            {
                fuel = 0;
            }
            fuelBar.SetAmount(fuel);
        }

        private void SpawnPlatforms()
        {
            Vector3 _spawnPos = GetSpawnPos();
            //Instantiate(platforms[UnityEngine.Random.Range(0, platforms.Length)],_spawnPos,Quaternion.identity);
            ObjectPooler.singleton.SpawnFromPool(platforms[UnityEngine.Random.Range(0, platforms.Length)], _spawnPos, Quaternion.identity);
        }

        private Vector3 GetSpawnPos()
        {
            Vector3 pos = spawnPos.position;
            pos = new Vector3(UnityEngine.Random.Range(screenLeft.position.x, screenRight.position.x), pos.y);
            spawnPos.position = new Vector3(spawnPos.position.x, spawnPos.position.y + UnityEngine.Random.Range(3.0f, 5.0f), 0);
            return pos;
        }
        public void Shoot()
        {
            GameObject.FindWithTag("Hero").GetComponent<Player>().Shot();
        }
        public void Pause()
        {
            pauseMenu.SetActive(true);
            AudioManager.instance.Stop("HappyTune");
            AudioManager.instance.Play("benSound");
            Time.timeScale = 0f;
        }

        public void LoadGameOverScene()
        {
            tempostats.score = score;
            tempostats.coinsCount = coinsCollected;
            tempostats.killsCount = killsCount;
            SceneManager.LoadScene("GameOver");
        }
    }
}
