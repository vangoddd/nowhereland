using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUIButtonHandler : MonoBehaviour {
  public GameEvent OnPlusButtonClicked;
  public GameEvent OnMinusButtonClicked;

  public void OnPlusClicked() {
    OnPlusButtonClicked.Raise();
  }

  public void OnMinusClicked() {
    OnMinusButtonClicked.Raise();
  }
}
