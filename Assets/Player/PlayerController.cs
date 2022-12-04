using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rbody;
    float axisH = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        axisH = Input.GetAxis("Horizontal");
    }

    void FixedUpdate() {
        // Update the velocity of the player
        rbody.velocity = new Vector2(axisH * 3.0f, rbody.velocity.y);
    }
}
