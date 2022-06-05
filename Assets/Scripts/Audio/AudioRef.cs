using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Reference")]
public class AudioRef : ScriptableObject {
  [SerializeField]
  public AudioClip clip;
}
