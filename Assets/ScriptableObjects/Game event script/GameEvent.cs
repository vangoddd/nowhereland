using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject {
  private List<GameEventListener> listeners = new List<GameEventListener>();

  public void Raise() {
    for (int i = listeners.Count - 1; i >= 0; i--) {
      listeners[i].OnEventRaised();
    }
  }

  public void RegisterListener(GameEventListener l) {
    listeners.Add(l);
  }
  public void UnregisterListener(GameEventListener l) {
    listeners.Remove(l);
  }
}
