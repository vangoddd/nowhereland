using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/MapIcon")]
public class MapIconSO : ScriptableObject {
  public MapIconRelation[] iconList;

  [SerializeField]
  public Dictionary<int, Sprite> getSprite;

  public void InitiateIcons() {
    getSprite = new Dictionary<int, Sprite>();

    foreach (MapIconRelation item in iconList) {
      getSprite.Add(item.worldObject.GetComponent<WorldObject>().objectID, item.iconSprite);
    }
  }

  [ContextMenu("Print List")]
  public void PrintList() {
    foreach (KeyValuePair<int, Sprite> icon in getSprite) {
      Debug.Log("Key : " + icon.Key + ", Value : " + icon.Value.name);
    }
  }

}




[System.Serializable]
public class MapIconRelation {
  public GameObject worldObject;
  public Sprite iconSprite;
}
