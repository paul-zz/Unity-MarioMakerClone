using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Score : MonoBehaviour
{
    private float timeLeft = 300;
    private int timeLeftInt;
    public static int playerScore = 0;
    private static int playerCoin = 0;
    public GameObject timeLeftUI;
    public GameObject playerScoreUI;
    public GameObject playerCoinUI;
    void Start()
    {
        DataManagement.dataManagement.LoadData();
    }
    void Update()
    {
        timeLeft -= Time.deltaTime;
        timeLeftInt = (int)timeLeft;
        timeLeftUI.gameObject.GetComponent<Text>().text = timeLeftInt.ToString().PadLeft(3, '0');
        playerScoreUI.gameObject.GetComponent<Text>().text = playerScore.ToString().PadLeft(9, '0');
        playerCoinUI.gameObject.GetComponent<Text>().text = "×" + playerCoin.ToString().PadLeft(2, '0');
        if(timeLeft < .1f)
        {
            SceneManager.LoadScene("Prototype1");
        }
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if(trig.gameObject.name == "EndLevel")
        {
            CountScore();
        }
        if(trig.gameObject.tag == "Coin")
        {
            playerScore = playerScore + 100;
            playerCoin = playerCoin + 1;
            SoundManagerScript.PlaySound("coin");
            Destroy(trig.gameObject);
        }
        if(trig.gameObject.tag == "Mushroom")
        {
            Debug.Log("State set to super");
            ScoreDisplayerScript.showScore("1000", trig.gameObject.transform.position);
            addScore(1000);
            if(gameObject.GetComponent<PlayerMoveProt>().state == PlayerMoveProt.PlayerState.small)
            {
                gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.64f, 1.28f);
                gameObject.GetComponent<PlayerMoveProt>().state = PlayerMoveProt.PlayerState.super;
                gameObject.GetComponent<Animator>().SetBool("isSuper", true);
                SoundManagerScript.PlaySound("upgrade");
                gameObject.GetComponent<TimeManagement>().slowTime();
                gameObject.GetComponent<PlayerGrowAnimation>().playAnimation();
                gameObject.GetComponent<TimeManagement>().normalTime();
                gameObject.GetComponent<PlayerMoveProt>().buttomDistance = gameObject.GetComponent<BoxCollider2D>().size.y/2+0.05f; // 1.0f
            }
            else
            {
                SoundManagerScript.PlaySound("upgrade");
            }
            Destroy(trig.gameObject);
        }
        if(trig.gameObject.tag == "FireFlower")
        {
            ScoreDisplayerScript.showScore("1000", trig.gameObject.transform.position);
            addScore(1000);
            if(gameObject.GetComponent<PlayerMoveProt>().state != PlayerMoveProt.PlayerState.fireFlower)
            {
                gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.64f, 1.28f);
                if(gameObject.GetComponent<PlayerMoveProt>().state == PlayerMoveProt.PlayerState.small)
                {
                    gameObject.GetComponent<PlayerMoveProt>().state = PlayerMoveProt.PlayerState.fireFlower;
                    gameObject.GetComponent<Animator>().SetBool("isSuper", true);
                    gameObject.GetComponent<Animator>().SetBool("isFireFlower", true);
                    gameObject.GetComponent<PlayerGrowAnimation>().playAnimation();
                }
                else
                {
                    gameObject.GetComponent<PlayerMoveProt>().state = PlayerMoveProt.PlayerState.fireFlower;
                    gameObject.GetComponent<Animator>().SetBool("isFireFlower", true);
                }
                SoundManagerScript.PlaySound("upgrade");
                gameObject.GetComponent<TimeManagement>().slowTime();
                gameObject.GetComponent<TimeManagement>().normalTime();
                gameObject.GetComponent<PlayerMoveProt>().buttomDistance = gameObject.GetComponent<BoxCollider2D>().size.y/2+0.05f; // 1.0f
            }
            else
            {
                SoundManagerScript.PlaySound("upgrade");
            }
            Destroy(trig.gameObject);
        }
    }

    void CountScore()
    {
        Debug.Log("Data says high score is currently " + DataManagement.dataManagement.highScore);
        playerScore = playerScore + (int)timeLeft * 10;
        DataManagement.dataManagement.highScore = playerScore;
        DataManagement.dataManagement.SaveData();
        Debug.Log("Now that we have added the score to DataManagement " + DataManagement.dataManagement.highScore);
        Debug.Log(playerScore);
    }

    public static void addScore(int addNum)
    {
        playerScore += addNum;
    }

    public static void addCoin()
    {
        playerCoin += 1;
    }

    void EnableTimer()
    {
        
    }
}
