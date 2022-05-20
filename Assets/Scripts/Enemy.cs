using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float EnemySpeed = 1.5f;
    public int XMoveDirection = 1;
    private float scale;
    // Start is called before the first frame update
    void Start()
    {
        scale = gameObject.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newRaycastCentr = new Vector2(transform.position.x, transform.position.y-gameObject.GetComponent<SpriteRenderer>().bounds.size.y/4);// 1/4 lower than the original origin
        RaycastHit2D hit = Physics2D.Raycast(newRaycastCentr, new Vector2(XMoveDirection, 0));
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(XMoveDirection* EnemySpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y) ;
        float hitDistance;
        if(scale <= 1.0f)
        {
            hitDistance = 0.4f;
        }
        else
        {
            hitDistance = 0.8f;
        }
        if(hit.collider!=null && hit.distance < hitDistance && hit.collider.gameObject.layer != 3 && hit.collider.gameObject.tag != "Player") // not flip when touch the collectable layer
        {
            Flip();
            /*
            if(hit.collider.tag=="Player" && hit.collider.GetComponent<PlayerMoveProt>().isInvincible == false)
            {
                hit.collider.gameObject.GetComponent<PlayerHealth>().Degrade();
            }
            */
        }
        if(hit.collider!=null && hit.distance < hitDistance && hit.collider.tag == "Player") // not flip when touch the collectable layer
        {
            if(hit.collider.GetComponent<PlayerMoveProt>().isInvincible == false)
            {
                Debug.Log("Not Invincible!");
                hit.collider.GetComponent<PlayerHealth>().Degrade();
            }
            
            /*
            if(hit.collider.tag=="Player" && hit.collider.GetComponent<PlayerMoveProt>().isInvincible == false)
            {
                hit.collider.gameObject.GetComponent<PlayerHealth>().Degrade();
            }
            */
        }
        //TODO: fix this code
        destroyWhenFallDown();
    }

    void Flip()
    {
        if(XMoveDirection >0)
        {
            XMoveDirection = -1;
        }
        else
        {
            XMoveDirection = 1;
        }
        gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.gameObject.tag == "Player" )
        {
            if(!collider.gameObject.GetComponent<PlayerMoveProt>().isInvincible)
            {
                collider.gameObject.GetComponent<PlayerHealth>().Degrade();
            }
            
        }
        
    }

    public void die()
    {
        SoundManagerScript.PlaySound("kick");
        ScoreDisplayerScript.showScore("100", gameObject.transform.position);
        Player_Score.addScore(100);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1000));
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = -2000;
        gameObject.GetComponent<Enemy>().enabled = false;
    }

    private void destroyWhenFallDown()
    {
        if(gameObject.transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
