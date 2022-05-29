using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class TimeSO : ScriptableObject {
  public UnityEvent<int> OnDayChange;
  public UnityEvent<int> OnTick;
  public UnityEvent OnNightChange;
  public UnityEvent<WorldData> OnGameLoad;
  public UnityEvent<string> OnIngameHourTick;

  public int tick, day;
  public float secondsPerDay;
  public float secondsPerTick;
  public float nightRatio;

  private int ticksPerDay;
  private int tickToNight;
  private int inGameMinutePerTick;
  private int inGameMinuteCounter;

  public float transitionTime = 5f;

  public bool isDay = true;

  public GameEvent OnDayChangeEvent;

  private void OnEnable() {
    ResetValues();
  }

  public void ResetValues() {
    tick = 0;
    day = 1;
    isDay = true;
    inGameMinuteCounter = 0;

    ticksPerDay = Mathf.FloorToInt(secondsPerDay / secondsPerTick);
    tickToNight = Mathf.FloorToInt(ticksPerDay * (1f - nightRatio));
    inGameMinutePerTick = Mathf.FloorToInt(1440 / ticksPerDay);

    if (OnDayChange == null) {
      OnDayChange = new UnityEvent<int>();
    }

    if (OnTick == null) {
      OnTick = new UnityEvent<int>();
    }

    if (OnNightChange == null) {
      OnNightChange = new UnityEvent();
    }

    if (OnGameLoad == null) {
      OnGameLoad = new UnityEvent<WorldData>();
    }

    if (OnIngameHourTick == null) {
      OnIngameHourTick = new UnityEvent<string>();
    }
  }

  public void Tick() {
    tick++;
    inGameMinuteCounter += inGameMinutePerTick;
    if (tick >= tickToNight && isDay) ChangeToNight();
    if (tick >= ticksPerDay) ChangeDay();
    OnTick.Invoke(tick);
    OnIngameHourTick.Invoke(CalculateInGameHour(inGameMinuteCounter));
  }

  [ContextMenu("Invoke Change Day Event")]
  public void ChangeDay() {
    isDay = true;
    tick = 0;
    inGameMinuteCounter = 0;
    day++;
    OnDayChange.Invoke(day);
    OnDayChangeEvent.Raise();
  }

  public void ChangeToNight() {
    isDay = false;
    OnNightChange.Invoke();
  }

  public void ApplyLoadedData(WorldData data) {
    tick = data.tick;
    day = data.day;
    isDay = data.isDay;

    OnGameLoad.Invoke(data);
  }

  public string CalculateInGameHour(int inGameMinute) {
    int hour = (inGameMinute / 60 + 6) % 24;
    int minute = (inGameMinute % 60) / 10 * 10;
    string ingameHour = hour + " " + minute;
    return ingameHour;
  }
}
