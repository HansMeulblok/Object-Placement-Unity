using System.Collections;
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

    float targetAngle = 0;
    float degreesPerClick = 2;
    float secsPerClick = 0.3f;
   
    private float curAngle = 0f;
    private float startAngle=0f;
    private float startTime=0f;

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
            Debug.Log("added collider");
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
        
        mousePos = Input.mousePosition;
        Vector3 worldpos = cam.ScreenToWorldPoint(mousePos);

        previewObject.transform.position = new Vector3(worldpos.x, worldpos.y, 0);
        
        if(Input.GetButtonDown("Fire1") && !isColliding && !rotationMode)
        {
            //instantiate
            currentObject = Instantiate(prefab, new Vector3(worldpos.x, worldpos.y, 0), Quaternion.identity);
            rotationMode = true;
        }

        if(rotationMode)
        {
            RotateObject();
        }


    }

    void RotateObject()
    {
        Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
        var clicks = Mathf.Round(Input.GetAxis("Mouse ScrollWheel") * 100);
        if (clicks != 0) {
            targetAngle += clicks * degreesPerClick;
            startAngle = curAngle;
            startTime = Time.time;
        }
       
        var t = (Time.time - startTime) / secsPerClick;
        if (t <= 1) {
            curAngle = Mathf.Lerp(startAngle, targetAngle, t);
            // finally, do the actual rotation
            currentObject.transform.rotation = new Quaternion(0,curAngle, 0, 0);
        }
    }
}
