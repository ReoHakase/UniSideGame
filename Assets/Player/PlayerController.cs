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

public enum GameStatus
{
  Playing,
  GameCleared,
  GameOver,
  GameFinished
}

public class PlayerController : MonoBehaviour
{

  Rigidbody2D rbody;

  GameManager gameManager;

  [SerializeField] float horizontalMoveSpeed = 3.0f;
  [SerializeField] float floatingMoveSpeedMultiplier = 0.5f;
  [SerializeField] float jumpForce = 3.0f;

  public LayerMask groundLayer;
  bool isOnGround = false;
  bool shouldJump = false;

  Animator animator;
  AnimationClipName currentAnime = AnimationClipName.PlayerStop;
  AnimationClipName prevAnime = AnimationClipName.PlayerStop;

  public static GameStatus gameStatus = GameStatus.Playing;
  float axisH = 0.0f;

  // Start is called before the first frame update
  void Start()
  {
    rbody = this.GetComponent<Rigidbody2D>();
    animator = this.GetComponent<Animator>();
    gameManager = GameObject.Find("Canvas").GetComponent<GameManager>();
    gameStatus = GameStatus.Playing;
  }

  // Update is called once per frame
  void Update()
  {
    if (gameStatus != GameStatus.Playing)
    {
      return; // Do nothing if the game is not in a playing status.
    }


    axisH = Input.GetAxis("Horizontal");

    if (axisH > 0)
    {
      transform.localScale = new Vector2(1.0f, 1.0f);
      //transform.Rotate(new Vector3(0.0f, 30.0f, 0.0f));
    }
    else if (axisH < 0)
    {
      transform.localScale = new Vector2(-1.0f, 1.0f);
      //transform.Rotate(new Vector3(0.0f, -30.0f, 0.0f));
    }

    if (Input.GetButtonDown("Jump"))
    {
      Jump();
    }
  }

  void FixedUpdate()
  {

    if (gameStatus != GameStatus.Playing)
    {
      return; // Do nothing if the game is not in a playing status.
    }


    // Check if the player is on the ground
    isOnGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundLayer);

    if (isOnGround)
    {
      rbody.velocity = new Vector2(axisH * horizontalMoveSpeed, rbody.velocity.y);
    }
    else if (axisH != 0)
    {
      if (rbody.velocity.x * axisH < 0)
      {
        rbody.velocity = new Vector2(axisH * horizontalMoveSpeed * floatingMoveSpeedMultiplier, rbody.velocity.y);
      }
      else
      {
        rbody.velocity = new Vector2(axisH * horizontalMoveSpeed, rbody.velocity.y);
      }
    }

    if (isOnGround && shouldJump)
    {
      Debug.Log("Jump!");
      rbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
      shouldJump = false;
    }

    // Set next animation
    if (isOnGround)
    {
      if (axisH == 0)
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
    if (currentAnime != prevAnime)
    {
      animator.Play(currentAnime.ToString());
      prevAnime = currentAnime;
    }


  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    Debug.Log("Collision with " + collision.gameObject.tag);
    switch (collision.gameObject.tag)
    {
      case "Goal":
        Goal();
        break;
      case "Dead":
        GameOver();
        break;
      case "ScoreItem":
        ScoreItemManager scoreItemManager = collision.gameObject.GetComponent<ScoreItemManager>();
        Debug.Log("Acquired " + scoreItemManager.scoreValue + " points.");
        gameManager.AddScore(scoreItemManager.scoreValue);
        Destroy(collision.gameObject);
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
    Debug.Log("Goal");
    animator.Play(AnimationClipName.PlayerGoal.ToString());
    gameStatus = GameStatus.GameCleared;
    GameStop();

  }

  public void GameOver()
  {
    Debug.Log("Game Over");

    if (gameStatus == GameStatus.GameCleared)
    {
      Debug.Log("Game Over effect was cancelled due to the game already cleared.");
      return; // Do nothing if the game is already cleared.
    }

    animator.Play(AnimationClipName.PlayerOver.ToString());
    gameStatus = GameStatus.GameOver;
    GameStop();

    // Show GameOver effect
    // Disable player collider
    CapsuleCollider2D playerCollider2d = this.GetComponent<CapsuleCollider2D>();
    playerCollider2d.enabled = false;
    rbody.AddForce(Vector2.up * 5.0f, ForceMode2D.Impulse);
  }

  void GameStop()
  {
    Debug.Log("Game Stopped");
    rbody.velocity = new Vector2(0, 0); // Force player to stop
  }
}
