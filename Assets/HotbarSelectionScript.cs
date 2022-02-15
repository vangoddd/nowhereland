using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSelectionScript : MonoBehaviour {
  [SerializeField] private RectTransform rectTransform;

  private int initialX = -60;
  private int gap = 24;

  public int selection = 0;

  public void ChangeSelection(int index) {
    rectTransform.localPosition = new Vector3(initialX + (index * gap), rectTransform.localPosition.y, 0f);
    selection = index;
  }

  void Update() {
    ChangeSelection(selection);
    if (selection > 5) selection = 0;
    if (selection < 0) selection = 5;
  }
}
