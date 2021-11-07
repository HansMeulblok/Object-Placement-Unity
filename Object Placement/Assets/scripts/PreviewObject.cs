using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    [HideInInspector]   
    public List<string> tags = new List<string>();
    [HideInInspector]
    public ObjectPlacement objectPlacement;
    private string currentTag;
    private SpriteRenderer sprite;
    private Color originalColor;
 

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        for (int i = 0; i < tags.Count; i++)
        {
            if(tags[i] == other.tag)
            {
                // Colour red
                sprite.color = new Color (255, 0, 0, 0.5f);
                objectPlacement.isColliding = true;
                // save string
                currentTag = other.tag;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if(currentTag != null && other.CompareTag(currentTag))
        {
            objectPlacement.isColliding = false;
            sprite.color = originalColor;
        }
    }
}
