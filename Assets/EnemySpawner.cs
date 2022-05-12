using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
  private static EnemySpawner _instance;

  public static EnemySpawner Instance {
    get {
      if (_instance is null) {
      }
      return _instance;
    }
  }

  private void Awake() {
    _instance = this;
  }

  public EnemyDatabase enemyDB;

  public void SpawnEnemy(int enemyID, float x, float y) {
    GameObject temp = Instantiate(enemyDB.enemies[enemyID]);
    temp.transform.position = new Vector2(x, y);
  }

  public void LoadEnemy(List<WorldEnemyData> data) {
    foreach (WorldEnemyData e in data) {
      Debug.Log("spawning enemy with id " + e.enemyID + "at " + e.position[0] + ", " + e.position[1]);
      SpawnEnemy(e.enemyID, e.position[0], e.position[1]);
    }
  }

}
