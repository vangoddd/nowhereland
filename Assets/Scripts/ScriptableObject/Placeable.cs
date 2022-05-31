using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Placeable item")]
public class Placeable : ItemData {
  public GameObject worldObjectPrefab;
  public ItemInteraction itemInteraction;

  public override string getItemType() {
    return "Structure";
  }

}
