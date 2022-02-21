using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeUIController : MonoBehaviour {
  [SerializeField] TextMeshProUGUI day;
  [SerializeField] TimeSO _timeSO;

  void OnEnable() {
    _timeSO.OnDayChange.AddListener(changeDay);
    _timeSO.OnGameLoad.AddListener(LoadEvent);
    changeDay(_timeSO.day);
  }
  void OnDisable() {
    _timeSO.OnDayChange.RemoveListener(changeDay);
    _timeSO.OnGameLoad.RemoveListener(LoadEvent);
  }

  void changeDay(int dayData) {
    day.text = "Day " + dayData.ToString();
  }

  void LoadEvent(WorldData data) {
    day.text = "Day " + data.day.ToString();
  }
}
