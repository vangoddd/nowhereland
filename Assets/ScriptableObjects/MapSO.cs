using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSO : ScriptableObject {
  public int seed;
  public int mapSize;
  public int chunkSize;

  public List<List<GameObject>> chunks;
  public List<List<int>> tileMap;
  public List<List<int>> biomes;

  public List<GameObject> worldObject;
  public List<List<float>> rawNoiseData;

  void OnEnable() {
    chunks = null;
    tileMap = null;
    biomes = null;
    worldObject = null;
    rawNoiseData = null;

    seed = 0;

    for (int i = 0; i < (mapSize * mapSize / (chunkSize * chunkSize)); i++) {
      chunks.Add(new List<GameObject>());
    }
  }
}
