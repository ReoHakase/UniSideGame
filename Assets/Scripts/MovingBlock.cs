using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour, ISwitchTarget
{
  public bool shouldMoveWhenPlayerRides = false;
  public bool shouldStopWhenPlayerLeaves = false;
  public bool shouldMove = false;

  Vector2 initialPosition = Vector2.zero;
  public Vector2 delta = Vector2.zero;
  public float moveDuration = 1.0f; // in seconds
  public float stopDelay = 1.0f; // in seconds

  Vector2 deltaPerFrame = Vector2.zero;
  bool isReverse = false; // If true, the block is moving to the initial position, from delta position.

  // Start is called before the first frame update
  void Start()
  {
    initialPosition = (Vector2)transform.position;
    UpdateDeltaPerFrame();
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    if (shouldMove)
    {
      // 移動
      if (!isReverse)
      {
        // 正方向への移動(initial -> initial + delta)
        transform.Translate((Vector3)deltaPerFrame);
      }
      else
      {
        // 逆方向への移動(initial + delta -> initial)
        transform.Translate((Vector3)(deltaPerFrame * -1));
      }

      // 現在位置が初期位置とdeltaの間にあるかどうかを判定
      // 現在位置から見て、初期位置とdeltaが違う側にある場合(内積が負)は、その間にあると判定
      bool isBetweenInitialAndDelta =
        Vector2.Dot(
          initialPosition - (Vector2)transform.position,
          (initialPosition + delta) - (Vector2)transform.position
        ) < 0;

      if (isBetweenInitialAndDelta)
      {
        // その間にある場合
        // 何もしない
      }
      else
      {
        // その間にない場合
        // 正しい範囲内に収める
        if (!isReverse)
        {
          // 正方向への移動をしていた場合、行き先のdeltaの位置に移動
          transform.position = (Vector3)(initialPosition + delta);
        }
        else
        {
          // 逆方向への移動をしていた場合、初期位置に移動
          transform.position = (Vector3)initialPosition;
        }

        // 移動方向を反転させる
        isReverse = !isReverse;
        // 移動を停止
        shouldMove = false;
        // 停止後の遅延時間を設定
        Invoke(nameof(Move), stopDelay);
      }
    }
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    // もし接触したのがプレイヤーだったら
    if (collision.gameObject.CompareTag("Player"))
    {
      // プレイヤーの親をこのオブジェクトに設定
      collision.gameObject.transform.SetParent(transform);
      if (shouldMoveWhenPlayerRides)
      {
        // プレイヤーが乗っている場合、移動を開始
        Move();
        CancelInvoke(nameof(Stop));
      }
    }
  }

  void OnCollisionExit2D(Collision2D collision)
  {
    // もし接触していたのがプレイヤーだったら
    if (collision.gameObject.CompareTag("Player"))
    {
      // プレイヤーの親を解除
      collision.gameObject.transform.SetParent(null);
      if (shouldStopWhenPlayerLeaves)
      {
        // プレイヤーが乗っていない場合、移動を停止
        Invoke(nameof(Stop), stopDelay);
      }
    }
  }

  void Move()
  {
    shouldMove = true;
  }

  void Stop()
  {
    shouldMove = false;
  }

  void UpdateDeltaPerFrame()
  {
    float timestep = Time.fixedDeltaTime;
    deltaPerFrame = delta * timestep / moveDuration;
  }

  public void OnSwitchChanged(bool isEnabled)
  {
    Debug.Log(
      "A MovingBlock handled a switch event with isEnabled = " + isEnabled + "."
    );
    shouldMove = isEnabled;
  }
}
