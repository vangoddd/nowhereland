using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoiseRenderer))]
public class NoiseRendererEditor : Editor {
  public override void OnInspectorGUI() {
    base.OnInspectorGUI();

    NoiseRenderer script = (NoiseRenderer)target;
    script.RegenerateTexture();
  }
}