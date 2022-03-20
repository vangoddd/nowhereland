using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGameData {
  public PlayerStatData _playerStatData;
  public MapSaveData _mapSaveData;
  public WorldData _worldData;
  public InventoryData _inventoryData;

  public SaveGameData(PlayerStatData playerStatData, MapSaveData mapSaveData, WorldData worldData, InventoryData invData) {
    _playerStatData = playerStatData;
    _mapSaveData = mapSaveData;
    _worldData = worldData;
    _inventoryData = invData;
  }
  //   public WorldData _worldData;
}

[System.Serializable]
public class MapSaveData {
  public int seed;
  public int mapSize;
  public List<int> tileMap;
  public List<int> biomeMap;
  public List<WorldObjectData> worldObjectDatas;
  public List<WorldItemData> worldItemDatas;
}

[System.Serializable]
public class WorldData {
  public int day;
  public int tick;
  public bool isDay;
}

[System.Serializable]
public class InventoryData {
  public int[] itemId;
  public int[] itemDurability;
  public int[] itemAmount;
}
