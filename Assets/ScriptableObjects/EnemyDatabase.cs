using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy Database")]
public class EnemyDatabase : ScriptableObject {
  public List<GameObject> enemies;
}
