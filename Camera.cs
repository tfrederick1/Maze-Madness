using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    GameObject player;
    float new_x;
    float new_y;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            player = GameObject.FindGameObjectWithTag("Player");
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1f);
        }

        catch (NullReferenceException E) { };
    }


    void LateUpdate()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        new_x = player.transform.position.x;
        new_y = player.transform.position.y;
        transform.position = new Vector3(new_x, new_y, -1f);
    }
}
