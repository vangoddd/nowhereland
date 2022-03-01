using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseRenderer : MonoBehaviour {

  public Texture2D tex;
  // Start is called before the first frame update
  void Start() {
    tex = generateTexture();
    tex.filterMode = FilterMode.Point;
    GetComponent<RawImage>().texture = tex;
  }

  Texture2D generateTexture() {
    Texture2D texture = new Texture2D(300, 300, TextureFormat.ARGB32, false);

    List<List<float>> rawNoiseData;
    NoiseGenerator ng = new NoiseGenerator();
    ng.GenerateNoise(300, 240, 12f, 1, 0.2f, 1f);
    rawNoiseData = ng.rawNoiseData;

    for (int x = 0; x < 300; x++) {
      for (int y = 0; y < 300; y++) {
        //Debug.Log(rawNoiseData[x][y]);
        texture.SetPixel(x, y, GetNoiseColor(rawNoiseData[x][y]));
      }
    }
    texture.Apply();

    return texture;
  }

  Color GetNoiseColor(float val) {
    if (val > 0.35) {
      return Color.white;
    } else {
      return Color.black;
    }
    //return new Color(val, val, val, 1f);
  }
}
