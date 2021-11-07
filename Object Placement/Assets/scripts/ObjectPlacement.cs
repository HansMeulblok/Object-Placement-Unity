using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    [HideInInspector] 
    public bool isColliding =  false;
    [HideInInspector] 
    public Vector3 worldPos;
    [HideInInspector] 
    public int scrollWheelInput;
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
        InitPreviewObject(); 
    }

    void Update()
    {
        RotateObject();

        mousePos = Input.mousePosition;
        if(mousePos == Vector3.zero)
        {
            worldPos = Vector3.zero;
        }
        else
        {
            worldPos = cam.ScreenToWorldPoint(mousePos);
        }

        previewObject.transform.position = new Vector3(worldPos.x, worldPos.y, 0);
        
        if(Input.GetButtonDown("Fire1") && objectPlacementActive && !isColliding)
        {
            SpawnObject();
        }
    }

    void RotateObject()
    {
        if((int)Mathf.Round(Input.GetAxis("Mouse ScrollWheel") * 100) > 0 )
        {
            scrollWheelInput = (int)Mathf.Round(Input.GetAxis("Mouse ScrollWheel") * 100);
        }

        if(scrollWheelInput != 0)
        {
            Vector3 rotationVector = previewObject.transform.rotation.eulerAngles;
            rotationVector.z += scrollWheelInput;
            previewObject.transform.rotation = Quaternion.Euler(rotationVector);
        }
    }
    public void SpawnObject()
    {
        currentObject = Instantiate(prefab, previewObject.transform.position, previewObject.transform.rotation);
    }

    public void setPlacementMode(bool placementState)
    {
        objectPlacementActive = placementState;
    }
    public GameObject GetPreviewObject()
    {
        return previewObject;
    }

    public GameObject GetCurrentObject()
    {
        return currentObject;
    }

    private void InitPreviewObject()
    {
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
}
