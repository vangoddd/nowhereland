using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/MapIcon")]
public class MapIconSO : ScriptableObject {
  public MapIconRelation[] iconList;
}

[System.Serializable]
public class MapIconRelation {
  public GameObject worldObject;
  public Sprite iconSprite;
}
