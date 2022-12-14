using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
  public GameObject scoreText;

  // Start is called before the first frame update
  void Start()
  {
    // `GameObject scoreText`のテキストを変更する
    Debug.Log("GameManager.totalScore: " + GameManager.totalScore);
    scoreText
      .GetComponent<TMPro.TMP_Text>()
      .SetText(GameManager.totalScore.ToString());
  }

  // Update is called once per frame
  void Update() { }
}
