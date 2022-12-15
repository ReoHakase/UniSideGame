using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmikBlock : MonoBehaviour
{
  public float detectionRadius = 0.0f;
  public bool shouldDelete = false;
  bool isFell = false;
  public float fadeOutDuration = 0.5f; // in seconds
  float leftDuration = 1.0f;

  // Start is called before the first frame update
  void Start()
  {
    // Disable physics
    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    // Set leftDuration to fadeOutDuration
    leftDuration = fadeOutDuration;
  }

  // Update is called once per frame
  void Update()
  {
    // Get an player object
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    // If an player object exists and the player is in the detection radius
    if (
      player != null
      && Vector2.Distance(player.transform.position, transform.position)
        < detectionRadius
    )
    {
      Debug.Log(
        "Player is in the detection radius, distance: "
          + Vector2.Distance(player.transform.position, transform.position)
      );
      // Enable physics
      GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    if (isFell)
    {
      // Decrement leftDuration
      leftDuration -= Time.deltaTime;
      // Set the alpha value of the sprite while preserve the color
      GetComponent<SpriteRenderer>().color = new Color(
        GetComponent<SpriteRenderer>().color.r,
        GetComponent<SpriteRenderer>().color.g,
        GetComponent<SpriteRenderer>().color.b,
        leftDuration / fadeOutDuration
      );

      if (leftDuration <= 0.0f)
      {
        // If the leftDuration is less than or equal to 0.0f
        // Destroy the object
        Destroy(gameObject);
      }
    }
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (shouldDelete)
    {
      isFell = true;
    }
  }
}
