using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    public float EnemySpeed = 1.5f;
    public int XMoveDirection = 1;
    private float scale;
    public bool inShell = false;
    public float movingSpeed = 6.0f;
    public bool isMoving = false;

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
        float hitDistance;
        if(scale <= 1.0f)
        {
            hitDistance = 0.4f;
        }
        else
        {
            hitDistance = 0.8f;
        }
        if(!inShell)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(XMoveDirection* EnemySpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y) ;
            if(hit.collider!=null && hit.distance < hitDistance && hit.collider.gameObject.layer != 3 && hit.collider.gameObject.tag != "Player") // not flip when touch the collectable layer
            {
                Flip();

            }
            if(hit.collider!=null && hit.distance < hitDistance && hit.collider.tag == "Player") // not flip when touch the collectable layer
            {
                if(hit.collider.GetComponent<PlayerMoveProt>().isInvincible == false)
                {
                    hit.collider.GetComponent<PlayerHealth>().Degrade();
                }
            }

        }
        else
        {
            if(isMoving)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(movingSpeed * XMoveDirection, gameObject.GetComponent<Rigidbody2D>().velocity.y);
                if(hit.collider!=null && hit.distance < hitDistance && hit.collider.gameObject.layer != 3 && hit.collider.gameObject.tag != "Enemy" && hit.collider.gameObject.tag != "Player") // not flip when touch the collectable layer
                {

                    if(hit.collider.gameObject.tag == "QuestionBox")
                    {
                        if(hit.collider.gameObject.tag == "QuestionBox")
                        {
                            if(hit.collider.gameObject.GetComponent<BlockBounce>().enabled)
                            {
                                hit.collider.gameObject.GetComponent<BlockBounce>().Bounce();
                            }
                            hit.collider.gameObject.GetComponent<Animator>().SetBool("isDisabled", true);
                            hit.collider.gameObject.GetComponent<BlockBounce>().enabled = false;
                            if(hit.collider.gameObject.GetComponent<QuestionBox>().enabled)
                            {
                                hit.collider.gameObject.GetComponent<QuestionBox>().Spawn();
                                hit.collider.gameObject.GetComponent<QuestionBox>().enabled = false;
                            }
                            else
                            {
                                SoundManagerScript.PlaySound("bump");
                            }
                        }
                    }
                    else if (hit.collider.gameObject.tag == "Breakable")
                    {
                        hit.collider.gameObject.GetComponent<BlockBreak>().Break();
                    }
                    else
                    {
                        SoundManagerScript.PlaySound("bump");
                    }

                    Flip();
                }
                if(hit.collider!=null && hit.distance < hitDistance && hit.collider.tag == "Player") // not flip when touch the collectable layer
                {
                    if(hit.collider.GetComponent<PlayerMoveProt>().isInvincible == false)
                    {
                        hit.collider.GetComponent<PlayerHealth>().Degrade();
                    }
                }
            }

        }
        

    }

    void Flip()
    {
        if(XMoveDirection > 0)
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
        if(!inShell)
        {
            if(collider.gameObject.tag == "Player")
            {
                if(!collider.gameObject.GetComponent<PlayerMoveProt>().isInvincible)
                {
                    collider.gameObject.GetComponent<PlayerHealth>().Degrade();
                } 

            }
        }
        else
        {
            if(!isMoving)
            {
                if(collider.gameObject.tag == "Player")
                {
                    SoundManagerScript.PlaySound("kick");
                    if(collider.gameObject.transform.position.x > gameObject.transform.position.x) move("left");
                    else move("right");
                    
                }
            }
            else
            {
                if(collider.gameObject.tag == "Player")
                {
                    if(!collider.gameObject.GetComponent<PlayerMoveProt>().isInvincible)
                    {
                        collider.gameObject.GetComponent<PlayerHealth>().Degrade();
                    } 

                }
                if(collider.gameObject.tag == "Enemy")
                {
                    collider.gameObject.GetComponent<Enemy>().die();
                }
                if(collider.gameObject.tag == "KoopaTroopa")
                {
                    collider.gameObject.GetComponent<Koopa>().die();
                }
                if(collider.gameObject.tag == "Beetle")
                {
                    Flip();
                    collider.gameObject.GetComponent<Beetle>().die();
                }
            }
        }

    }

    public void intoShell()
    {
        inShell = true;
        gameObject.GetComponent<Animator>().SetBool("inShell", true);
        gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(gameObject.GetComponent<CapsuleCollider2D>().size.x, 0.60f);
    }

    public void move(string direction)
    {
        StartCoroutine(moveCoroutine(direction));
    }
    public void stop()
    {
        StartCoroutine(stopCoroutine());
    }
    IEnumerator moveCoroutine(string direction)
    {
        
        switch(direction)
        {
            case "left":
                XMoveDirection = -1;
                break;
            case "right":
                XMoveDirection = 1;
                break;
        }
        yield return new WaitForFixedUpdate();
        yield return isMoving = true;
    }
    IEnumerator stopCoroutine()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        yield return isMoving = false;
    }

    public void die()
    {
        SoundManagerScript.PlaySound("kick");
        ScoreDisplayerScript.showScore("100", gameObject.transform.position);
        Player_Score.addScore(100);
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1000));
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = -2000;
        gameObject.GetComponent<Koopa>().enabled = false;
    }
}
