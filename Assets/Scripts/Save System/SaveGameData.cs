using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGameData {
  public PlayerStatData _playerStatData;
  public SaveGameData(PlayerStatData playerStatData) {
    _playerStatData = playerStatData;
  }
  //   public MapSaveData _mapSaveData;
  //   public WorldData _worldData;
}

[System.Serializable]
public class MapSaveData {
  public int seed;
}

[System.Serializable]
public class WorldData {
  public int day;
  public int tick;
}
