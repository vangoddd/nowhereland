using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGameData {
  public PlayerStatData _playerStatData;
  public MapSaveData _mapSaveData;
  public WorldData _worldData;

  public SaveGameData(PlayerStatData playerStatData, MapSaveData mapSaveData, WorldData worldData) {
    _playerStatData = playerStatData;
    _mapSaveData = mapSaveData;
    _worldData = worldData;
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
}

[System.Serializable]
public class WorldData {
  public int day;
  public int tick;
  public bool isDay;
}
