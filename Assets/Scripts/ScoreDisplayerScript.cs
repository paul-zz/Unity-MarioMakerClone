using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameObject score100;
    public static GameObject score200;
    public static GameObject score1000;
    public static GameObject score2000;
    public static GameObject score5000;
    public static GameObject score1up;
    public static ScoreDisplayerScript displayer;
    public float liftHeight = 0.64f;

    private GameObject spawnedScore;
    void Start()
    {
        score100 = Resources.Load<GameObject>("100");
        score200 = Resources.Load<GameObject>("200");
        score1000 = Resources.Load<GameObject>("1000");
        score2000 = Resources.Load<GameObject>("2000");
        score5000 = Resources.Load<GameObject>("5000");
        score1up = Resources.Load<GameObject>("1up");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        displayer = this;
    }
    public static void showScore(string score, Vector3 position)
    {
        
        switch(score)
        {
            case "100":
                Debug.Log("100 displayed");
                displayer.StartCoroutine(displayer.scoreAnimation(score100, position));
                break;
            case "200":
                displayer.StartCoroutine(displayer.scoreAnimation(score200, position));
                break;
            case "1000":
                displayer.StartCoroutine(displayer.scoreAnimation(score1000, position));
                break;
            case "2000":
                displayer.StartCoroutine(displayer.scoreAnimation(score2000, position));
                break;
            case "5000":
                displayer.StartCoroutine(displayer.scoreAnimation(score5000, position));
                break;
            case "1up":
                displayer.StartCoroutine(displayer.scoreAnimation(score1up, position));
                break;
        }
    }



    IEnumerator scoreAnimation(GameObject scoreObject, Vector3 position)
    {
        yield return new WaitForSeconds(0.1f);
        spawnedScore = Instantiate(scoreObject, position, Quaternion.identity);
        Vector3 originalPosition = position;
        while(true)
        {
            spawnedScore.transform.position = new Vector3(position.x, spawnedScore.transform.position.y + 3.0f*Time.deltaTime, position.z);
            if(spawnedScore.transform.position.y > position.y + liftHeight)
            {
                break;
            }
            yield return null;
        }
        Destroy(spawnedScore, 1.0f);
        yield return new WaitForSeconds(1);
    }
}
