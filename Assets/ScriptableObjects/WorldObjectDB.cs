using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DB/WorldObject DB")]
public class WorldObjectDB : ScriptableObject {
  public List<GameObject> worldObjects;
  public float[] spawnWeight;
  public float[] spawnWeightThreshold;

  public Dictionary<GameObject, int> objectLookup;

  void OnEnable() {
    //InitiateDict();
  }

  public void InitiateDict() {
    if (worldObjects == null) worldObjects = new List<GameObject>();
    objectLookup = new Dictionary<GameObject, int>();

    int index = 0;
    foreach (var item in worldObjects) {
      objectLookup.Add(item, index);
      index++;
    }

    if (spawnWeight == null || spawnWeight.Length == 0) return;

    if (spawnWeight.Length > 0) spawnWeightThreshold = CalculateWeightThreshold();
  }

  float[] CalculateWeightThreshold() {
    // Calculate biome threshold
    float[] objectThreshold = new float[worldObjects.Count];
    float total = 0;
    for (int i = 0; i < worldObjects.Count; i++) {
      total += spawnWeight[i];
    }
    for (int i = 0; i < worldObjects.Count; i++) {
      if (i == 0) objectThreshold[i] = spawnWeight[i] / total;
      else objectThreshold[i] = objectThreshold[i - 1] + (spawnWeight[i] / total);
    }
    return objectThreshold;
  }
}
