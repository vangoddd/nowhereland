using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : Destroyable {
  int growthTimer = 3;
  public TimeSO _timeSO;

  public GameObject GrowthObject;

  void Start() {
    base.InitializeObject();
    _timeSO.OnDayChange.AddListener(DayListener);
  }

  void OnDestroy() {
    _timeSO.OnDayChange.RemoveListener(DayListener);
  }

  private void DayListener(int day) {
    growthTimer--;
    if (growthTimer <= 0) {
      GameObject spawnedObject = Instantiate(GrowthObject);
      spawnedObject.transform.position = transform.position;
      DestroyWorldObject();
    }
  }
}
