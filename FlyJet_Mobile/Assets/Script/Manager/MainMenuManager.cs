using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Transform[] wayPoints;
    public GameObject HeroPrefab;
    public int speed;
    private Transform targetPos;
    private bool shooting;
    // Start is called before the first frame update
    void Start()
    {
        targetPos = SetWayPoint();
        shooting = false;
    }

    // Update is called once per frame
    void Update()
    {


        if (Vector2.Distance(HeroPrefab.transform.position, targetPos.position) >= 1)
        {
            HeroPrefab.transform.position = Vector2.MoveTowards(HeroPrefab.transform.position, 
            targetPos.position, speed * Time.deltaTime);
            if (HeroPrefab.transform.position.x < targetPos.transform.position.x)
            {
                HeroPrefab.transform.rotation = Quaternion.Euler(0,0,0);
            }
            else
            {
                HeroPrefab.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else if(!shooting)
        {
            StartCoroutine("Flip");
            shooting = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            targetPos.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    IEnumerator ShootAndLook()
    {
        yield return new WaitForSeconds(1);
        HeroPrefab.GetComponent<Animator>().SetTrigger("shoot");
        yield return new WaitForSeconds(1);
        targetPos= SetWayPoint();
        shooting = false;
    }
    IEnumerator Flip()
    {
        yield return new WaitForSeconds(1);
        int flipCount = Random.Range(0, 3);
        for (int i = 0; i < flipCount; i++)
        {
            HeroPrefab.transform.Rotate(0, 180, 0);
            yield return new WaitForSeconds(1);
        }
        StartCoroutine("ShootAndLook");
    }


    Transform SetWayPoint()
    {
        int posIndex = Random.Range(0, wayPoints.Length);
        return wayPoints[posIndex];
    }
    public void Play()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void Setting()
    {

    }

    public void About()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
