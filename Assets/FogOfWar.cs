using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour {
  public MapSO _map;
  public PlayerStats _playerStat;
  public TimeSO _time;

  public int visionRadius;

  private CircleAlgorithm circleGenerator;

  public Texture2D fogTexture;

  void OnEnable() {
    circleGenerator = new CircleAlgorithm();

    _time.OnTick.AddListener(TickListener);
  }

  void OnDisable() {
    _time.OnTick.RemoveListener(TickListener);
  }

  void TickListener(int tick) {
    //UpdateFog();
  }

  void Update() {
    UpdateFog();
  }

  public void UpdateFog() {
    List<Vector2Int> visionArea = circleGenerator.GetCircleList(visionRadius, (int)_playerStat.position.x, (int)_playerStat.position.y);
    foreach (Vector2Int point in visionArea) {
      _map.fogOfWarData[point.x, point.y] = true;
    }
  }
}
