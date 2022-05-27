using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapSO : ScriptableObject {
  public int seed;
  public int mapSize;
  public int chunkSize;

  public List<List<GameObject>> chunks;
  public List<List<int>> tileMap;
  public List<List<int>> biomes;

  public List<GameObject> worldItemData;
  public List<GameObject> worldObjects;
  public List<List<float>> rawNoiseData;
  public List<WorldObjectData> worldObjectDatas;

  public void ResetValues() {
    chunks = new List<List<GameObject>>();
    tileMap = new List<List<int>>();
    biomes = new List<List<int>>();
    worldObjects = new List<GameObject>();
    worldItemData = new List<GameObject>();
    rawNoiseData = new List<List<float>>();
    worldObjectDatas = new List<WorldObjectData>();

    int pseudoMapsize = (Mathf.CeilToInt((float)mapSize / (float)chunkSize)) * chunkSize;

    // for (int i = 0; i <= (mapSize * mapSize / (chunkSize * chunkSize)); i++) {
    for (int i = 0; i < (pseudoMapsize * pseudoMapsize / (chunkSize * chunkSize)); i++) {
      //Debug.Log("adding chunks number " + i);
      chunks.Add(new List<GameObject>());
    }
  }
}
