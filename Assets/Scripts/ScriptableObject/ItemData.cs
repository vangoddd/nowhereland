using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Base item")]
public class ItemData : ScriptableObject {
  public int stackCount;
  public string description;
  public new string name;

  public Sprite sprite;

  [ContextMenu("Use Item")]
  public virtual void UseItem() {
    Debug.Log("Using " + name);
  }

  public bool isStackable() {
    return stackCount != 1;
  }
}
