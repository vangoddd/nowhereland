using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WorldObjectDB : ScriptableObject {
  public List<GameObject> worldObjects;

  public Dictionary<GameObject, int> objectLookup;

  void OnEnable() {
    if (worldObjects == null) worldObjects = new List<GameObject>();
    objectLookup = new Dictionary<GameObject, int>();

    int index = 0;
    foreach (var item in worldObjects) {
      objectLookup.Add(item, index);
      index++;
    }
  }
}
