using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderMaster : MonoBehaviour
{
    public List<Material> materials;
    Material originalMat;

    public bool activateOnStart = false;
    public int _matIndex;
    public float _duration;
   

    void Start()
    {
        originalMat = GetComponent<SpriteRenderer>().material;
        SetShader(_matIndex, _duration);
        
    }


    public void SetShader(int index, float duration = 0)
    {
        _matIndex = index;
        _duration = duration;
        ActivateMaterial();
    }

    private void ActivateMaterial()
    {
        GetComponent<SpriteRenderer>().material = materials[_matIndex];
        if (_duration != 0)
        {
            Invoke("ResetMaterial", _duration);
        }
    }


    public void ResetMaterial()
    {
        GetComponent<SpriteRenderer>().material = originalMat;
    }
}
