﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private List<string> colliderTags = new List<string>();
    private Vector3 mousePos;
    private Camera cam;

    [HideInInspector] 
    public bool isColliding =  false;
    private bool rotationMode = false;
    private GameObject previewObject;
    private GameObject currentObject;
    private Color previewColour;
    bool readyToPlace;
    float cooldownTimer;
    float maxcoolDown = 0.5f;
    void Start()
    {
        cam = Camera.main;

        previewObject = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);

        previewColour = previewObject.GetComponent<SpriteRenderer>().color;
        previewColour.a = 0.5f;
        previewObject.GetComponent<SpriteRenderer>().color = previewColour;

        PreviewObject pv = previewObject.AddComponent<PreviewObject>();
        pv.tags = colliderTags;
        pv.objectPlacement = this.gameObject.GetComponent<ObjectPlacement>();

        if(previewObject.GetComponent<Collider2D>() == null)
        {
            BoxCollider2D col = previewObject.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
        }
        else
        {
            previewObject.GetComponent<Collider2D>().isTrigger = true;
        }

        if(previewObject.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = previewObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.drag = 0;
        }
        else
        {
            previewObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    void Update()
    {

        cooldownTimer += Time.deltaTime;
        if(cooldownTimer >= maxcoolDown)
        {
            readyToPlace = true;
            cooldownTimer = maxcoolDown;
        }

        if(rotationMode)
        {
            RotateObject();
        }

        mousePos = Input.mousePosition;
        Vector3 worldpos = cam.ScreenToWorldPoint(mousePos);

        previewObject.transform.position = new Vector3(worldpos.x, worldpos.y, 0);
        
        if(Input.GetButtonDown("Fire1") && !isColliding && !rotationMode && readyToPlace)
        {
            //instantiate
            currentObject = Instantiate(prefab, new Vector3(worldpos.x, worldpos.y, 0), Quaternion.identity);
            previewObject.SetActive(false);
            rotationMode = true;
        }
    }

    void RotateObject()
    {
        var clicks = Mathf.Round(Input.GetAxis("Mouse ScrollWheel") * 100);

        if(clicks != 0)
        {
            Vector3 rotationVector = currentObject.transform.rotation.eulerAngles;
            rotationVector.z += clicks;
            currentObject.transform.rotation = Quaternion.Euler(rotationVector);
        }

        if(Input.GetButtonDown("Fire1"))
        {
            resetCooldown();
            rotationMode = false;
            previewObject.SetActive(true);
        }

    }

    void resetCooldown()
    {
        readyToPlace = false;
        cooldownTimer = 0;
    }
}
