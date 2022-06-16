using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerObject : Destroyable {
  int enemyCount = 3;
  public int maxEnemyCount = 6;
  public int spawnCooldownTick = 5;
  public float spawnRange = 5f;
  public int refillEnemyTick = 30;
  int tickCounter = 30;
  int cooldownCounter = 0;

  public PlayerStats _playerStat;
  public GameObject EnemyPrefab;
  public TimeSO _timeSO;

  public AudioRef spawnAudio;

  void OnEnable() {
    _timeSO.OnTick.AddListener(tickListener);
    tickCounter = refillEnemyTick;
  }

  void OnDisable() {
    _timeSO.OnTick.RemoveListener(tickListener);
  }

  void tickListener(int tickCount) {
    if (enemyCount < maxEnemyCount) {
      tickCounter--;
    }
    if (tickCounter <= 0) {
      AddEnemy();
      tickCounter = refillEnemyTick;
    }
    if (cooldownCounter > 0) {
      cooldownCounter--;
    }

    //listen for player
    if ((_playerStat.position - (Vector2)transform.position).sqrMagnitude <= spawnRange * spawnRange) {
      if (cooldownCounter <= 0) {
        cooldownCounter = spawnCooldownTick;
        SpawnEnemy();
      }
    }
  }

  void AddEnemy() {
    if (enemyCount > maxEnemyCount) return;
    enemyCount++;
  }

  void SpawnEnemy() {
    if (enemyCount > 0) {
      AudioManager.Instance.PlayOneShot(spawnAudio);
      enemyCount--;
      Instantiate(EnemyPrefab, transform.position, transform.rotation);
    }
  }

  void OnDrawGizmosSelected() {
    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere(transform.position + offset, interactDistance);
    Gizmos.DrawWireSphere(transform.position, spawnRange);
  }
}
