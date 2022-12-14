using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameSceneName
{
  Stage1,
  Stage2,
  Stage3,
  Title,
  Result
}

public class SceneManager : MonoBehaviour
{
  public GameSceneName currentGameScene = GameSceneName.Stage1;

  // Start is called before the first frame update
  void Start() { }

  // Update is called once per frame
  void Update() { }

  public void Load()
  {
    UnityEngine.SceneManagement.SceneManager.LoadScene(
      currentGameScene.ToString()
    );
  }
}
