using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
  public int enemyID;
  public float health = 100;
  public float moveSpeed = 0.5f;
  public float damage = 10f;

  private float stunTimer = 0f;
  private float attackTimer = 0f;

  public bool aggro = false;
  public float aggroRange = 5f;
  public float offAggroRange = 15f;

  public LootTable drops;

  public EnemyHandler _enemyHandler;
  private Rigidbody2D rb;
  private SpriteRenderer spriteRenderer;

  public HealthInteraction _healthInteraction;

  public GameObject[] damageAnimation;

  void Start() {
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    _enemyHandler.enemyList.Add(this);
    //ChunkHandlerScript.addObjectToChunk(this.gameObject);
  }

  public void Stun(float duration) {
    stunTimer = duration;
  }

  public void Hurt(float amt) {
    health -= amt;
    Stun(0.5f);
    StartCoroutine(ApplyFlashingEffect(0.1f));

    int RandomChoice = Random.Range(0, damageAnimation.Length);
    Instantiate(damageAnimation[RandomChoice], transform.position, transform.rotation);

    if (health <= 0) {
      Die();
    }
  }

  public void Die() {
    _enemyHandler.enemyList.Remove(this);
    ChunkHandlerScript.removeObjectFromChunk(this.gameObject);

    ItemSpawner.Instance.spawnDrops(transform.position, drops);

    Destroy(this.gameObject);
  }

  IEnumerator ApplyFlashingEffect(float duration) {
    spriteRenderer.color = Color.red;
    yield return new WaitForSeconds(duration);
    spriteRenderer.color = Color.white;
    yield return null;
  }

  void Update() {
    if ((_enemyHandler._playerStat.position - (Vector2)this.transform.position).sqrMagnitude <= aggroRange * aggroRange && aggro == false) {
      aggro = true;
      _enemyHandler.ChangeAggro(this, aggro);
    }
    if ((_enemyHandler._playerStat.position - (Vector2)this.transform.position).sqrMagnitude >= offAggroRange * offAggroRange && aggro == true) {
      aggro = false;
      _enemyHandler.ChangeAggro(this, aggro);
    }

    if (stunTimer > 0) {
      stunTimer -= Time.deltaTime;
    }

    if (attackTimer > 0) {
      attackTimer -= Time.deltaTime;
    }
  }

  void FixedUpdate() {
    //chase player
    if (aggro) {
      if (stunTimer <= 0) {
        Vector2 moveDir = (_enemyHandler._playerStat.position - (Vector2)this.transform.position).normalized;
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);
      }
    }
  }

  void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.tag == "Player") {
      Debug.Log("apply damage");
      _healthInteraction.HurtPlayer(damage);
      attackTimer = 1f;
    }
  }

  void OnCollisionStay2D(Collision2D collision) {
    if (collision.gameObject.tag == "Player") {
      Stun(1f);
      if (attackTimer <= 0f) {
        _healthInteraction.HurtPlayer(damage);
        attackTimer = 1f;
      }
    }
  }

}
