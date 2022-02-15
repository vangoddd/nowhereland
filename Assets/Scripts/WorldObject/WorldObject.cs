using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObject : MonoBehaviour
{

  public float interactDistance = 1f;
  public Vector3 offset = Vector3.zero;
  public bool interactable = true;
  // Start is called before the first frame update
  void Start()
  {
    ChunkHandlerScript.addObjectToChunk(gameObject);
    BoxCollider2D clickHitbox = gameObject.AddComponent<BoxCollider2D>() as BoxCollider2D;
    clickHitbox.isTrigger = true;
  }

  public abstract void Interact(GameObject player);

  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere(transform.position + offset, interactDistance);
  }

  public bool canInteract(Vector2 player)
  {
    if (!interactable) return false;
    if (Vector2.Distance(player, transform.position + offset) <= interactDistance)
    {
      return true;
    }
    return false;
  }

}
