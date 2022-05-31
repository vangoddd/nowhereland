using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/MapIcon")]
public class MapIconSO : ScriptableObject {
  public MapIconRelation[] iconList;

  public Dictionary<int, Sprite> getSprite;

  void OnEnable() {
    getSprite = new Dictionary<int, Sprite>();

    foreach (MapIconRelation item in iconList) {
      getSprite.Add(item.worldObject.GetComponent<WorldObject>().objectID, item.iconSprite);
    }
  }
}

[System.Serializable]
public class MapIconRelation {
  public GameObject worldObject;
  public Sprite iconSprite;
}
