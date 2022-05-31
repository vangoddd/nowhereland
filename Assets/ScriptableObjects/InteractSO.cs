using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "SO/Interact SO")]
public class InteractSO : ScriptableObject {
  public UnityEvent OnInteractMove;

  private void OnEnable() {
    if (OnInteractMove == null) {
      OnInteractMove = new UnityEvent();
    }
  }

  public void DoInteractMove() {
    OnInteractMove.Invoke();
  }
}
