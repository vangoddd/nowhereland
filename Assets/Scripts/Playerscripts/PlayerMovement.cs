using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {
  private Vector2 movement;

  public float moveSpeed = 5f;
  public Rigidbody2D rb;

  private bool moving = false;
  public bool interacting = false;
  private Vector2 lastClickedPos;
  private Vector2 moveDir;
  private Vector2 lastPosition;

  public GameObject dustParticle;
  private float dustTimer = 0f;
  public float dustInterval = 1f;

  private GameObject moveToTarget = null;

  public float interactSearchRadius = 2f;

  private Animator animator;

  void Start() {
    animator = GetComponent<Animator>();
  }

  void Update() {
    //Handle idle sprite
    animator.SetFloat("lastPosX", moveDir.x);
    animator.SetFloat("lastPosY", moveDir.y);

    //Handle dust timer
    dustTimer += Time.deltaTime;

    if (moving && dustTimer >= dustInterval) {
      if (((Vector2)transform.position - lastPosition).sqrMagnitude > 0.0001f) {
        Instantiate(dustParticle, (Vector2)transform.position - moveDir * 0.5f, transform.rotation);
        dustTimer = 0f;
      }

    }

    // if (Input.GetMouseButton(0))
    // {
    //   if (!EventSystem.current.IsPointerOverGameObject())
    //   {
    //     UpdateMoveDir();
    //   }
    // }
    if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)) {
      // Check if finger is over a UI element
      if (Input.GetTouch(0).phase == TouchPhase.Began) {
        if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
          UpdateMoveDir();
        }
      } else {
        UpdateMoveDir();
      }
    }

    if (Input.GetKeyDown(KeyCode.F)) {
      MoveInteract();
    }
  }


  public void MoveInteract() {
    //Debug.Log("interacting");
    Collider2D[] hitColliders = Physics2D.OverlapCircleAll((Vector2)transform.position, interactSearchRadius);
    GameObject closest = null;
    foreach (var hit in hitColliders) {
      if (hit.gameObject.layer == 6) {
        if (!hit.gameObject.GetComponent<WorldObject>().interactable) continue;
        if (closest == null) closest = hit.gameObject;
        if (Vector3.Distance(hit.transform.position, transform.position) < Vector3.Distance(closest.transform.position, transform.position)) {
          closest = hit.gameObject;
        }
      }
    }

    if (closest != null) {
      // if (closest.GetComponent<WorldObject>().interactable)
      // {
      interacting = true;
      moveToTarget = closest;
      UpdateMoveDir((Vector2)closest.transform.position);
      //}
    }
  }

  void UpdateMoveDir() {
    lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    moveDir = (lastClickedPos - (Vector2)transform.position).normalized;
    moving = true;
    interacting = false;
  }

  void UpdateMoveDir(Vector2 point) {
    lastClickedPos = point;
    moveDir = (lastClickedPos - (Vector2)transform.position).normalized;
    moving = true;
  }

  void FixedUpdate() {
    lastPosition = transform.position;
    // rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    if (moving && (((Vector2)transform.position - lastClickedPos).sqrMagnitude > .15f)) {
      if (interacting) {
        WorldObject targetObject = moveToTarget.GetComponent<WorldObject>();
        if (targetObject.canInteract(transform.position)) {
          moving = false;
          interacting = false;
          targetObject.Interact(gameObject);
        } else {
          rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);

        }
      } else {
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);
      }
    } else {
      moving = false;
    }
  }

  public void setSpeed(float speed) {
    moveSpeed = speed;
    Debug.Log("Player speed changed to " + speed);
  }

  public void Teleport(float x, float y) {
    transform.position = new Vector3(x, y, transform.position.z);
  }

  void OnDrawGizmosSelected() {
    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere(transform.position, interactSearchRadius);
  }
}
