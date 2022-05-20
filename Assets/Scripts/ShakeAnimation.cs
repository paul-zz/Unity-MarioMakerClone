using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform rendererObject;
    private Quaternion originalRotation;
    private float rotateSpeed = 0.5f;
    private bool isShaking = false;
    void Start()
    {
        isShaking = false;
        rendererObject = transform.parent.GetChild(0);
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.enabled == true)
        {
            shake();
        }
        
    }

    private void shake()
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeAnimator());
        }
    }
    IEnumerator ShakeAnimator()
    {
        isShaking = true;
        Debug.Log("Rotating");
        for(int i = 0; i< 3; i++)
        {
            while (true)
            {
                rendererObject.rotation = Quaternion.Euler(0, 0, rendererObject.rotation.eulerAngles.z + rotateSpeed);
                Debug.Log(rendererObject.rotation.eulerAngles.z);
                if (rendererObject.rotation.eulerAngles.z >= 4f && rendererObject.rotation.eulerAngles.z < 10f)
                {
                    break;
                }
                yield return null;
            }
            Debug.Log("Recovering");
            while (true)
            {
                rendererObject.rotation = Quaternion.Euler(0, 0, rendererObject.rotation.eulerAngles.z - rotateSpeed);
                Debug.Log(rendererObject.rotation.eulerAngles.z);
                if (rendererObject.rotation.eulerAngles.z <= 356f && rendererObject.rotation.eulerAngles.z > 300f)
                {

                    break;
                }
                yield return null;
            }
            yield return null;
        }
        Debug.Log("Recovered");
        rendererObject.rotation = originalRotation;
        isShaking = false;
    }
}
