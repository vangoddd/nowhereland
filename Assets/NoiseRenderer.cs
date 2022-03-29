using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class NoiseRenderer : MonoBehaviour {

  public int mapSize, seed, octaves;
  public float magnification, presistance, lacunarity;

  public float cutoff;

  public Texture2D tex;
  // Start is called before the first frame update
  void Start() {
    tex = generateTexture();
    tex.filterMode = FilterMode.Point;
    GetComponent<RawImage>().texture = tex;
  }

  [ContextMenu("Regenerate Texture")]
  public void RegenerateTexture() {
    tex = generateTexture();
    GetComponent<RawImage>().texture = tex;
  }

  Texture2D generateTexture() {
    Texture2D texture = new Texture2D(mapSize, mapSize, TextureFormat.ARGB32, false);

    List<List<float>> rawNoiseData;
    NoiseGenerator ng = new NoiseGenerator();
    ng.GenerateNoise(mapSize, seed, magnification, octaves, presistance, lacunarity);
    rawNoiseData = ng.rawNoiseData;

    for (int x = 0; x < mapSize; x++) {
      for (int y = 0; y < mapSize; y++) {
        //Debug.Log(rawNoiseData[x][y]);
        texture.SetPixel(x, y, GetNoiseColor(rawNoiseData[x][y]));
      }
    }
    texture.Apply();

    return texture;
  }

  Color GetNoiseColor(float val) {
    if (val > cutoff) {
      return Color.white;
    } else {
      return Color.black;
    }
    //return new Color(val, val, val, 1f);
  }
}



[CustomEditor(typeof(NoiseRenderer))]
public class NoiseRendererEditor : Editor {
  public override void OnInspectorGUI() {
    base.OnInspectorGUI();

    NoiseRenderer script = (NoiseRenderer)target;
    script.RegenerateTexture();
  }
}
