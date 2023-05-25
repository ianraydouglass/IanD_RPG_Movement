using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created for the Ruins of Vha version 2 movement module
public class ObjectLayer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private int layerZoneValue = 100;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    
    void Update()
    {
        LayerShift();
    }

    private void LayerShift()
    {
        //this should turn the y float into something that translates well into my pixels to units value
        int positionModifier = (int)(transform.position.y * -16);
        spriteRenderer.sortingOrder = positionModifier + layerZoneValue;
    }
}
