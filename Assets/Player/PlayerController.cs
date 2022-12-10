using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AnimationClipName
{
    PlayerStop,
    PlayerMove,
    PlayerJump,
    PlayerGoal,
    PlayerOver
}

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rbody;



    [SerializeField] float horizontalMoveSpeed = 3.0f;
    [SerializeField] float jumpForce = 3.0f;

    public LayerMask groundLayer;
    bool isOnGround = false;
    bool shouldJump = false;

    Animator animator;
    AnimationClipName currentAnime = AnimationClipName.PlayerStop;
    AnimationClipName prevAnime = AnimationClipName.PlayerStop;

    float axisH = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
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

        // Set next animation
        if (isOnGround)
        {
            if(axisH == 0)
            {
                currentAnime = AnimationClipName.PlayerStop;
            }
            else
            {
                currentAnime = AnimationClipName.PlayerMove;
            }
        }
        else
        {
            currentAnime = AnimationClipName.PlayerJump;
        }
        if(currentAnime != prevAnime)
        {
            animator.Play(currentAnime.ToString());
            prevAnime = currentAnime;
        }
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Goal":
                Goal();
                break;
            case "Dead":
                GameOver();
                break;
        }
    }

    void Jump()
    {
        shouldJump = true;
        Debug.Log("An certain jump force will exert on Player on a next fixedUpdate execution.");
    }

    void Goal()
    {
        animator.Play(AnimationClipName.PlayerGoal.ToString());
    }

    void GameOver()
    {
        animator.Play(AnimationClipName.PlayerOver.ToString());
    }
}
