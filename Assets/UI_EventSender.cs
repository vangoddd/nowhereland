using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EventSender : MonoBehaviour {
  public GameEvent OnUIOpen;
  public GameEvent OnUIClose;

  void OnEnable() {
    OnUIOpen.Raise();
  }

  void OnDisable() {
    OnUIClose.Raise();
  }
}
