using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMoveProt : MonoBehaviour
{
    public enum PlayerState
    {
        small=1,
        super,
        fireFlower,
        star
    };
    [Header("Player Run")]
    public float moveSpeed = 0.01f;
    public float maxSpeed = .1f;
    public float speedRate = 1.0f;
    private float moveX;
    
    [Header("Player Jump")]
    public int playerJumpPower = 1250;
    public int playerBouncePower = 800;
    public int playerBackPower = 800;
    public float gravity = 1f;
    public float fallMultiplier = 5f;
    public int linearDrag = 6;
    
    [Header("Player State")]
    public float buttomDistance; // Distance from player center to player buttom
    public PlayerState state;
    public bool isInvincible = false;
    public bool onGround = false;
    private bool canBounce = false;
    private bool isBouncing = false;

    private bool onPipe = false;

    [Header("Objects")]
    public GameObject blockPartPrefab;
    public GameObject fireballPrefab;
    public Rigidbody2D rb;

    void Start()
    {
        buttomDistance = gameObject.GetComponent<BoxCollider2D>().size.y/2+0.1f;
        state = PlayerState.small;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        // Default value tested is 0.5f
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMove();
    }

    void Update()
    {
        PlayerJump();
        PlayerFire();
        modifyPhysics();
        PlayerRaycast();
        checkCanBounce();
    }

    int getPlayerDirection()
    {
        if(gameObject.GetComponent<SpriteRenderer>().flipX == true) return -1;
        else return 1;
    }

    void PlayerJump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if(onGround)
            {
                SoundManagerScript.PlaySound("jump");
                Jump();
            }
            
        }
    }

    void PlayerFire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (state == PlayerState.fireFlower)
                Fire();
        }
    }

    void PlayerMove()
    {
        moveX = Input.GetAxis("Horizontal");
        if(moveX!=0.0f)
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            if (moveX < 0.0f)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (moveX > 0.0f)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", false);
        }
        if(Input.GetButton("Fire1"))
        {
            speedRate = 1.5f;
        }
        else
        {
            speedRate = 1.0f;
        }
        rb.AddForce(2.5f * Vector2.right * moveX * moveSpeed * speedRate);
        if(Mathf.Abs(rb.velocity.x) > maxSpeed*speedRate)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x)*maxSpeed*speedRate, rb.velocity.y);
        }
 

    }
    

    void Jump()
    {
        rb.AddForce(Vector2.up * playerJumpPower, ForceMode2D.Impulse);
        gameObject.GetComponent<Animator>().SetBool("isJumping", true);
        onGround = false;
    }

    void Fire()
    {
        GameObject generatedFireball;
        SoundManagerScript.PlaySound("fire");
        gameObject.GetComponent<Animator>().SetBool("isFiring", true);
        Invoke("cancelFiringAnimation", 0.2f);
        generatedFireball = Instantiate(fireballPrefab, new Vector2(transform.position.x+getPlayerDirection()*0.1f, transform.position.y+0.2f), Quaternion.identity);
        generatedFireball.GetComponent<FireballMove>().Move(rb.velocity.x, getPlayerDirection());

    }

    void cancelFiringAnimation()
    {
        gameObject.GetComponent<Animator>().SetBool("isFiring", false);
    }
    void modifyPhysics()
    {
        bool changingDirections = (moveX>0&&rb.velocity.x<0||(moveX <0 && rb.velocity.x>0));
        if(onGround)
        {
            if(Mathf.Abs(moveX)<0.4f||changingDirections)
            {
                rb.drag = linearDrag;
                //Debug.Log(rb.drag);
            }
            else
            {
                //Debug.Log(rb.drag);
                rb.drag = 0;
            }
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if(rb.velocity.y<0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if(rb.velocity.y>0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (fallMultiplier/2);
            }
        }
    }


    void playerBounce()
    {
        isBouncing = true;
        if (Input.GetButton("Jump"))
        {
            Debug.Log("SuperJump!");
            rb.AddForce(Vector2.up * playerBouncePower + Vector2.up * playerJumpPower, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("Normal Bounce!");
            rb.AddForce(Vector2.up * playerBouncePower, ForceMode2D.Impulse);
        }
        Invoke("stopBouncing", 0.5f);
    }

    void stopBouncing()
    {
        isBouncing = false;
    }

    void checkCanBounce()
    {
        if(rb.velocity.y <= 0)
        {
            canBounce = true;
        }
        else
        {
            canBounce = false;
        }
    }
    void OnCollisionEnter2D(Collision2D Col)
    {
        if(Col.gameObject.tag=="Mushroom")
        {
            Destroy(Col.gameObject);
        }
        if(Col.gameObject.tag=="Bottom")
        {
            gameObject.GetComponent<PlayerHealth>().playerDie();
        }
    }


    void PlayerRaycast()
    {
        // Raycast Down to squish enemies or stand on ground
        RaycastHit2D hitL = Physics2D.Raycast(new Vector2(transform.position.x- 0.2f, transform.position.y), Vector2.down);
        RaycastHit2D hitM = Physics2D.Raycast(transform.position, Vector2.down);
        RaycastHit2D hitR = Physics2D.Raycast(new Vector2(transform.position.x+ 0.2f, transform.position.y), Vector2.down);
        RaycastHit2D hit = hitM;
        if(hitL.collider!=null && hitL.collider.gameObject.tag!="Hidden" && hitL.distance<buttomDistance|| hitR.collider!=null && hitR.collider.gameObject.tag!="Hidden" && hitR.distance<buttomDistance|| hitM.collider!=null && hitM.collider.gameObject.tag!="Hidden"&& hitM.distance<buttomDistance)
        {
            onGround = true;
            if(gameObject.GetComponent<Animator>().GetBool("isJumping") && rb.velocity.y<0)
            {
                gameObject.GetComponent<Animator>().SetBool("isJumping", false);
            }
            if(hitL.collider!=null && hitL.collider.gameObject.tag == "Enemy"&& hitL.distance<buttomDistance||hitR.collider!=null && hitR.collider.gameObject.tag == "Enemy"&& hitR.distance<buttomDistance||hitM.collider!=null && hitM.collider.gameObject.tag == "Enemy"&& hitM.distance<buttomDistance)
            {
                if(hitL.collider.gameObject.tag == "Enemy")
                {
                    hit = hitL;
                }
                else if(hitR.collider.gameObject.tag == "Enemy")
                {
                    hit = hitR;
                }
                else if(hitM.collider.gameObject.tag == "Enemy")
                {
                    hit = hitM;
                }
                if(gameObject.GetComponent<Animator>().GetBool("isJumping"))
                {
                    gameObject.GetComponent<Animator>().SetBool("isJumping", false);
                }
                playerBounce();
                SoundManagerScript.PlaySound("stomp");
                hit.collider.gameObject.GetComponent<Animator>().SetBool("isCrushed", true);
                /*
                // These part are preserved for killing enemy by star
                hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                hit.collider.gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
                hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity.y);
                hit.collider.gameObject.GetComponent<Rigidbody2D>().angularVelocity = -2000;
                */
                hit.collider.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                hit.collider.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                hit.collider.gameObject.GetComponent<Enemy>().enabled = false;
                ScoreDisplayerScript.showScore("100", hit.collider.gameObject.transform.position);
                Player_Score.addScore(100);
                Destroy(hit.collider.gameObject, 1.0f);
            }
            if(hitL.collider.gameObject.tag == "KoopaTroopa" && hitL.distance<buttomDistance||hitR.collider.gameObject.tag == "KoopaTroopa" && hitR.distance<buttomDistance||hitM.collider.gameObject.tag == "KoopaTroopa" && hitM.distance<buttomDistance)
            {
                if(hitL.collider.gameObject.tag == "KoopaTroopa")
                {
                    hit = hitL;
                }
                else if(hitR.collider.gameObject.tag == "KoopaTroopa")
                {
                    hit = hitR;
                }
                else if(hitM.collider.gameObject.tag == "KoopaTroopa")
                {
                    hit = hitM;
                }
                if(gameObject.GetComponent<Animator>().GetBool("isJumping"))
                {
                    gameObject.GetComponent<Animator>().SetBool("isJumping", false);
                }
                if(hit.collider.gameObject.GetComponent<Koopa>().inShell == false)
                {
                    if(canBounce && !isBouncing)
                    {
                        playerBounce();
                        SoundManagerScript.PlaySound("stomp");
                    }
                    hit.collider.gameObject.GetComponent<Koopa>().intoShell();
                    hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0
                    );
                }
                else
                {
                    if(hit.collider.gameObject.GetComponent<Koopa>().isMoving == false)
                    {
                        if(canBounce && !isBouncing)
                        {
                            playerBounce();
                            SoundManagerScript.PlaySound("kick");
                            if(gameObject.transform.position.x > hit.collider.gameObject.transform.position.x) hit.collider.gameObject.GetComponent<Koopa>().move("left");
                            else hit.collider.gameObject.GetComponent<Koopa>().move("right");
                        }

                    }
                    else if(hit.collider.gameObject.GetComponent<Koopa>().isMoving == true)
                    {
                        if(canBounce && !isBouncing)
                        {
                            playerBounce();
                            SoundManagerScript.PlaySound("kick");
                            hit.collider.gameObject.GetComponent<Koopa>().stop();
                        }
                    }

                }
                Debug.Log(isBouncing);
            }
                        if(hitL.collider.gameObject.tag == "Beetle" && hitL.distance<buttomDistance||hitR.collider.gameObject.tag == "Beetle" && hitR.distance<buttomDistance||hitM.collider.gameObject.tag == "Beetle" && hitM.distance<buttomDistance)
            {
                if(hitL.collider.gameObject.tag == "Beetle")
                {
                    hit = hitL;
                }
                else if(hitR.collider.gameObject.tag == "Beetle")
                {
                    hit = hitR;
                }
                else if(hitM.collider.gameObject.tag == "Beetle")
                {
                    hit = hitM;
                }
                if(gameObject.GetComponent<Animator>().GetBool("isJumping"))
                {
                    gameObject.GetComponent<Animator>().SetBool("isJumping", false);
                }
                if(hit.collider.gameObject.GetComponent<Beetle>().inShell == false)
                {
                    if(canBounce && !isBouncing)
                    {
                        playerBounce();
                        SoundManagerScript.PlaySound("stomp");
                    }
                    hit.collider.gameObject.GetComponent<Beetle>().intoShell();
                    hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0
                    );
                }
                else
                {
                    if(hit.collider.gameObject.GetComponent<Beetle>().isMoving == false)
                    {
                        if(canBounce && !isBouncing)
                        {
                            playerBounce();
                            SoundManagerScript.PlaySound("kick");
                            if(gameObject.transform.position.x > hit.collider.gameObject.transform.position.x) hit.collider.gameObject.GetComponent<Beetle>().move("left");
                            else hit.collider.gameObject.GetComponent<Beetle>().move("right");
                        }

                    }
                    else if(hit.collider.gameObject.GetComponent<Beetle>().isMoving == true)
                    {
                        if(canBounce && !isBouncing)
                        {
                            playerBounce();
                            SoundManagerScript.PlaySound("kick");
                            hit.collider.gameObject.GetComponent<Beetle>().stop();
                        }
                    }

                }
                Debug.Log(isBouncing);
            }
            if(hitL.collider!=null && hitL.collider.gameObject.tag == "BulletBill"&& hitL.distance<buttomDistance||hitR.collider!=null && hitR.collider.gameObject.tag == "BulletBill"&& hitR.distance<buttomDistance||hitM.collider!=null && hitM.collider.gameObject.tag == "BulletBill"&& hitM.distance<buttomDistance)
            {
                if(hitL.collider.gameObject.tag == "BulletBill")
                {
                    hit = hitL;
                }
                else if(hitR.collider.gameObject.tag == "BulletBill")
                {
                    hit = hitR;
                }
                else if(hitM.collider.gameObject.tag == "BulletBill")
                {
                    hit = hitM;
                }
                if(gameObject.GetComponent<Animator>().GetBool("isJumping"))
                {
                    gameObject.GetComponent<Animator>().SetBool("isJumping", false);
                }
                rb.AddForce(Vector2.up * playerBouncePower, ForceMode2D.Impulse);
                SoundManagerScript.PlaySound("stomp");
                /*
                // These part are preserved for killing enemy by star
                hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                hit.collider.gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
                hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity.y);
                hit.collider.gameObject.GetComponent<Rigidbody2D>().angularVelocity = -2000;
                */
                hit.collider.gameObject.GetComponent<BulletBill>().Die();
                ScoreDisplayerScript.showScore("100", hit.collider.gameObject.transform.position);
                Player_Score.addScore(100);
            }
        }
        else
        {
            onGround = false;
        }
        if(hitL.collider!=null && hitR.collider!=null && hitM.collider!=null && hitM.distance<buttomDistance && hitM.collider.tag == "Pipe")
        {
            onPipe = true;
            if(Input.GetAxis("Vertical")<0)
            {
                hitM.collider.GetComponent<PipeTravel>().Travel();
            }
        }
        /*
        if ((hitL.collider!=null && hitL.distance<buttomDistance || hitR.collider!=null && hitR.distance<buttomDistance|| hitM.collider!=null && hitM.distance<buttomDistance )&& hit.collider.gameObject.tag=="Enemy")
        {
            if(gameObject.GetComponent<Animator>().GetBool("isJumping"))
            {
                gameObject.GetComponent<Animator>().SetBool("isJumping", false);
            }
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * playerBouncePower);
            hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            hit.collider.gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
            hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity.y);
            hit.collider.gameObject.GetComponent<Rigidbody2D>().angularVelocity = -2000;
            hit.collider.gameObject.GetComponent<Enemy>().enabled = false;
        }
        */

        // Raycast up to break blocks
        // TODO:FIX HERE
        RaycastHit2D rayUp = Physics2D.Raycast(transform.position, Vector2.up);
        //RaycastHit2D rayUp = Physics2D.BoxCast(transform.position, new Vector2(0.64f, 0.64f), 0f, Vector2.up);
        if (rayUp.collider!= null && rayUp.distance < buttomDistance && rayUp.collider.gameObject.tag == "Breakable" && gameObject.GetComponent<Animator>().GetBool("isJumping"))
        {
            
            // Destroy(rayUp.collider.gameObject);
            if(state == PlayerState.small)
            {
                rayUp.collider.gameObject.GetComponent<BlockBounce>().Bounce();
            }
            else
            {
                rayUp.collider.gameObject.GetComponent<BlockBreak>().Break();
                // rb.AddForce(Vector2.down * playerBackPower);
            }
            
        }
        if (rayUp.collider!= null && rayUp.distance < buttomDistance && rayUp.collider.gameObject.tag == "QuestionBox" &&gameObject.GetComponent<Animator>().GetBool("isJumping"))
        {
            if(rayUp.collider.gameObject.GetComponent<BlockBounce>().enabled)
            {
                rayUp.collider.gameObject.GetComponent<BlockBounce>().Bounce();
            }
            rayUp.collider.gameObject.GetComponent<Animator>().SetBool("isDisabled", true);
            rayUp.collider.gameObject.GetComponent<BlockBounce>().enabled = false;
            if(rayUp.collider.gameObject.GetComponent<QuestionBox>().enabled)
            {
                rayUp.collider.gameObject.GetComponent<QuestionBox>().Spawn();
                rayUp.collider.gameObject.GetComponent<QuestionBox>().enabled = false;
            }
        }
        if (rayUp.collider!= null && rayUp.distance < buttomDistance && rayUp.collider.gameObject.tag == "Hidden" &&gameObject.GetComponent<Animator>().GetBool("isJumping") && gameObject.GetComponent<Rigidbody2D>().velocity.y>0)
        {
            if(rayUp.collider.gameObject.GetComponent<BlockBounce>().enabled)
            {
                rayUp.collider.gameObject.GetComponent<BlockBounce>().Bounce();
            }
            rayUp.collider.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            rayUp.collider.gameObject.GetComponents<BoxCollider2D>()[0].enabled = true;
            rayUp.collider.gameObject.GetComponent<Animator>().SetBool("isDisabled", true);
            rayUp.collider.gameObject.tag = "QuestionBox";
            rayUp.collider.gameObject.GetComponent<BlockBounce>().enabled = false;
            if(rayUp.collider.gameObject.GetComponent<QuestionBox>().enabled)
            {
                rayUp.collider.gameObject.GetComponent<QuestionBox>().Spawn();
                rayUp.collider.gameObject.GetComponent<QuestionBox>().enabled = false;
            }
        }

        // Raycast horizontal to detect block collision
    
    }
    
    public void SetState(PlayerState setState)
    {
        state = setState;
    }
    void OnTriggerEnter2D(Collider2D trig)
    {
       
        if(trig.gameObject.tag == "Bottom")
        {
            Debug.Log("ONTRIGGER");
            gameObject.GetComponent<PlayerHealth>().playerDie();
        }
        
    }


}
