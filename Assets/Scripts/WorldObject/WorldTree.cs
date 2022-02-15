using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTree : WorldObject
{
  public override void Interact(GameObject player)
  {
    Debug.Log("Player interacted with Tree " + gameObject.name + player.transform.position.x.ToString());
  }
}
