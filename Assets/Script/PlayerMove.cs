using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    public bool isTouchLeft;
    public bool isTouchRight;
    public int life;
    public int point;
    public float maxSpeed;
    public GameObject life1, life2, life3;
    public GameObject player;
    public GameObject gameOverSet;
    public GameObject ClearSet;
    public GameObject MenuSet;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    GameManager manager;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        manager = GetComponent<GameManager>();
        
    }

    void Update()
    {
        //미끄러지지 않고 움직임
        float h = Input.GetAxisRaw("Horizontal");
        if ((isTouchRight && h == 1)||(isTouchLeft&&h==-1))
            h = 0;
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, 0, 0) * maxSpeed * Time.deltaTime;

        transform.position = curPos + nextPos;

        //ESC키 누르면 게임종ㄹ
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        
       
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "left":
                    isTouchLeft = true;
                    break;
                case "right":
                    isTouchRight = true;
                    break;
            }
        }

        //지렁이에 닿으면 목숨 -1
        if (collision.gameObject.tag == "Enemy")
        {
            //Point

            //Deactive Item
            OnDamaged();
            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }

        //귤 닿으면 점수 +1
        if (collision.gameObject.tag == "Citrus")
        {
            UpPoint();
            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
            if (point == 45)
            {
                GameClear();
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        //좌우 경계에 걸렸을 때 떨림 X
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "left":
                    isTouchLeft = false;
                    break;
                case "right":
                    isTouchRight = false;
                    break;
            }
        }

        
    }

    void OnDamaged()
    {
        
        //Change Layer
        gameObject.layer = 11;

        life--;

        if (life == 2)
            life1.SetActive(false);
        else if (life == 1)
            life2.SetActive(false);
        else
        {
            life3.SetActive(false);
            GameOver();
        }


        //View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        Invoke("OffDamaged", 0.5f);
    }
    
    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    void UpPoint()
    {
        gameObject.layer = 11;

        if (point < 45) point += 1;
        else return; //45개 이상 받으면 점수 더이상 안올라ㄴ
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        Invoke("OffDamaged", 0.5f);
        
    }
    //게임시작 누르면 게임 Start
    public void GameStart()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }
    //게임설명 누르면 게임 설명서 나옴
    public void GameGuide()
    {
        SceneManager.LoadScene("GuideScene");
    }
    //지렁이 3개 받으면 게임오버
    public void GameOver()
    {
        gameOverSet.SetActive(true);
        Reset();
        
    }
    //귤 45개 받으면 게임 클리어
    public void GameClear()
    {
        ClearSet.SetActive(true);
        Reset();
    }
    //다시하기 누르면 게임 다시 시작
    public void GameRetry()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }
    //닫기 누르면 게임종료
    public void GameEnd()
    {
        Application.Quit();
    }
    public void GoStart()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void Menu()
    { 
        MenuSet = GameObject.Find("Canvas").transform.Find("Panel_MenuSet").transform.Find("MenuSet").gameObject;
        MenuSet.SetActive(true);
        Time.timeScale = 0;

    }
    public void MenuClose()
    {
        MenuSet.SetActive(false);
        Time.timeScale = 1;
    }
    private void Reset()
    {
        player.SetActive(false);
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] citrus = GameObject.FindGameObjectsWithTag("Citrus");
        foreach (GameObject test in enemy) { test.SetActive(false); }
        foreach (GameObject test1 in citrus) { test1.SetActive(false); }
    }


}
