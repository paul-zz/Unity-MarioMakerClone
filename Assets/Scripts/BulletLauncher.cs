using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject launchObject;
    public int coolDownTime = 3;
    public GameObject playerObject;
    void Start()
    {
        StartCoroutine(launchCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator launchCoroutine()
    {
        GameObject launchedObject;
        while(true)
        {
            yield return new WaitForSeconds(coolDownTime);
            SoundManagerScript.PlaySound("bullet");
            launchedObject = Instantiate(launchObject, new Vector3(transform.position.x, transform.position.y + 0.32f, transform.position.z), Quaternion.identity);
            if(playerObject.transform.position.x > transform.position.x)
            {
                if(launchedObject.tag == "BulletBill")
                launchedObject.GetComponent<BulletBill>().Flip();
            }
        }
    }
}
