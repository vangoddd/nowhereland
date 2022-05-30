using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : Destroyable {
  int growthTimer = 3;
  public TimeSO _timeSO;

  public GameObject GrowthObject;

  void Start() {
    InitializeObject();
    if (status == -1) status = growthTimer;
    _timeSO.OnDayChange.AddListener(DayListener);
  }

  void OnDestroy() {
    _timeSO.OnDayChange.RemoveListener(DayListener);
  }

  public override void Interact(GameObject player) {
    base.Interact(player);
    status = growthTimer;
  }

  private void DayListener(int day) {
    growthTimer--;
    status = growthTimer;
    if (growthTimer <= 0) {
      GameObject spawnedObject = Instantiate(GrowthObject);
      spawnedObject.transform.position = transform.position;
      status = growthTimer;
      DestroyWorldObject();
    }
  }

  public override void OnDataLoad() {
    growthTimer = status;
  }
}
