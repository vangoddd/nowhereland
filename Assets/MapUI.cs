using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MapUI : MonoBehaviour, IDragHandler {
  public MapGenerator mapGenerator;
  public RawImage rawImage;
  public RectTransform rectTransform;
  public RectTransform playerRT;
  public RectTransform playerFrameRT;

  public PlayerStats _playerStat;

  private int mag = 4;
  public int selection = 0;

  public int[] scale;

  public Texture2D[] magnification;
  int mapSize;

  int maxDelta = 0;

  [SerializeField] private Canvas canvas;
  [SerializeField] private Vector2 originalPos;

  public void InitiateMap() {
    Debug.Log("Map Generated");
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
  }

  void OnEnable() {
    UpdateMap();

    Vector2 anchoredPos = _playerStat.position - new Vector2(mapGenerator.mapSize / 2, mapGenerator.mapSize / 2);

    //unscaled anchorpos
    anchoredPos *= 150f / (float)mapGenerator.mapSize;

    //scaled pos
    anchoredPos *= (float)scale[selection] * -1f;

    rectTransform.anchoredPosition = anchoredPos;
    Vector2 clampedPosition = new Vector2(Mathf.Clamp(rectTransform.anchoredPosition.x, -maxDelta, maxDelta), Mathf.Clamp(rectTransform.anchoredPosition.y, -maxDelta, maxDelta));
    rectTransform.anchoredPosition = clampedPosition;

    playerFrameRT.anchoredPosition = rectTransform.anchoredPosition;
    playerRT.anchoredPosition = anchoredPos * -1f;
  }

  [ContextMenu("Update Map")]
  public void UpdateMap() {
    rawImage.texture = magnification[selection];
    rawImage.SetNativeSize();

    rectTransform.sizeDelta = rectTransform.sizeDelta / 2;
    maxDelta = ((magnification[selection].width / 2) - 150) / 2;

    Vector2 clampedPosition = new Vector2(Mathf.Clamp(rectTransform.anchoredPosition.x, -maxDelta, maxDelta), Mathf.Clamp(rectTransform.anchoredPosition.y, -maxDelta, maxDelta));
    rectTransform.anchoredPosition = clampedPosition;
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

  //Drag handling
  public void OnDrag(PointerEventData eventData) {
    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    Vector2 clampedPosition = new Vector2(Mathf.Clamp(rectTransform.anchoredPosition.x, -maxDelta, maxDelta), Mathf.Clamp(rectTransform.anchoredPosition.y, -maxDelta, maxDelta));
    rectTransform.anchoredPosition = clampedPosition;
    playerFrameRT.anchoredPosition = rectTransform.anchoredPosition;
  }
}
