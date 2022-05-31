using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Start mode")]
public class StartMode : ScriptableObject {
  public bool loadGame = false;

  void OnEnable() {
    loadGame = false;
  }
}
