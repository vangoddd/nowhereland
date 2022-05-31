using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Loot Table")]
public class LootTable : ScriptableObject {
  public FixedLootInfo[] fixedLoot;
  public ChanceLootInfo[] chanceLoot;
}

[System.Serializable]
public class FixedLootInfo {
  public ItemData item;
  public int amount;
}

[System.Serializable]
public class ChanceLootInfo {
  public ItemData item;
  public int amount;
  public float chance;
}
