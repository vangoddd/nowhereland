using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleAnimation : MonoBehaviour {

  public Image image;

  void Start() {
    LeanTween.value(gameObject, 0f, 1f, 5f).setEase(LeanTweenType.linear).setIgnoreTimeScale(true).setOnUpdate((float val) => {
      image.color = new Color(1f, 1f, 1f, val);
    });
  }


}
