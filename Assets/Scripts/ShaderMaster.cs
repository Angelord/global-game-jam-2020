using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderMaster : MonoBehaviour
{
    public List<Material> materials;
    Material originalMat;

    private int _matIndex;
    private float _pause;
    private float _duration;
   

    void Start()
    {
        originalMat = GetComponent<SpriteRenderer>().material;
        SetShader(0, 1, 1);
        
    }



    public void SetShader(int index, float duration = 0, float pause = 0)
    {
        // no pause means the effect ends after duration
        _matIndex = index;
        _duration = duration;
        _pause = pause;
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
        if(_pause != 0)
        {
            Invoke("ActivateMaterial", _pause);
        }
    }
}
