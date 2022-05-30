using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MapUI : MonoBehaviour, IDragHandler {
  public MapGenerator mapGenerator;
  public RawImage rawImage;
  public RawImage fogOfWarImage;
  public RectTransform rectTransform;
  public RectTransform playerRT;
  public RectTransform playerFrameRT;
  public RectTransform fogOfWarRT;

  public PlayerStats _playerStat;

  private int mag = 4;
  public int selection = 0;

  float mapRatio;

  public int[] scale;

  public Texture2D[] magnification;
  int mapSize;

  int maxDelta = 0;

  [SerializeField] private Canvas canvas;
  [SerializeField] private Vector2 originalPos;

  public Texture2D fogTexture;

  public void InitiateMap() {
    scale = new int[mag];
    scale[0] = 1;
    for (int i = 1; i < mag; i++) {
      scale[i] = scale[i - 1] * 2;
    }

    mapSize = mapGenerator.mapSize;
    magnification = new Texture2D[mag];
    magnification[0] = mapGenerator.mapTexture;

    originalPos = rectTransform.anchoredPosition;

    GenerateMagnification();

    fogTexture = new Texture2D(mapGenerator.mapSize, mapGenerator.mapSize, TextureFormat.ARGB32, false);
    fogTexture.filterMode = FilterMode.Point;
    for (int i = 0; i < mapGenerator.mapSize; i++) {
      for (int j = 0; j < mapGenerator.mapSize; j++) {
        fogTexture.SetPixel(i, j, new Color(0.31f, 0.14f, 0.19f));
      }
    }
    fogTexture.Apply();
  }

  void OnEnable() {
    mapRatio = 150f / (float)mapGenerator.mapSize;
    UpdateMap(true);
    UpdateFog();
  }

  [ContextMenu("Update Map")]
  public void UpdateMap(bool snap) {
    rawImage.texture = magnification[selection];
    rawImage.SetNativeSize();

    rectTransform.sizeDelta = rectTransform.sizeDelta * (150f / (float)mapGenerator.mapSize);
    playerFrameRT.sizeDelta = rectTransform.sizeDelta;
    fogOfWarRT.sizeDelta = rectTransform.sizeDelta;

    maxDelta = ((magnification[selection].width / 2) - 150) / 2;
    if (maxDelta < 0) maxDelta = 0;

    Vector2 anchoredPos = _playerStat.position - new Vector2(mapGenerator.mapSize / 2, mapGenerator.mapSize / 2);

    //unscaled anchorpos
    anchoredPos *= mapRatio;

    //scaled pos
    anchoredPos *= (float)scale[selection] * -1f;

    //snap map into player
    if (snap) rectTransform.anchoredPosition = anchoredPos;

    Vector2 clampedPosition = new Vector2(Mathf.Clamp(rectTransform.anchoredPosition.x, -maxDelta, maxDelta), Mathf.Clamp(rectTransform.anchoredPosition.y, -maxDelta, maxDelta));
    rectTransform.anchoredPosition = clampedPosition;

    playerFrameRT.anchoredPosition = rectTransform.anchoredPosition;
    //playerFrameRT.sizeDelta =
    playerRT.anchoredPosition = anchoredPos * -1f;

    ScaleFog();
  }

  void GenerateMagnification() {
    for (int i = 1; i < mag; i++) {
      magnification[i] = new Texture2D(mapSize * scale[i], mapSize * scale[i], TextureFormat.ARGB32, false);
      magnification[i].filterMode = FilterMode.Point;

      for (int x = 0; x < mapSize; x++) {
        for (int y = 0; y < mapSize; y++) {
          MagnifyTexture(x, y, scale[i], ref magnification[i]);
        }
      }
      magnification[i].Apply();
    }
  }

  void MagnifyTexture(int x, int y, int scale, ref Texture2D newTexture) {
    for (int i = 0; i < scale; i++) {
      for (int j = 0; j < scale; j++) {
        newTexture.SetPixel((x * scale) + i, (y * scale) + j, magnification[0].GetPixel(x, y));
      }
    }
  }

  void UpdateFog() {
    for (int x = 0; x < mapSize; x++) {
      for (int y = 0; y < mapSize; y++) {
        if (mapGenerator.map.fogOfWarData[x, y]) {
          fogTexture.SetPixel(x, y, new Color(0f, 0f, 0f, 0f));
        }
      }
    }
    fogTexture.Apply();

    fogOfWarImage.texture = fogTexture;
  }

  //Drag handling
  public void OnDrag(PointerEventData eventData) {
    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    Vector2 clampedPosition = new Vector2(Mathf.Clamp(rectTransform.anchoredPosition.x, -maxDelta, maxDelta), Mathf.Clamp(rectTransform.anchoredPosition.y, -maxDelta, maxDelta));
    rectTransform.anchoredPosition = clampedPosition;
    playerFrameRT.anchoredPosition = rectTransform.anchoredPosition;

    ScaleFog();
  }

  void ScaleFog() {

  }

  public void OnZoom() {
    selection++;
    if (selection >= mag) {
      selection = mag - 1;
    }
    UpdateMap(false);
  }

  public void OnUnZoom() {
    selection--;
    if (selection < 0) {
      selection = 0;
    }
    UpdateMap(false);
  }
}
