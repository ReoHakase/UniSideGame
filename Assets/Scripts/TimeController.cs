using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeCountMode
{
  CountDown,
  CountUp
}

public class TimeController : MonoBehaviour
{
  public bool isDisabled = false;
  public bool shouldCount = false;
  public TimeCountMode timeCountMode = TimeCountMode.CountDown;
  public float initialTime = 60.0f; // in seconds, will be used only when timeCountMode is CountDown, otherwise it will be ignored.
  public float maxTime = 0.0f; // in seconds, will be used only when timeCountMode is CountUp, otherwise it will be ignored.
  float currentTime = 0.0f; // in seconds

  // Start is called before the first frame update
  void Start()
  {
    switch (timeCountMode)
    {
      case TimeCountMode.CountDown:
        currentTime = initialTime;

        break;
      case TimeCountMode.CountUp:
        currentTime = 0.0f;
        break;
    }
  }

  // Update is called once per frame
  void Update()
  {
    switch (timeCountMode)
    {
      case TimeCountMode.CountDown:
        currentTime -= Time.deltaTime;
        if (currentTime <= 0.0f)
        {
          Debug.Log("CountDown: currentTime has reached 0.0f!");
          currentTime = 0.0f;
          // PlayerController.gameStatus = GameStatus.GameOver;
        }
        break;
      case TimeCountMode.CountUp:
        currentTime += Time.deltaTime;
        if (currentTime >= maxTime)
        {
          Debug.Log("CountDown: currentTime has reached limitTime!");
          currentTime = maxTime;
          // PlayerController.gameStatus = GameStatus.GameOver;
        }
        break;
    }
    // Debug.Log("Current time: " + currentTime + " seconds.");

  }

  public string GetText()
  {
    return ((int)currentTime).ToString();
  }

  public float GetCurrentTime()
  {
    return currentTime;
  }
}
