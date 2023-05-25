using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created for the Ruins of Vha version 2 movement module
public class PlayerLayer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer playerSpriteRenderer;
    [SerializeField]
    private GameObject playerContainerObject;
    private Transform playerContainerTransform;

    //this value should be set to whatever layer starting range is supposed to be applied to the object
    [SerializeField]
    private int layerZoneValue = 100;

    void Start()
    {
        playerContainerTransform = playerContainerObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        LayerShift();
    }

    private void LayerShift()
    {
        //this should turn the y float into something that translates well into my pixels to units value
        int positionModifier = (int)(playerContainerTransform.position.y * -16);
        playerSpriteRenderer.sortingOrder = positionModifier + layerZoneValue;
    }
}
