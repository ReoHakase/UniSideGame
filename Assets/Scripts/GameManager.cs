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

    // Start is called before the first frame update
    void Start()
    {
        // Call a method named `InactiveImage` after 1 second.
        Invoke(nameof(InactiveImage), 1.0f); // DO
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.gameStatus == GameStatus.GameOver)
        {
            // Game Over
            panel.SetActive(true);

            titleImage = mainImage.GetComponent<UnityEngine.UI.Image>();
            titleImage.sprite = gameOverSprite;
            mainImage.SetActive(true);

            restartButton.SetActive(true);
            nextButton.SetActive(false);
            PlayerController.gameStatus = GameStatus.GameFinished;
        }
        else if(PlayerController.gameStatus == GameStatus.GameCleared)
        {
            // Game Cleared
            panel.SetActive(true);

            titleImage = mainImage.GetComponent<UnityEngine.UI.Image>();
            titleImage.sprite = gameClearSprite;
            mainImage.SetActive(true);

            restartButton.SetActive(true);
            nextButton.SetActive(true);
            PlayerController.gameStatus = GameStatus.GameFinished;
        }
    }

    void InactiveImage()
    {
        mainImage.SetActive(false);
    }
}
