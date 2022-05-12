using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButton : MonoBehaviour {
  public GameEvent OnMapButton;
  public void OnClick() {
    OnMapButton.Raise();
  }
}
