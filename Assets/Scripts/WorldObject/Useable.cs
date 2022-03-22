using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Useable : Destroyable {
  public GameEvent OnWorldItemUse;

  public virtual void UseObject() {
    OnWorldItemUse.Raise();
  }
}
