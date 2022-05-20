using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update

    public int PlayerHealthValue;
    public bool hasDied;
    private Camera Cam;
    private float height;
    private float CamY;
    void Start()
    {
        Cam = Camera.main;
        hasDied = false;
        gameObject.GetComponent<Animator>().SetBool("isDead", false);
    }

    // Update is called once per frame
    void Update()
    {
        height = Cam.orthographicSize;
        CamY = Cam.transform.position.y;
        if(hasDied)
        {
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
            gameObject.GetComponent<Animator>().SetBool("isDead", true);
            gameObject.GetComponent<PlayerMoveProt>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 500));
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
            hasDied = false;
        }
        if(gameObject.transform.position.y < -5)
        {
            StartCoroutine("Die");
        }
    }
    
    IEnumerator Die()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Prototype1");
        Destroy(gameObject);
    }

    public void Degrade()
    {
        if(gameObject.GetComponent<PlayerMoveProt>().state == PlayerMoveProt.PlayerState.super)
        {
            gameObject.GetComponent<PlayerMoveProt>().isInvincible = true;
            Physics2D.IgnoreLayerCollision(7, 8);
            gameObject.GetComponent<PlayerMoveProt>().state = PlayerMoveProt.PlayerState.small;
            gameObject.GetComponent<Animator>().SetBool("isSuper", false);
            SoundManagerScript.PlaySound("degrade");
            gameObject.GetComponent<TimeManagement>().slowTime();
            gameObject.GetComponent<PlayerGrowAnimation>().playDegradeAnimation();
            gameObject.GetComponent<TimeManagement>().normalTime();
            Invoke("cancelInvincible", 1.0f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.64f, 0.64f);
            gameObject.GetComponent<PlayerMoveProt>().buttomDistance = 0.4f;
            
        }
        else if(gameObject.GetComponent<PlayerMoveProt>().state == PlayerMoveProt.PlayerState.fireFlower)
        {
            gameObject.GetComponent<PlayerMoveProt>().isInvincible = true;
            Physics2D.IgnoreLayerCollision(7, 8);
            gameObject.GetComponent<PlayerMoveProt>().state = PlayerMoveProt.PlayerState.super;
            gameObject.GetComponent<Animator>().SetBool("isFireFlower", false);
            SoundManagerScript.PlaySound("degrade");
            gameObject.GetComponent<TimeManagement>().slowTime();
            gameObject.GetComponent<TimeManagement>().normalTime();
            Invoke("cancelInvincible", 1.0f);
        }
        else if(gameObject.GetComponent<PlayerMoveProt>().state == PlayerMoveProt.PlayerState.small)
        {
            SoundManagerScript.PlaySound("die");
            hasDied = true;
        }
    }

    public void playerDie()
    {
        SoundManagerScript.PlaySound("die");
        hasDied = true;
    }

    private void cancelInvincible()
    {
        gameObject.GetComponent<PlayerMoveProt>().isInvincible = false;
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }
}
