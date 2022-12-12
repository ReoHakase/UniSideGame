using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public GameObject mainImage;
  public Sprite gameOverSprite;
  public Sprite gameClearSprite;
  public GameObject panel;
  public GameObject restartButton;
  public GameObject nextButton;
  UnityEngine.UI.Image titleImage;
  TimeController timeController;

  public GameObject timeBar;
  public TMPro.TMP_Text timeText;
  public GameObject scoreText;
  public static int totalScore = 0; // 以前のステージまででの合計スコア
  public int stageScore = 0;

  // Start is called before the first frame update
  void Start()
  {
    // Call a method named `InactiveImage` after 1 second.
    Invoke(nameof(InactiveImage), 1.0f);

    panel.SetActive(false);

    timeController = this.GetComponent<TimeController>();
    if (timeController.isDisabled)
    {
      timeBar.SetActive(false); // Hide time bar and its children objects if the time function is disabled.
    }

    UpdateScoreText();
  }

  // Update is called once per frame
  void Update()
  {
    if (PlayerController.gameStatus == GameStatus.GameOver)
    {
      // Game Over
      panel.SetActive(true);

      titleImage = mainImage.GetComponent<UnityEngine.UI.Image>();
      titleImage.sprite = gameOverSprite;
      mainImage.SetActive(true);

      restartButton.SetActive(true);
      nextButton.SetActive(false);

      if (timeController != null)
      {
        timeController.shouldCount = false;
      }

      PlayerController.gameStatus = GameStatus.GameFinished;
    }
    else if (PlayerController.gameStatus == GameStatus.GameCleared)
    {
      // Game Cleared
      panel.SetActive(true);

      titleImage = mainImage.GetComponent<UnityEngine.UI.Image>();
      titleImage.sprite = gameClearSprite;
      mainImage.SetActive(true);

      restartButton.SetActive(true);
      nextButton.SetActive(true);

      if (timeController != null)
      {
        timeController.shouldCount = false;
        AddScore((int)timeController.GetCurrentTime() * 10); // 残り1秒あたり10ポイントだけ加算
      }

      SumUpScore(); // ステージクリア時にスコアを合計に加算

      PlayerController.gameStatus = GameStatus.GameFinished;
    }
    else if (PlayerController.gameStatus == GameStatus.Playing)
    {
      GameObject player = GameObject.FindGameObjectWithTag("Player");
      PlayerController playerController = player.GetComponent<PlayerController>();

      if (timeController != null)
      {
        if (!timeController.isDisabled)
        {
          timeText.SetText(timeController.GetText());

          if (timeController.GetCurrentTime() <= 0.0f)
          {
            playerController.GameOver();
          }
        }

      }
    }
  }

  void InactiveImage()
  {
    mainImage.SetActive(false);
  }

  void UpdateScoreText()
  {
    string displayedScore = (totalScore + stageScore).ToString();
    scoreText.GetComponent<TMPro.TMP_Text>().SetText(displayedScore);
    Debug.Log("Updated Score: " + displayedScore);
  }

  void SumUpScore()
  {
    totalScore += stageScore;
    stageScore = 0;
    UpdateScoreText();
  }

  public void AddScore(int score)
  {
    stageScore += score;
    UpdateScoreText();
  }
}
