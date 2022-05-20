using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBounce : MonoBehaviour
{
    // Start is called before the first frame update
    public float bounceSpeed = 0.4f;
    public float bounceHeight = 5.0f;
    private Vector2 originalPosition;
    private Vector3 originalScale;
    public bool canBounce = true;
    public bool isBouncing = false;
    void Start()
    {
        originalPosition = transform.localPosition;
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(isBouncing)
        {
            BlockRayCast();
        }
    }

    public void Bounce()
    {
        if(canBounce && !isBouncing)
        {
            SoundManagerScript.PlaySound("bump");
            StartCoroutine(BounceAnimation());
        }
    }

    IEnumerator BounceAnimation()
    {
        // Bounce Up
        isBouncing = true;
        while(true)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + bounceSpeed*Time.deltaTime);
            transform.localScale = new Vector3(transform.localScale.x + bounceSpeed*Time.deltaTime, (transform.localScale.y + bounceSpeed*Time.deltaTime), 1);
            if(transform.localPosition.y >= originalPosition.y + bounceHeight)
            {
                break;
            }
            yield return null;
        }
        // Fall down
        while(true)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - bounceSpeed*Time.deltaTime);
            transform.localScale = new Vector3(transform.localScale.x - bounceSpeed*Time.deltaTime, (transform.localScale.y - bounceSpeed*Time.deltaTime), 1);
            if(transform.localPosition.y <= originalPosition.y)
            {
                break;
            }
            yield return null;
        }
        transform.localPosition = originalPosition;
        transform.localScale = originalScale;
        isBouncing = false;
    }

    void BlockRayCast()
    {
        // Kill enemy when bouncing below them
        
        RaycastHit2D rayUp = Physics2D.BoxCast(transform.position, new Vector2(0.64f, 0.64f), 0f, Vector2.up);
    
        if (rayUp.collider!= null && rayUp.distance < 0.3f && rayUp.collider.gameObject.tag == "Enemy")
        {
            rayUp.collider.GetComponent<Enemy>().die();
        }
        if (rayUp.collider!= null && rayUp.distance < 0.3f && rayUp.collider.gameObject.tag == "Mushroom")
        {
            Debug.Log("Mushroom Flip!");
            rayUp.collider.gameObject.GetComponent<MushroomAI>().Flip();
        }

    }

}
