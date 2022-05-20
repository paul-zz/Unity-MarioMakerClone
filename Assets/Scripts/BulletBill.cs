using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBill : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 2f;
    public int movingDirection = -1;
    private bool isAlive = true;
    Transform originalTransform;
    void Start()
    {
        originalTransform = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(movingDirection * moveSpeed, 0);
            gameObject.transform.position = new Vector3(transform.position.x, originalTransform.position.y, originalTransform.position.z);
        }
    }

    public void Die()
    {
        isAlive = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 6;
        Destroy(gameObject, 5.0f);
    }

    public void Flip()
    {
        gameObject.GetComponent<SpriteRenderer>().flipX = true;
        movingDirection = -movingDirection;
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
}
