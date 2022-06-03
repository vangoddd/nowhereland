using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScreenFade : MonoBehaviour {
  public Image image;
  public Action passedAction;

  void Awake() {
    DontDestroyOnLoad(gameObject);
  }

  void Start() {
    LeanTween.value(gameObject, Color.clear, Color.black, 2f).setLoopPingPong(1).setIgnoreTimeScale(true).setOnUpdate((Color val) => {
      image.color = val;
      if (val == Color.black) {
        passedAction.Invoke();
      }
    }).setOnComplete(() => {
      Destroy(gameObject);
    });
  }

  public void SetAction(Action newAction) {
    passedAction = newAction;
  }
}
