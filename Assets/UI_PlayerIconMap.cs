using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerIconMap : MonoBehaviour {
  public PlayerStats _playerStat;
  public MapUI mapUI;
  public RectTransform playerRT;
  public RectTransform frameRT;

  void Update() {
    // frameRT.anchoredPosition = mapUI.rectTransform.anchoredPosition;

    // Vector2 anchoredPos = _playerStat.position - new Vector2(mapUI.mapGenerator.mapSize / 2, mapUI.mapGenerator.mapSize / 2);

    // //unscaled anchorpos
    // anchoredPos *= 150f / (float)mapUI.mapGenerator.mapSize;

    // //scaled pos
    // anchoredPos *= (float)mapUI.scale[mapUI.selection];

    // playerRT.anchoredPosition = anchoredPos;
  }
}
