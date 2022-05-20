using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public GameObject explosion;
    public float fireballHSpeed = 5.0f;
    public float fireballVSpeed = 4.0f;
    private int XMoveDirection;
    private float fireballHSpeedReal;
    void Start()
    {
        rb.angularVelocity = -1500;
    }

    // Update is called once per frame
    void Update()
    {
        FireballRaycast();
        destroyWhenFallDown();
    }

    public void Move(float playerSpeed, int playerDirection)
    {
        fireballHSpeedReal = playerSpeed + playerDirection * fireballHSpeed;
        rb.velocity = new Vector2(fireballHSpeedReal, 0);
        XMoveDirection = playerDirection;
    }

    private void Bounce()
    {
        rb.velocity = new Vector2(fireballHSpeedReal, fireballVSpeed);
    }

    public void FireballRaycast()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(XMoveDirection, 0));
        float hitDistance = 0.2f;
        if(hit.collider!=null && hit.distance < hitDistance && hit.collider.gameObject.layer != 3 && hit.collider.gameObject.tag != "Player") // not flip when touch the collectable layer
        {
            if(hit.collider.gameObject.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<Enemy>().die();
            }
            else if(hit.collider.gameObject.tag == "KoopaTroopa")
            {
                hit.collider.gameObject.GetComponent<Koopa>().die();
            }
            else
            {
                SoundManagerScript.PlaySound("bump");
            }
            playExplosion();
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.gameObject.tag == "Enemy")
        {
            playExplosion();
            collider.gameObject.GetComponent<Enemy>().die();
            Destroy(gameObject);
        }
        if(collider.gameObject.tag == "KoopaTroopa")
        {
            playExplosion();
            collider.gameObject.GetComponent<Koopa>().die();
            Destroy(gameObject);
        }
        if(collider.gameObject.tag == "BulletBill")
        {
            playExplosion();
            Destroy(gameObject);
        }
        if(collider.gameObject.tag == "Beetle")
        {
            playExplosion();
            Destroy(gameObject);
        }
        Bounce();
        
    }
    private void destroyWhenFallDown()
    {
        if(gameObject.transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    private void playExplosion()
    {
        GameObject spawnedExplotion;
        spawnedExplotion = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(spawnedExplotion, .2f);
    }
}
