using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeText : MonoBehaviour {
  public TextMeshProUGUI text;
  private Color originalColor;
  void Start() {
    originalColor = text.color;
    LeanTween.value(text.gameObject, originalColor, Color.clear, 3f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeInCubic).setLoopPingPong().setOnUpdate((Color col) => {
      text.color = col;
    });
  }
}
