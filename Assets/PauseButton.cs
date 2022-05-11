using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour {
  public GameEvent onPauseButton;
  public void OnClick() {
    //TimeManager.Instance.togglePause();
    onPauseButton.Raise();
  }
}
