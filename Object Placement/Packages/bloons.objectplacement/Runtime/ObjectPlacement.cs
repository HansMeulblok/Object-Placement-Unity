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
    public bool Mode2D = true;
    public bool singlePlacement;
    [SerializeField] public GameObject prefab2D;
    [SerializeField] public GameObject prefab3D;
    [SerializeField] private List<string> colliderTags = new List<string>();
    private Vector3 mousePos;
    private Camera cam;
    private GameObject previewObject;
    private GameObject currentObject;
    private Color previewColour;
    public bool objectPlacementActive = true;
    private int layermask = 7;

    void Start()
    {
        cam = Camera.main;

        if(Mode2D)
        {
            Debug.Log("2D mode");
            Init2DPreviewObject(); 
        }
        else
        {
            Debug.Log("3D mode");
            Init3DPreviewObject();
        }
    }

    void Update()
    {
        if(!objectPlacementActive)
        {
            previewObject.SetActive(false);
            return;
        }
        else
        {
            previewObject.SetActive(true);
        }
        
        if(Mode2D)
        {
            Update2DPreview();
        }
        else
        {
            Update3DPreview();
        }

        mousePos = Input.mousePosition;
        if(mousePos == Vector3.zero)
        {
            worldPos = Vector3.zero;
        }
        else
        {
            worldPos = cam.ScreenToWorldPoint(mousePos);
        }

        
        if(Input.GetButtonDown("Fire1") && objectPlacementActive && !isColliding)
        {
            SpawnObject();
            if(singlePlacement)
            {
                objectPlacementActive = false;
            }
        }
    }

    void RotateObject()
    {
        if((int)Mathf.Round(Input.GetAxis("Mouse ScrollWheel") * 100) > 0 )
        {
            scrollWheelInput = (int)Mathf.Round(Input.GetAxis("Mouse ScrollWheel") * 100);
        }
        else
        {
            scrollWheelInput = 0;
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
        if(Mode2D)
        {
            currentObject = Instantiate(prefab2D, previewObject.transform.position, previewObject.transform.rotation);
        }
        else
        {
            currentObject = Instantiate(prefab3D, previewObject.transform.position, previewObject.transform.rotation);
        }
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
    
    public void setPreviewObject(GameObject go)
    {
        previewObject = go;
        if(Mode2D)
        {
            Init2DPreviewObject();
        }
        else
        {
            Init3DPreviewObject();
        }
    }

    public bool IsColliding()
    {
        return isColliding;
    }

    public void AddTag(string tag)
    {
        colliderTags.Add(tag);
    }

    private void Update2DPreview()
    {
        RotateObject();
        previewObject.transform.position = new Vector3(worldPos.x, worldPos.y, 0);
    }

    private void Update3DPreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, layermask))
        {
            previewObject.transform.position = hit.point;
        }
    }

    private void Init2DPreviewObject()
    {
        if(previewObject != null)
        {
            Destroy(previewObject);
        }

        previewObject = Instantiate(prefab2D, new Vector3(0, 0, 0), Quaternion.identity);

        previewColour = previewObject.GetComponent<SpriteRenderer>().color;
        previewColour.a = 0.5f;
        previewObject.GetComponent<SpriteRenderer>().color = previewColour;

        PreviewObject2D pv = previewObject.AddComponent<PreviewObject2D>();
        
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
        previewObject.SetActive(false);
    }

    private void Init3DPreviewObject()
    {
        if(previewObject != null)
        {
            Destroy(previewObject);
        }

        previewObject = Instantiate(prefab3D, new Vector3(0, 0, 0), Quaternion.identity);
        previewColour = previewObject.GetComponent<MeshRenderer>().material.color;
        previewColour.a = 0.5f;
        previewObject.GetComponent<MeshRenderer>().material.color = previewColour;

        PreviewObject3D pv = previewObject.AddComponent<PreviewObject3D>();

        pv.tags = colliderTags;
        pv.objectPlacement = this.gameObject.GetComponent<ObjectPlacement>();

        if(previewObject.GetComponent<Collider>() == null)
        {
            BoxCollider col = previewObject.AddComponent<BoxCollider>();
            col.isTrigger = true;
        }
        else
        {
            previewObject.GetComponent<Collider>().isTrigger = true;
        }

        if(previewObject.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = previewObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.drag = 0;
        }
        else
        {
            previewObject.GetComponent<Rigidbody>().useGravity = false;
        }
        previewObject.SetActive(false);

    }
}
