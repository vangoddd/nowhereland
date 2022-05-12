using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStart : MonoBehaviour {
  public StartMode mode;

  void Start() {
    mode.loadGame = false;
  }
}
