using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    Animator animator;
    int state;
    Rigidbody2D rb;
    int coins;

    // UI Stuff
    GameObject Counter;
    TextMeshProUGUI counter;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        state = 0;
        coins = 0;

        Counter = GameObject.Find("Counter");
        counter = Counter.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //========== Moving and Animating ==========//
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                state = 1;
                transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + .09f, gameObject.transform.position.z);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                state = 1;
                transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - .09f, gameObject.transform.position.z);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                state = 1;
                transform.position = new Vector3(gameObject.transform.position.x + .09f, gameObject.transform.position.y, gameObject.transform.position.z);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                state = 1;
                transform.position = new Vector3(gameObject.transform.position.x - .09f, gameObject.transform.position.y, gameObject.transform.position.z);
            }
        }

        else
        {
            state = 0;
        }

        animator.SetInteger("State", state);

        //========== Raycast ==========//
        Vector2 p = transform.position;
       
        List<RaycastHit2D> rays = new List<RaycastHit2D>();

        for(int i = 0; i < 60; i++)
        {
            Debug.DrawRay(p - new Vector2(.5f, 0), new Vector2(Mathf.Cos(i * 6 * Mathf.PI / 180f), Mathf.Sin(i * 6 * Mathf.PI / 180f)) * 10f, Color.red);
            rays.Add(Physics2D.Raycast(p - new Vector2(.5f, 0), new Vector2(Mathf.Cos(i * 6 * Mathf.PI / 180f), Mathf.Sin(i * 6 * Mathf.PI / 180f)), 10f, LayerMask.GetMask("Default")));
        }

        for(int i = 0; i < rays.Count; i++)
        {
            if(rays[i].collider != null)
            {
                SpriteRenderer sr = rays[i].collider.gameObject.GetComponent<SpriteRenderer>();

                sr.color = sr.color + new Color((1f / 60f), (1f / 60f), (1f / 60f));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coins++;
            counter.text = "x" + coins;
            Debug.Log(coins);
        }
    }
}
