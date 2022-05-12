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
  public EnemyHandler enemyHandler;

  public ItemDatabase itemDB;
  public Inventory inventory;

  public MapSO map;

  public ChestHandler chestHandler;
  public Camera cam;

  private void Awake() {
    _instance = this;
  }

  [ContextMenu("Save Game")]
  public bool SaveGame() {
    SaveGameData data = new SaveGameData(GeneratePlayerStatData(), GenerateMapSaveData(), GenerateWorldData(), GenerateInventoryData());

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
      cam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, cam.transform.position.z);
      playerStat.setStat(data._playerStatData);

      map.tileMap = Get2DTilemapListFromData(data._mapSaveData);
      map.biomes = Get2DBiomeMapListFromData(data._mapSaveData);
      map.worldObjectDatas = data._mapSaveData.worldObjectDatas;

      chestHandler.ApplyLoadedData(data._mapSaveData);

      timeSO.ApplyLoadedData(data._worldData);

      inventory.ApplyLoadedData(data._inventoryData);
      ItemSpawner.Instance.SpawnItemFromLoadedData(data._mapSaveData.worldItemDatas);
      EnemySpawner.Instance.LoadEnemy(data._mapSaveData.worldEnemyDatas);
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
    List<WorldObjectData> currentWorldData = new List<WorldObjectData>();
    List<WorldItemData> currentWorldItemData = new List<WorldItemData>();
    List<WorldEnemyData> currentEnemyData = new List<WorldEnemyData>();

    List<int> currentChestIds = new List<int>();
    List<InventoryData> currentChestContents = new List<InventoryData>();

    //Handling adding tilemap to List
    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        currentMapData.Add(map.tileMap[x][y]);
        currentBiomeData.Add(map.biomes[x][y]);
      }
    }

    //adding map data from worldobject list
    for (int i = 0; i < map.worldObjects.Count; i++) {
      WorldObject obj = map.worldObjects[i].GetComponent<WorldObject>();
      int objId = obj.objectID;
      Vector2 pos = new Vector2(map.worldObjects[i].transform.position.x, map.worldObjects[i].transform.position.y);

      int status = obj.status;
      if (obj is Destroyable && !(obj is Chest)) status = ((Destroyable)obj).health;
      currentWorldData.Add(new WorldObjectData(objId, pos, status));
    }

    //handle world item
    for (int i = 0; i < map.worldItemData.Count; i++) {
      WorldItemData temp = new WorldItemData();
      ItemScript itemScript = map.worldItemData[i].GetComponent<ItemScript>();
      temp.itemId = itemDB.itemLookup[itemScript.itemData];
      temp.itemAmount = itemScript.itemAmount;
      temp.itemDurability = itemScript.durability;
      temp.position[0] = map.worldItemData[i].transform.position.x;
      temp.position[1] = map.worldItemData[i].transform.position.y;

      currentWorldItemData.Add(temp);
    }

    //handle chest data
    foreach (var chest in chestHandler.chestList) {
      currentChestIds.Add(chest.Key);
      currentChestContents.Add(chestHandler.GenerateChestData(chest.Value));
    }

    //handle enemy
    foreach (Enemy e in enemyHandler.enemyList) {
      WorldEnemyData temp = new WorldEnemyData();
      temp.enemyID = e.enemyID;
      temp.position[0] = e.gameObject.transform.position.x;
      temp.position[1] = e.gameObject.transform.position.y;
      currentEnemyData.Add(temp);
      Debug.Log("saving enemy with id " + temp.enemyID + "at " + temp.position[0] + ", " + temp.position[1]);
    }

    data.tileMap = currentMapData;
    data.biomeMap = currentBiomeData;
    data.worldObjectDatas = currentWorldData;
    data.worldItemDatas = currentWorldItemData;
    data.worldEnemyDatas = currentEnemyData;

    data.chestIds = currentChestIds;
    data.chestContents = currentChestContents;

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

  public InventoryData GenerateInventoryData() {
    InventoryData data = new InventoryData();
    int[] currentItemId = new int[20];
    int[] currentItemDurability = new int[20];
    int[] currentItemAmount = new int[20];

    for (int i = 0; i < 20; i++) {
      currentItemId[i] = -1;
      currentItemDurability[i] = -1;
      currentItemAmount[i] = -1;

      if (i < 18) {
        if (inventory.itemList[i] != null) {
          currentItemId[i] = itemDB.itemLookup[inventory.itemList[i].itemData];
          currentItemDurability[i] = inventory.itemList[i].durability;
          currentItemAmount[i] = inventory.itemList[i].amount;
        }
      } else {
        if (i == 18) {
          if (inventory.handSlot != null) {
            currentItemId[18] = itemDB.itemLookup[inventory.handSlot.itemData];
            currentItemDurability[18] = inventory.handSlot.durability;
            currentItemAmount[18] = 1;
          }
        } else if (i == 19) {
          if (inventory.armorSlot != null) {
            currentItemId[19] = itemDB.itemLookup[inventory.armorSlot.itemData];
            currentItemDurability[19] = inventory.armorSlot.durability;
            currentItemAmount[19] = 1;
          }
        }
      }
    }

    data.itemId = currentItemId;
    data.itemDurability = currentItemDurability;
    data.itemAmount = currentItemAmount;

    return data;
  }
}

