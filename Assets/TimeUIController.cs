using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeUIController : MonoBehaviour {
  [SerializeField] TextMeshProUGUI day;
  [SerializeField] TimeSO _timeSO;

  void OnEnable() {
    _timeSO.OnDayChange.AddListener(changeDay);
    changeDay(_timeSO.day);
  }
  void OnDisable() {
    _timeSO.OnDayChange.RemoveListener(changeDay);
  }

  void changeDay(int dayData) {
    day.text = "Day " + dayData.ToString();
  }
}
