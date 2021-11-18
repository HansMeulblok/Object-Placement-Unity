using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject3D : MonoBehaviour
{
    [HideInInspector]   
    public List<string> tags = new List<string>();
    [HideInInspector]
    public ObjectPlacement objectPlacement;
    private string currentTag;
    private MeshRenderer meshRenderer;
    private Color originalColor;
 

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < tags.Count; i++)
        {
            if(tags[i] == other.tag)
            {
                // Colour red
                meshRenderer.material.color = new Color (255, 0, 0, 0.5f);
                objectPlacement.isColliding = true;
                // save string
                currentTag = other.tag;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {

        if(currentTag != null && other.CompareTag(currentTag))
        {
            objectPlacement.isColliding = false;
            meshRenderer.material.color = originalColor;
        }
    }
}
