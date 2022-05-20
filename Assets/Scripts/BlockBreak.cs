using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreak : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject blockPartPrefab;
    private bool isBroken = false;
    private Transform originalTransform;
    void Start()
    {
        originalTransform = gameObject.transform;
    }


    public void Break()
    {
        if(!isBroken)
        {
            StartCoroutine("BreakAnimation");
        }
        BlockRayCast();
    }
    IEnumerator BreakAnimation()
    {
        // Bounce Up
        isBroken = true;
        SoundManagerScript.PlaySound("smash");
        GameObject blockPart1 = Instantiate(blockPartPrefab, new Vector3(gameObject.transform.position.x-0.16f, gameObject.transform.position.y-0.16f,0), Quaternion.identity);
        GameObject blockPart2 = Instantiate(blockPartPrefab, new Vector3(gameObject.transform.position.x-0.16f, gameObject.transform.position.y+0.16f,0), Quaternion.identity);
        GameObject blockPart3 = Instantiate(blockPartPrefab, new Vector3(gameObject.transform.position.x+0.16f, gameObject.transform.position.y-0.16f,0), Quaternion.identity);
        GameObject blockPart4 = Instantiate(blockPartPrefab, new Vector3(gameObject.transform.position.x+0.16f, gameObject.transform.position.y+0.16f,0), Quaternion.identity);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 0.1f);
        blockPart1.GetComponent<Rigidbody2D>().AddForce(new Vector2(-200, 500));
        blockPart1.GetComponent<Rigidbody2D>().angularVelocity = -1000;
        blockPart2.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, 1000));
        blockPart2.GetComponent<Rigidbody2D>().angularVelocity = -1000;
        blockPart3.GetComponent<Rigidbody2D>().AddForce(new Vector2(200, 500));
        blockPart3.GetComponent<Rigidbody2D>().angularVelocity = 1000;
        blockPart4.GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 1000));
        blockPart4.GetComponent<Rigidbody2D>().angularVelocity = 1000;
        Destroy(blockPart1, 2);
        Destroy(blockPart2, 2);
        Destroy(blockPart3, 2);
        Destroy(blockPart4, 2);
        yield return null;
    }
    void BlockRayCast()
    {
        // Kill enemy when bouncing below them
        RaycastHit2D rayUp = Physics2D.Raycast(originalTransform.position, Vector2.up);
        if (rayUp.collider!= null && rayUp.distance < 0.5f && rayUp.collider.gameObject.tag == "Enemy")
        {
            rayUp.collider.GetComponent<Enemy>().die();
        }
        if (rayUp.collider!= null && rayUp.distance < 0.5f && rayUp.collider.gameObject.tag == "Mushroom")
        {
            rayUp.collider.gameObject.GetComponent<MushroomAI>().Flip();
        }

    }
}
