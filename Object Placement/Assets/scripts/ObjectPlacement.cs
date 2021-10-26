using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera cam;

    [SerializeField] private GameObject prefab;
    private GameObject previewObject;
    private Color previewColour;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        previewObject = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        previewColour = previewObject.GetComponent<SpriteRenderer>().color;
        previewColour.a = 0.5f;
        previewObject.GetComponent<SpriteRenderer>().color = previewColour;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        Vector3 worldpos = cam.ScreenToWorldPoint(mousePos);

        previewObject.transform.position = new Vector3(worldpos.x, worldpos.y, 0);

        if(Input.GetButtonDown("Fire1"))
        {
            //instantiate
            Instantiate(prefab, new Vector3(worldpos.x, worldpos.y, 0), Quaternion.identity);
        }
 
    }
}
