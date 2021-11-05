using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    [HideInInspector] 
    public bool isColliding =  false;
    [SerializeField] private GameObject prefab;
    [SerializeField] private List<string> colliderTags = new List<string>();
    private Vector3 mousePos;
    private Camera cam;
    private GameObject previewObject;
    private GameObject currentObject;
    private Color previewColour;
    private bool objectPlacementActive = true;

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
        RotateObject();

        mousePos = Input.mousePosition;
        Vector3 worldpos = cam.ScreenToWorldPoint(mousePos);

        previewObject.transform.position = new Vector3(worldpos.x, worldpos.y, 0);
        
        if(Input.GetButtonDown("Fire1") && objectPlacementActive && !isColliding)
        {
            currentObject = Instantiate(prefab, new Vector3(worldpos.x, worldpos.y, 0), previewObject.transform.rotation);
        }
    }

    void RotateObject()
    {
        int clicks = (int)Mathf.Round(Input.GetAxis("Mouse ScrollWheel") * 100);

        if(clicks != 0)
        {
            Vector3 rotationVector = previewObject.transform.rotation.eulerAngles;
            rotationVector.z += clicks;
            previewObject.transform.rotation = Quaternion.Euler(rotationVector);
        }
    }

    public void setPlacementMode(bool placementState)
    {
        objectPlacementActive = placementState;
    }
}
