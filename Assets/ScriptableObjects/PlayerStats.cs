using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "SO/Player Stats")]
public class PlayerStats : ScriptableObject {
  public float maxStat = 100;

  public float health, hunger, thirst;
  public Vector2 position;
  public Vector2 playerDirection;

  public float drain_hunger, drain_thirst, drain_health;

  public UnityEvent<PlayerStatData> OnStatChangeEvent;
  public UnityEvent OnPlayerDie;

  public GameEvent OnPlayerDieEvent;

  public HealthInteraction _healthInteraction;
  public Inventory _inventory;
  public AudioRef playerHurtSound;
  private bool alive = true;

  private void OnEnable() {
    ResetValues();

    if (OnStatChangeEvent == null) {
      OnStatChangeEvent = new UnityEvent<PlayerStatData>();
    }
    if (OnPlayerDie == null) {
      OnPlayerDie = new UnityEvent();
    }

    _healthInteraction.OnPlayerHurt.AddListener(playerHurt);
  }

  void OnDisable() {
    _healthInteraction.OnPlayerHurt.RemoveListener(playerHurt);
  }

  public void ResetValues() {
    maxStat = 100;
    health = maxStat;
    hunger = maxStat;
    thirst = maxStat;
    alive = true;
  }

  public void decreaseHealth(float amt) {
    health -= amt;
    OnStatChangeEvent.Invoke(new PlayerStatData(health, hunger, thirst, position));
  }

  public void playerHurt(float amt) {
    float reduction = 0f;
    if (_inventory.armorSlot != null) {
      Armor armor = _inventory.armorSlot.itemData as Armor;
      reduction = armor.def / 100f;
    }
    health -= amt * (1f - reduction);
    AudioManager.Instance.PlayOneShot(playerHurtSound);
    OnStatChangeEvent.Invoke(new PlayerStatData(health, hunger, thirst, position));
  }

  public void addStat(PlayerStatData data) {
    health += data.health;
    hunger += data.hunger;
    thirst += data.thirst;

    if (health < 0) health = 0;
    if (hunger < 0) hunger = 0;
    if (thirst < 0) thirst = 0;

    if (health > maxStat) health = maxStat;
    if (hunger > maxStat) hunger = maxStat;
    if (thirst > maxStat) thirst = maxStat;

    OnStatChangeEvent.Invoke(new PlayerStatData(health, hunger, thirst, position));
    if (health <= 0) {
      Die();
    }
  }

  public void addStat(float _health, float _hunger, float _thirst) {
    health += _health;
    hunger += _hunger;
    thirst += _thirst;

    if (health < 0) health = 0;
    if (hunger < 0) hunger = 0;
    if (thirst < 0) thirst = 0;

    if (health > maxStat) health = maxStat;
    if (hunger > maxStat) hunger = maxStat;
    if (thirst > maxStat) thirst = maxStat;

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

    if (health > maxStat) health = maxStat;
    if (hunger > maxStat) hunger = maxStat;
    if (thirst > maxStat) thirst = maxStat;

    OnStatChangeEvent.Invoke(new PlayerStatData(health, hunger, thirst, position));
    if (health <= 0) {
      Die();
    }
  }

  public void Die() {
    if (!alive) return;
    alive = false;
    OnPlayerDieEvent.Raise();
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

  public PlayerStatData(float health, float hunger, float thirst) {
    this.health = health;
    this.hunger = hunger;
    this.thirst = thirst;

    position[0] = 0;
    position[1] = 0;
  }
}
