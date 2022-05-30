using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDatabase : ScriptableObject {
  [SerializeField]
  public List<ItemData> itemList;

  public Dictionary<ItemData, int> itemLookup;

  void OnEnable() {
    //InitiateDict();
  }

  public void InitiateDict() {
    if (itemList == null) itemList = new List<ItemData>();
    itemLookup = new Dictionary<ItemData, int>();

    int index = 0;
    foreach (var item in itemList) {
      itemLookup.Add(item, index);
      index++;
    }
  }
}
