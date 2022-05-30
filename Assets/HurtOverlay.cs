using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HurtOverlay : MonoBehaviour {
  public Image image;
  void OnEnable() {
    LeanTween.value(gameObject, 1f, 0f, 1f).setOnUpdate((float val) => {
      image.color = new Color(1f, 1f, 1f, val);
    }).setOnComplete(() => {
      this.gameObject.SetActive(false);
    });
  }
}
