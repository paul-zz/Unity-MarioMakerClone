using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPlatform : MonoBehaviour
{
    // Start is called before the first frame updates
    private Vector3 originalTransform;
    private Vector3 endTransform;
    private Vector3 targetTransform;
    private bool movetoRight = true;
    public float movingDistance = 5.0f;
    public float movingSpeed = 1.5f;
    void Start()
    {
        originalTransform = gameObject.transform.position;
        endTransform = gameObject.transform.position + new Vector3(movingDistance, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == originalTransform)
        {
            targetTransform = endTransform;
        }
        else if (transform.position == endTransform)
        {
            targetTransform = originalTransform;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetTransform, movingSpeed * Time.deltaTime);
        //if (movetoRight)
        //{
        //    if (gameObject.transform.position.x < originalTransform.x + movingDistance)
        //    {
        //        // Debug.Log("PositionX:"+gameObject.transform.position.x.ToString()+"of"+(originalTransform.x+movingDistance).ToString());
        //        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(movingSpeed, 0);
        //        // gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(movingSpeed, 0));
        //    }
        //    else
        //    {
        //        movetoRight = false;
        //    }
        //}
        //else
        //{
        //    if (gameObject.transform.position.x > originalTransform.x)
        //    {
        //        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-movingSpeed, 0);
        //    }
        //    else
        //    {
        //        movetoRight = true;
        //    }
        //}



    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Other obj on plat");
        other.gameObject.transform.SetParent(transform);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        other.gameObject.transform.SetParent(null);
    }
}
