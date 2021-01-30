using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBoy : MonoBehaviour
{
    private RectTransform _transform;
   
    void Start()
    {
        _transform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float rot = 0.1f;
        
        _transform.localScale += new Vector3(0.1f, 1.0f, 1.0f);
    }
}
