using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject {
  public float maxStat = 100;

  public float health, hunger, thirst;
  public Vector2 position;

  public UnityEvent<PlayerStatData> OnStatChangeEvent;
  public UnityEvent OnPlayerDie;

  private void OnEnable() {
    maxStat = 100;
    health = maxStat;
    hunger = maxStat;
    thirst = maxStat;

    if (OnStatChangeEvent == null) {
      OnStatChangeEvent = new UnityEvent<PlayerStatData>();
    }
    if (OnPlayerDie == null) {
      OnPlayerDie = new UnityEvent();
    }
  }

  public void decreaseHealth(float amt) {
    health -= amt;
    OnStatChangeEvent.Invoke(new PlayerStatData(health, hunger, thirst, position));
  }

  public void addStat(PlayerStatData data) {
    health += data.health;
    hunger += data.hunger;
    thirst += data.thirst;

    if (health < 0) health = 0;
    if (hunger < 0) hunger = 0;
    if (thirst < 0) thirst = 0;

    OnStatChangeEvent.Invoke(new PlayerStatData(health, hunger, thirst, position));
    if (health <= 0) {
      Die();
    }
  }

  public void setStat(PlayerStatData data) {
    health = data.health;
    hunger = data.hunger;
    thirst = data.thirst;

    if (health < 0) health = 0;
    if (hunger < 0) hunger = 0;
    if (thirst < 0) thirst = 0;

    OnStatChangeEvent.Invoke(new PlayerStatData(health, hunger, thirst, position));
    if (health <= 0) {
      Die();
    }
  }

  public void Die() {
    OnPlayerDie.Invoke();
  }

  public void PlayerMove(Vector2 pos) {
    position = pos;
  }
}

//Class for updateing SO value, and for saving
[System.Serializable]
public class PlayerStatData {
  public float health, hunger, thirst;
  public float[] position = new float[2];

  public PlayerStatData(float health, float hunger, float thirst, Vector2 pos) {
    this.health = health;
    this.hunger = hunger;
    this.thirst = thirst;

    position[0] = pos.x;
    position[1] = pos.y;
  }
}
