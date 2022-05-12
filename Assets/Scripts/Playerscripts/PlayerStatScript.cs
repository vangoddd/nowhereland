using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatScript : MonoBehaviour {
  public PlayerStats _playerStat;
  public float temp;

  public bool godmode = false;

  void OnEnable() {
    _playerStat.OnPlayerDie.AddListener(Die);
  }
  void OnDisable() {
    _playerStat.OnPlayerDie.RemoveListener(Die);
  }

  void Update() {
    if (godmode) return;

    // float reductionAmt = -2 * Time.deltaTime;
    _playerStat.addStat(new PlayerStatData(0f, -_playerStat.drain_hunger * Time.deltaTime, -_playerStat.drain_thirst * Time.deltaTime, (Vector2)transform.position));

    if (_playerStat.hunger <= 0f) {
      _playerStat.addStat(new PlayerStatData(-_playerStat.drain_health * Time.deltaTime, 0f, 0f, (Vector2)transform.position));
    }

    if (_playerStat.thirst <= 0f) {
      _playerStat.addStat(new PlayerStatData(-_playerStat.drain_health * Time.deltaTime, 0f, 0f, (Vector2)transform.position));
    }
  }

  void Die() {
    _playerStat.OnPlayerDie.RemoveListener(Die);
    Debug.Log("Player died.");
  }

  public void ToggleGodMode() {
    godmode = !godmode;
    Debug.Log("Godmode : " + godmode);
  }
}
