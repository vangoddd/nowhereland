using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Useable {

  public override void UseObject() {
    Debug.Log("Using object " + gameObject.name);
  }

  public override void DestroyWorldObject() {
    base.DestroyWorldObject();

    Debug.Log("Dropping chest content : ");
  }

}
