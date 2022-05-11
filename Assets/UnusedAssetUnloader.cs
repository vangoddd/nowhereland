using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnusedAssetUnloader : MonoBehaviour {
  void Start() {
    // Debug.Log("unloader start method");
    // Unload();
  }

  public void Unload() {
    Resources.UnloadUnusedAssets();
  }
}
