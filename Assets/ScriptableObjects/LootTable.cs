using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
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
