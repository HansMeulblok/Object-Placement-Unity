using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera cam;

    [SerializeField] private GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        Vector3 worldpos = cam.ScreenToWorldPoint(mousePos);
        Debug.Log(worldpos);

        if(Input.GetButtonDown("Fire1"))
        {
            //instantiate
            Instantiate(prefab, new Vector3(worldpos.x, worldpos.y, 0), Quaternion.identity);
        }
 
    }
}
