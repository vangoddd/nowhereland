using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour {
  public MapGenerator mapGenerator;
  public RawImage rawImage;

  void Start() {
    UpdateMap();
  }

  public void UpdateMap() {
    rawImage.texture = mapGenerator.mapTexture;
  }
}
