using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PipeTravel : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneToTravel;
    public GameObject pipeToTravel;
    public GameObject playerObject;
    
    public bool canTravel = false;
    public bool isPlayingAnimation = false;
    private Scene currentScene;
    private float enterPipeSpeed = 1.0f;
    private float downPipeDistance = 0.32f;
    
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Travel()
    {
        if(canTravel)
        {
            if(sceneToTravel == currentScene.name)
            {
                
                PlayEnterPipeAnimation();
            }
            else
            {
                
                SceneManager.LoadScene(sceneToTravel, LoadSceneMode.Additive);
                SceneManager.MoveGameObjectToScene(playerObject, SceneManager.GetSceneByName(sceneToTravel));
                SceneManager.UnloadSceneAsync(currentScene);
                //playerObject.transform.position = new Vector2(pipeToTravel.transform.position.x, pipeToTravel.transform.position.y+0.32f);
            }
        }
    }

    private void PlayEnterPipeAnimation()
    {
        if(playerObject!=null)
        {
            if(playerObject.GetComponent<PlayerMoveProt>().state == PlayerMoveProt.PlayerState.small)
            {
                downPipeDistance = 0.64f;
                enterPipeSpeed = 1.0f;
            }
            else
            {
                downPipeDistance = 1.28f;
                enterPipeSpeed = 2.0f;
            }
        }
        if(!isPlayingAnimation)
        {
            StartCoroutine(enterPipeAnimation());
        }
    }




    IEnumerator enterPipeAnimation()
    {
        SoundManagerScript.PlaySound("degrade");
        float originalY = playerObject.transform.position.y;
        playerObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        playerObject.GetComponent<CapsuleCollider2D>().enabled = false;
        isPlayingAnimation = true;
        Debug.Log(downPipeDistance);
        while(true)
        {
            playerObject.transform.position = new Vector2(playerObject.transform.transform.position.x, playerObject.transform.transform.position.y-enterPipeSpeed*Time.deltaTime);
            if(playerObject.transform.position.y < originalY - downPipeDistance)
            {
                break;
            }
            yield return null;
        }
        playerObject.transform.position = new Vector2(pipeToTravel.transform.position.x, pipeToTravel.transform.position.y+0.64f-downPipeDistance);
        originalY = playerObject.transform.position.y;

        isPlayingAnimation = true;
        SoundManagerScript.PlaySound("degrade");
        while(true)
        {
            playerObject.transform.position = new Vector2(playerObject.transform.transform.position.x, playerObject.transform.transform.position.y+enterPipeSpeed*Time.deltaTime);
            if(playerObject.transform.position.y > originalY + downPipeDistance)
            {
                break;
            }
            yield return null;
        }
        playerObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        playerObject.GetComponent<CapsuleCollider2D>().enabled = true;
        isPlayingAnimation = false;
    }

}
