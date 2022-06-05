using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObject : MonoBehaviour {

  public int objectID;

  public float interactDistance = 1f;
  public Vector3 offset = Vector3.zero;
  public bool interactable = true;

  public Inventory _inventory;
  public LootTable drops;

  public MapSO _map;

  public int status = -1;

  public AudioRef interactSound;
  public AudioRef destroySound;

  protected void InitializeObject() {
    ChunkHandlerScript.addObjectToChunk(gameObject);
    ChunkHandlerScript.GenerateMapIcon(objectID, transform.position);
    BoxCollider2D clickHitbox = gameObject.AddComponent<BoxCollider2D>() as BoxCollider2D;
    clickHitbox.isTrigger = true;

    _map.worldObjects.Add(gameObject);
  }

  void Start() {
    InitializeObject();
  }

  public virtual void Interact(GameObject player) {
    AudioManager.Instance.PlayOneShot(interactSound);
  }

  void OnDrawGizmosSelected() {
    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere(transform.position + offset, interactDistance);
  }

  public abstract bool ToolRangeCheck();

  public bool canInteract(Vector2 player) {
    if (!interactable) return false;
    if (Vector2.Distance(player, transform.position + offset) <= interactDistance) {
      return true;
    }
    return false;
  }

  public virtual void DestroyWorldObject() {
    //remove itself from chunk thing
    ChunkHandlerScript.removeObjectFromChunk(gameObject);
    _map.worldObjects.Remove(gameObject);

    //give item drop
    ItemSpawner.Instance.spawnDrops(transform.position, drops);
    ChunkHandlerScript.DestroyIcon(transform.position);

    AudioManager.Instance.PlayOneShot(destroySound);
    //destroy
    Destroy(gameObject);
  }

  public virtual void OnDataLoad() {

  }

}
