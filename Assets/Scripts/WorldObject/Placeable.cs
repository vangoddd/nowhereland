using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : Destroyable {
  public virtual void PlaceObject() {
    Debug.Log("Placing " + gameObject.name);
  }
}
