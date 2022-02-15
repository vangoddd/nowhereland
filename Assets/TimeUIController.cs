using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeUIController : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI day, tick;
  [SerializeField] TimeSO _timeSO;

  void OnEnable()
  {
    _timeSO.OnDayChange.AddListener(changeDay);
    _timeSO.OnTick.AddListener(changeTick);

    changeDay(_timeSO.day);
  }
  void OnDisable()
  {
    _timeSO.OnDayChange.RemoveListener(changeDay);
    _timeSO.OnTick.RemoveListener(changeTick);
  }

  void changeDay(int dayData)
  {
    day.text = "Day : " + dayData.ToString();
  }

  void changeTick(int tickData)
  {
    tick.text = tickData.ToString();
  }
}
