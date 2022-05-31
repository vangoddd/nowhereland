using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanup : MonoBehaviour {
  public TimeSO timeSO;
  public PlayerStats playerStats;
  public MapSO mapSO;
  public Inventory inventory;
  public EnemyHandler enemyHandler;
  public ChestHandler chestHandler;

  public GameEvent InventoryUpdate;
  public GameEvent UpdateUIOnSceneLoad;

  public MapUI mapUI;

  void Start() {
    timeSO.ResetValues();
    playerStats.ResetValues();
    mapSO.ResetValues();
    inventory.ResetValues();
    enemyHandler.ResetValues();
    chestHandler.ResetValues();

    mapUI.AttachListener();

    InventoryUpdate.Raise();
    UpdateUIOnSceneLoad.Raise();
    //timeSO.OnDayChange.Invoke(1);
  }
}
