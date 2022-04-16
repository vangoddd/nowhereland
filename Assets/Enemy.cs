using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
  public float health = 100;

  public void Hurt(float amt) {
    health -= amt;

    if (health <= 0) {
      Die();
    }
  }

  public void Die() {
    Destroy(this.gameObject);
  }
}
