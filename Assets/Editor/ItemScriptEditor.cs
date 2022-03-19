using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemScript))]
public class ItemScriptEditor : Editor {
  public override void OnInspectorGUI() {
    base.OnInspectorGUI();

    ItemScript item = (ItemScript)target;
    item.ApplyData();
  }
}