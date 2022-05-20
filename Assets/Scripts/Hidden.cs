using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidden : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D[] colliders;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        colliders = gameObject.GetComponents<BoxCollider2D>();
        colliders[0].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
