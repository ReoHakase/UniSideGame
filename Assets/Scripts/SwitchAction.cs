using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwitchTarget
{
  void OnSwitchChanged(bool isEnabled);
}

public class SwitchAction : MonoBehaviour
{
  public GameObject[] targets;
  public Sprite enabledSprite;
  public Sprite disabledSprite;

  [SerializeField]
  bool isEnabled = false;

  // Start is called before the first frame update
  void Start()
  {
    UpdateSprite();
  }

  // Update is called once per frame
  void Update() { }

  private void OnTriggerEnter2D(Collider2D other)
  {
    // プレイヤーが触れたら
    if (other.gameObject.CompareTag("Player"))
    {
      // スイッチの状態を反転
      isEnabled = !isEnabled;

      // スイッチの状態に応じてスプライトを変更
      UpdateSprite();

      // スイッチの状態に応じてターゲットの状態を変更
      foreach (GameObject target in targets)
      {
        target.GetComponent<ISwitchTarget>().OnSwitchChanged(isEnabled);
      }
    }
  }

  private void UpdateSprite()
  {
    if (isEnabled)
    {
      GetComponent<SpriteRenderer>().sprite = enabledSprite;
    }
    else
    {
      GetComponent<SpriteRenderer>().sprite = disabledSprite;
    }
  }
}
