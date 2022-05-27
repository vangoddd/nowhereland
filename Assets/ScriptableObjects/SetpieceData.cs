using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Setpiece_biome", menuName = "SO/Setpiece Data")]
public class SetpieceData : ScriptableObject {
  public SetpieceDataHelper[] objects;
}

[System.Serializable]
public class SetpieceDataHelper {
  public GameObject prefab;
  public int amount;
}
