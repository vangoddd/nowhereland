using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyHandler : ScriptableObject {
  public PlayerStats _playerStat;

  public List<Enemy> enemyList;
  public List<Enemy> aggroList;

  void OnEnable() {
    ResetValues();
  }

  public void ResetValues() {
    enemyList = new List<Enemy>();
    aggroList = new List<Enemy>();
  }

  public void ChangeAggro(Enemy e, bool aggro) {
    if (aggro) {
      aggroList.Add(e);
    } else {
      aggroList.Remove(e);
    }

    if (aggroList.Count == 0) {
      Debug.Log("Lost all aggro");
    } else {
      Debug.Log("Have an aggro");
    }
  }
}
