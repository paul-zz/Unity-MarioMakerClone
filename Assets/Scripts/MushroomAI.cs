using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAI : MonoBehaviour
{
   
    public int MushroomSpeed = 3;
    public int XMoveDirection = 1;
    private float scale;
    // Start is called before the first frame update
    void Start()
    {
        scale = gameObject.transform.localScale.x;
    }
    void Update()
    {
        //TODO: fix this code
        Vector2 newRaycastCentr = new Vector2(transform.position.x, transform.position.y-gameObject.GetComponent<SpriteRenderer>().bounds.size.y/4);// 1/4 lower than the original origin
        RaycastHit2D hit = Physics2D.Raycast(newRaycastCentr, new Vector2(XMoveDirection, 0));
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(XMoveDirection* MushroomSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y) ;
        float hitDistance=0.4f;
        if(hit.collider!=null && hit.distance < hitDistance &&hit.collider.gameObject.tag!="Enemy"&&hit.collider.gameObject.tag!="Player"&&hit.collider.gameObject.tag!="KoopaTroopa")
        {
            Flip();
        }
    }
    public void Flip()
    {
        XMoveDirection = 0-XMoveDirection;
    }


}
