using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChooser : MonoBehaviour
{
    public List<Sprite> AllSprites;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr) sr.sprite = AllSprites[Random.Range(0, AllSprites.Count)];
    }

}
