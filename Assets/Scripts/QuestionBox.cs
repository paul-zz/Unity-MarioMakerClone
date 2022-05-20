using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBox : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 spawnPoint;
    public GameObject content;
    public GameObject animatedCoin;
    private GameObject spawnedGameObject;
    private bool canSpawn = true;
    private bool isPlaying = false;
    private Transform original;
    private float originalGravity;
    public int spawnNum=1;
    void Start()
    {
        spawnPoint = new Vector3(transform.position.x, transform.position.y + 0.1f, 0);
        original = transform;
        if(content.tag != "Coin")
        {
            originalGravity = content.GetComponent<Rigidbody2D>().gravityScale;
        }
    }
        

    // Update is called once per frame
    void Update()
    {
    
    }

    private void playSpawnAnimate(GameObject spawnedObject)
    {
        if(canSpawn && !isPlaying)
        {
            SoundManagerScript.PlaySound("spawn");
            StartCoroutine(spawnAnimate(spawnedObject));
        }
    }
    private void playCoinAnimate(GameObject spawnedObject)
    {
        if(canSpawn && !isPlaying)
        {
            SoundManagerScript.PlaySound("coin");
            StartCoroutine(coinAnimate(spawnedObject));
        }
    }

    IEnumerator spawnAnimate(GameObject spawned)
    {
        isPlaying = true;
        BoxCollider2D[] boxCollider2Ds;
        CircleCollider2D[] circleCollider2Ds;
        boxCollider2Ds = spawned.GetComponents<BoxCollider2D>();
        circleCollider2Ds = spawned.GetComponents<CircleCollider2D>();
        foreach(BoxCollider2D collider2D in boxCollider2Ds)
        {
            collider2D.enabled = false;
        }
        foreach(CircleCollider2D collider2D1 in circleCollider2Ds)
        {
            collider2D1.enabled = false;
        }
        spawned.GetComponent<Rigidbody2D>().gravityScale = 0;
        spawned.GetComponent<SpriteRenderer>().sortingLayerName = "BG";
        Vector3 originalScale = spawned.transform.localScale;
        Debug.Log(originalScale);
        if(spawned.transform.localScale.x>1)
        {
            Debug.Log("SCALE LARGER THAN 1");
            spawned.transform.localScale = new Vector3(1, 1, 1);
        }
        while(true)
        {
            spawned.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            spawned.transform.position = new Vector3(spawned.transform.position.x, spawned.transform.position.y+1.0f*Time.deltaTime, spawned.transform.position.z);
            if(originalScale.x > 1)
            {
                spawned.transform.localScale = new Vector3(spawned.transform.localScale.x + 1.0f*Time.deltaTime, spawned.transform.localScale.y +1.0f*Time.deltaTime, spawned.transform.localScale.z);
            }
            if(spawned.transform.position.y > original.position.y + 0.64f || (originalScale.x > 1&&spawned.transform.localScale.x > originalScale.x))
            {
                spawned.transform.localScale = originalScale;
                break;
            }
            yield return null;
        }
        foreach(BoxCollider2D collider2D in boxCollider2Ds)
        {
            collider2D.enabled = true;
        }
        foreach(CircleCollider2D collider2D1 in circleCollider2Ds)
        {
            collider2D1.enabled = true;
        }
        spawned.GetComponent<Rigidbody2D>().gravityScale = originalGravity;
        spawned.GetComponent<SpriteRenderer>().sortingLayerName = "EnemyAndItem";
        isPlaying = false;
    }

    IEnumerator coinAnimate(GameObject spawned)
    {
        isPlaying = true;
        spawned.GetComponent<SpriteRenderer>().sortingLayerName = "BG";
        SoundManagerScript.PlaySound("coin");
        // while(true)
        // {
        //     spawned.transform.position = new Vector3(spawned.transform.position.x, spawned.transform.position.y+5.0f*Time.deltaTime, spawned.transform.position.z);
        //     if(spawned.transform.position.y > original.position.y + 1.5f)
        //     {
        //         break;
        //     }
        //     yield return null;
        // }
        // while(true)
        // {
        //     spawned.transform.position = new Vector3(spawned.transform.position.x, spawned.transform.position.y-6f*Time.deltaTime, spawned.transform.position.z);
        //     if(spawned.transform.position.y < original.position.y + 0.2f)
        //     {
        //         break;
        //     }
        //     yield return null;
        // }
        spawned.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 300);
        yield return null;
        spawned.GetComponent<SpriteRenderer>().sortingLayerName = "EnemyAndItem";
        ScoreDisplayerScript.showScore("200", spawned.transform.position + new Vector3(0, 0.32f, 0));
        Player_Score.addScore(200);
        Destroy(spawned, 1);
        isPlaying = false;
    }

    private void spawnAndAnimate()
    {
        for (int i = 0; i < spawnNum; i++)
        {
            spawnedGameObject = Instantiate(content, spawnPoint, Quaternion.identity);
            playSpawnAnimate(spawnedGameObject);
        }
    }

    public void Spawn()
    {
        if(content.tag == "Coin")
        {
            spawnedGameObject = Instantiate(animatedCoin, spawnPoint, Quaternion.identity);
            Player_Score.addCoin();
            playCoinAnimate(spawnedGameObject);
        }
        else
        {
            Invoke("spawnAndAnimate", 0.2f);
        }
    }

}
