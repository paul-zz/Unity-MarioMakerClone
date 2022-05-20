using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void slowTime()
    {
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }
    public void normalTime()
    {
        Invoke("normalTimeEffect", .06f);
    }

    private void normalTimeEffect()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02F;
    }
}
