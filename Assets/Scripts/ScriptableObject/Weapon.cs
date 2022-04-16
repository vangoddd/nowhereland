using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Weapon item")]
public class Weapon : ItemData {
  public int durability;
  public float damage;
}
