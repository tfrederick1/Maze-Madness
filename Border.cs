using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(sr.color.r - 1f/60f > 0)
        {
            sr.color = sr.color - new Color(1f / 60f, 1f / 60f, 1f / 60f);
        }
        */
    }
}
