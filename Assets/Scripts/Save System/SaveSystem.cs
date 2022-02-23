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
  public TimeSO timeSO;

  public MapSO map;

  private void Awake() {
    _instance = this;
  }

  [ContextMenu("Save Game")]
  public bool SaveGame() {
    SaveGameData data = new SaveGameData(GeneratePlayerStatData(), GenerateMapSaveData(), GenerateWorldData());

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
      map.biomes = Get2DBiomeMapListFromData(data._mapSaveData);
      map.worldObjectDatas = data._mapSaveData.worldObjectDatas;

      timeSO.ApplyLoadedData(data._worldData);
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

    List<int> currentMapData = new List<int>();
    List<int> currentBiomeData = new List<int>();

    //Handling adding tilemap to List
    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        currentMapData.Add(map.tileMap[x][y]);
        currentBiomeData.Add(map.biomes[x][y]);
      }
    }

    data.tileMap = currentMapData;
    data.biomeMap = currentBiomeData;
    data.worldObjectDatas = map.worldObjectDatas;

    return data;
  }

  public List<List<int>> Get2DTilemapListFromData(MapSaveData data) {
    List<List<int>> tileMap = new List<List<int>>();
    int counter = 0;
    for (int x = 0; x < data.mapSize; x++) {
      tileMap.Add(new List<int>());
      for (int y = 0; y < data.mapSize; y++) {
        tileMap[x].Add(data.tileMap[counter]);
        counter++;
      }
    }
    return tileMap;
  }

  public List<List<int>> Get2DBiomeMapListFromData(MapSaveData data) {
    List<List<int>> biomeMap = new List<List<int>>();
    int counter = 0;
    for (int x = 0; x < data.mapSize; x++) {
      biomeMap.Add(new List<int>());
      for (int y = 0; y < data.mapSize; y++) {
        biomeMap[x].Add(data.biomeMap[counter]);
        counter++;
      }
    }
    return biomeMap;
  }

  public WorldData GenerateWorldData() {
    WorldData data = new WorldData();
    data.day = timeSO.day;
    data.tick = timeSO.tick;
    data.isDay = timeSO.isDay;
    return data;
  }
}
