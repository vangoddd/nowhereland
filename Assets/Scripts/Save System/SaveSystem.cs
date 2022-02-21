using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour {
  private static SaveSystem _instance;

  public static SaveSystem Instance {
    get {
      if (_instance is null) {

      }
      return _instance;
    }
  }

  public GameObject player;
  public PlayerStats playerStat;

  public MapSO map;

  private void Awake() {
    _instance = this;
  }

  [ContextMenu("Save Game")]
  public bool SaveGame() {
    SaveGameData data = new SaveGameData(GeneratePlayerStatData(), GenerateMapSaveData());

    BinaryFormatter formatter = new BinaryFormatter();
    string path = Application.persistentDataPath + "/save.dat";
    FileStream stream = new FileStream(path, FileMode.Create);
    formatter.Serialize(stream, data);
    Debug.Log("Data saved at : " + path);
    stream.Close();

    return true;
  }

  [ContextMenu("Load Game")]
  public bool LoadGame() {
    string path = Application.persistentDataPath + "/save.dat";
    if (File.Exists(path)) {
      BinaryFormatter formatter = new BinaryFormatter();
      FileStream stream = new FileStream(path, FileMode.Open);
      SaveGameData data = formatter.Deserialize(stream) as SaveGameData;

      player.transform.position = new Vector3(data._playerStatData.position[0], data._playerStatData.position[1], 0f);
      playerStat.setStat(data._playerStatData);

      map.tileMap = Get2DTilemapListFromData(data._mapSaveData);
      map.worldObjectDatas = data._mapSaveData.worldObjectDatas;

    } else {
      Debug.LogError("Savegame not found");
    }
    return true;
  }

  public PlayerStatData GeneratePlayerStatData() {
    return new PlayerStatData(playerStat.health, playerStat.hunger, playerStat.thirst, playerStat.position);
  }

  public MapSaveData GenerateMapSaveData() {
    MapSaveData data = new MapSaveData();
    data.seed = map.seed;
    data.mapSize = map.mapSize;

    List<int> currentTileData = new List<int>();

    int tileId;
    //Handling adding tilemap to List
    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        if (map.tileMap[x][y] == 1) {
          tileId = map.tileMap[x][y] + map.biomes[x][y];
        } else {
          tileId = map.tileMap[x][y];
        }
        currentTileData.Add(tileId);
      }
    }

    data.tileData = currentTileData;
    data.worldObjectDatas = map.worldObjectDatas;

    return data;
  }

  public List<List<int>> Get2DTilemapListFromData(MapSaveData data) {
    List<List<int>> tileMap = new List<List<int>>();
    int counter = 0;
    for (int x = 0; x < data.mapSize; x++) {
      tileMap.Add(new List<int>());
      for (int y = 0; y < data.mapSize; y++) {
        tileMap[x].Add(data.tileData[counter]);
        counter++;
      }
    }
    return tileMap;
  }
}
