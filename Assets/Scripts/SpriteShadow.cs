using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadow : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 offset = ShadowManager.shadowOffset;
    private SpriteRenderer sprRndCaster;
    private SpriteRenderer sprRndShadow;
    
    private Transform transCaster;
    private Transform transShadow;

    private bool shadowOn = ShadowManager.shadowOn;
    private float alpha = ShadowManager.shadowAlpha;

    void Start()
    {
        if (shadowOn)
        {
            transCaster = transform;
            transShadow = new GameObject().transform;
            transShadow.parent = transCaster;
            transShadow.gameObject.name = "shadow";
            transShadow.localRotation = Quaternion.identity;

            sprRndCaster = GetComponent<SpriteRenderer>();
            sprRndShadow = transShadow.gameObject.AddComponent<SpriteRenderer>();

            sprRndShadow.sortingLayerName = sprRndCaster.sortingLayerName;
            sprRndShadow.sortingOrder = sprRndCaster.sortingOrder - 2;
            sprRndShadow.color = new Color(0.0f, 0.0f, 0.0f, alpha);
            
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (shadowOn)
        {
            transShadow.position = new Vector2(transCaster.position.x + offset.x, transCaster.position.y + offset.y);
            sprRndShadow.enabled = sprRndCaster.enabled;
            sprRndShadow.sprite = sprRndCaster.sprite;
            sprRndShadow.flipX = sprRndCaster.flipX;
        }
    }
}
