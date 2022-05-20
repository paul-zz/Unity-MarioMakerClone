using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrowAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 originalPosition;
    private Vector3 originalScale;
    public bool canPlay = true;
    public bool isPlaying = false;
    public float growSpeed = 0.04f;
    public float shrinkSpeed = 0.01f;
    public float growYSize = 1.0f;
    public float shrinkYSize = .8f;
    void Start()
    {
        originalScale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playAnimation()
    {
        if(canPlay && !isPlaying)
        {
            StartCoroutine(growAnimation());
        }
    }

    public void playDegradeAnimation()
    {
        if(canPlay && !isPlaying)
        {
            StartCoroutine(degradeAnimation());
        }
    }

    IEnumerator growAnimation()
    {
        // Bounce Up
        isPlaying = true;
        transform.localScale = new Vector3(1, 0.5f, 1);
        while(true)
        {
            transform.localScale = new Vector3(1, (transform.localScale.y + growSpeed*Time.fixedDeltaTime), 1);

            if(transform.localScale.y > growYSize)
            {
                break;
            }
            yield return null;
        }
        for(int i = 0; i < 3; i++)
        {
            // Shrink A little and grow up again for 3 times
            while(true)
            {
                transform.localScale = new Vector3(1, (transform.localScale.y - shrinkSpeed*Time.fixedDeltaTime), 1);
                if(transform.localScale.y  <= shrinkYSize)
                {
                    break;
                }
                yield return null;
            }
            while(true)
            {
                transform.localScale = new Vector3(transform.localScale.x, (transform.localScale.y + shrinkSpeed*Time.fixedDeltaTime), 1);
                if(transform.localScale.y  > growYSize)
                {
                    break;
                }
                yield return null;
            }
        }
        transform.localScale = originalScale;
        isPlaying = false;
    }

    IEnumerator degradeAnimation()
    {
        // Bounce Up
        isPlaying = true;
        transform.localScale = new Vector3(1, 2.0f, 1);
        while(true)
        {
            transform.localScale = new Vector3(1, (transform.localScale.y - growSpeed*Time.fixedDeltaTime), 1);

            if(transform.localScale.y < 1.0f)
            {
                break;
            }
            yield return null;
        }
        for(int i = 0; i < 3; i++)
        {
            // Shrink A little and grow up again for 3 times
            while(true)
            {
                transform.localScale = new Vector3(1, (transform.localScale.y - shrinkSpeed*Time.fixedDeltaTime), 1);
                if(transform.localScale.y <= shrinkYSize)
                {
                    break;
                }
                yield return null;
            }
            while(true)
            {
                transform.localScale = new Vector3(transform.localScale.x, (transform.localScale.y + shrinkSpeed*Time.fixedDeltaTime), 1);
                if(transform.localScale.y  > growYSize)
                {
                    break;
                }
                yield return null;
            }
        }
        transform.localScale = originalScale;
        isPlaying = false;
    }
}
