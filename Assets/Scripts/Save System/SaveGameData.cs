using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGameData {
  public PlayerStatData _playerStatData;
  public MapSaveData _mapSaveData;

  public SaveGameData(PlayerStatData playerStatData, MapSaveData mapSaveData) {
    _playerStatData = playerStatData;
    _mapSaveData = mapSaveData;
  }
  //   public WorldData _worldData;
}

[System.Serializable]
public class MapSaveData {
  public int seed;
  public int mapSize;
  public List<int> tileData;
  public List<WorldObjectData> worldObjectDatas;
}

[System.Serializable]
public class WorldData {
  public int day;
  public int tick;
}
