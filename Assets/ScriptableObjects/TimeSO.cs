using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class TimeSO : ScriptableObject
{
  public UnityEvent<int> OnDayChange;
  public UnityEvent<int> OnTick;

  public int tick, day;
  public float secondsPerDay = 10f;
  public float secondsPerTick = 2f;

  private int ticksPerDay;

  private void OnEnable()
  {
    tick = 0;
    day = 1;

    ticksPerDay = Mathf.FloorToInt(secondsPerDay / secondsPerTick);

    if (OnDayChange == null)
    {
      OnDayChange = new UnityEvent<int>();
    }

    if (OnTick == null)
    {
      OnTick = new UnityEvent<int>();
    }

  }

  public void Tick()
  {
    tick++;
    if (tick >= ticksPerDay) ChangeDay();
    OnTick.Invoke(tick);
  }

  public void ChangeDay()
  {
    day++;
    tick -= ticksPerDay;
    OnDayChange.Invoke(day);
  }
}
