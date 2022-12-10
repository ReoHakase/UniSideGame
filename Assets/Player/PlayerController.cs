using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rbody;



    [SerializeField] float horizontalMoveSpeed = 3.0f;
    [SerializeField] float jumpForce = 3.0f;

    public LayerMask groundLayer;
    bool isOnGround = false;
    bool shouldJump = false;

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

        if(axisH > 0)
        {
            transform.localScale = new Vector2(1.0f, 1.0f);
            //transform.Rotate(new Vector3(0.0f, 30.0f, 0.0f));
        }
        else if(axisH < 0)
        {
            transform.localScale = new Vector2(-1.0f, 1.0f);
            //transform.Rotate(new Vector3(0.0f, -30.0f, 0.0f));
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void FixedUpdate() {

        isOnGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundLayer);

        if(isOnGround || axisH != 0)
        {
            // Update the horizontal velocity of the player
            rbody.velocity = new Vector2(axisH * horizontalMoveSpeed, rbody.velocity.y);
        }

        if(isOnGround && shouldJump)
        {
            Debug.Log("Jump!");
            rbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            shouldJump = false;
        }



    }

    void Jump()
    {
        shouldJump = true;
        Debug.Log("An certain jump force will exert on Player on a next fixedUpdate execution.");
    }
}
