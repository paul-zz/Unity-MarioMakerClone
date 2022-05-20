using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalldownPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isFalling;
    public float fallSpeed = 0.1f;
    private float downSpeed = 0.0f;
    void Start()
    {
        isFalling = false;   
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            downSpeed += fallSpeed * Time.fixedDeltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y - downSpeed, transform.position.z);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0)
            {
                isFalling = true;
                Destroy(gameObject, 10.0f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.enabled == true)
        {
            collision.collider.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.collider.gameObject.transform.parent = null;
    }


}
