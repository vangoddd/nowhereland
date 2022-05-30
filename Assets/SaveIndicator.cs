using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveIndicator : MonoBehaviour {
  public Animator animator;
  public Image image;

  public void TriggerIndicator() {
    image.color = new Color(1f, 1f, 1f, 1f);
    animator.SetTrigger("show");
  }

  public void HideIndicator() {
    image.color = new Color(1f, 1f, 1f, 0f);
  }
}
