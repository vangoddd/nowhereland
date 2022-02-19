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

  private void Awake() {
    _instance = this;
  }

  [ContextMenu("Save Game")]
  public bool SaveGame() {
    PlayerStatData playerStatData = new PlayerStatData(playerStat.health, playerStat.hunger, playerStat.thirst, playerStat.position);

    SaveGameData data = new SaveGameData(playerStatData);

    BinaryFormatter formatter = new BinaryFormatter();
    string path = Application.persistentDataPath + "/save.dat";
    FileStream stream = new FileStream(path, FileMode.Create);
    formatter.Serialize(stream, data);
    stream.Close();

    return true;
  }

  [ContextMenu("LoadGame")]
  public bool LoadGame() {
    string path = Application.persistentDataPath + "/save.dat";
    if (File.Exists(path)) {
      BinaryFormatter formatter = new BinaryFormatter();
      FileStream stream = new FileStream(path, FileMode.Open);
      SaveGameData data = formatter.Deserialize(stream) as SaveGameData;

      player.transform.position = new Vector3(data._playerStatData.position[0], data._playerStatData.position[1], 0f);
    } else {
      Debug.LogError("Savegame not found");
    }
    return true;
  }
}
