using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
  private Vector2 movement;

  public float moveSpeed = 5f;
  public Rigidbody2D rb;

  private bool moving = false;
  public bool interacting = false;
  private bool attacking = false;
  private Vector2 lastClickedPos;
  private Vector2 moveDir;
  private Vector2 lastPosition;

  private int lastChunkPos;

  public GameObject dustParticle;
  private float dustTimer = 0f;
  public float dustInterval = 1f;

  private GameObject moveToTarget = null;

  public float interactSearchRadius = 2f;

  private Animator animator;
  private PlayerInput playerInput;

  private bool UIOpen = false;

  private bool isUsing = false;

  [SerializeField] private InteractSO _interactSO;
  [SerializeField] private MapSO map;
  [SerializeField] private GameEvent OnChunkChanged;
  [SerializeField] private PlayerStats _playerStat;
  [SerializeField] private Inventory _inventory;

  private int enemyLayerMask;
  [SerializeField] private float attackRadius;

  void Start() {
    enemyLayerMask = LayerMask.GetMask("Enemy");
    animator = GetComponent<Animator>();
    playerInput = GetComponent<PlayerInput>();
  }

  void OnEnable() {
    _interactSO.OnInteractMove.AddListener(MoveInteract);
  }

  void OnDisable() {
    _interactSO.OnInteractMove.RemoveListener(MoveInteract);
  }

  void Update() {
    //Handle aniamtion sprite
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

    if (isMoveHold) {
      UpdateMoveDir();
    }

    int curChunkPos = Mathf.FloorToInt(transform.position.x / map.chunkSize) + (2 * Mathf.FloorToInt(transform.position.y / map.chunkSize));
    if (curChunkPos != lastChunkPos) {
      lastChunkPos = curChunkPos;
      OnChunkChanged.Raise();
    }

  }


  public void MoveInteract() {
    //Debug.Log("interacting");
    Collider2D[] hitColliders = Physics2D.OverlapCircleAll((Vector2)transform.position, interactSearchRadius);
    GameObject closest = null;
    foreach (var hit in hitColliders) {
      if (hit.gameObject.layer == 6) {
        if (!hit.gameObject.GetComponent<WorldObject>().ToolRangeCheck()) continue;
        if (closest == null) closest = hit.gameObject;
        if (Vector3.Distance(hit.transform.position, transform.position) < Vector3.Distance(closest.transform.position, transform.position)) {
          closest = hit.gameObject;
        }
      }
    }
    //Debug.Log("Closest : " + closest.name);

    if (closest != null) {
      interacting = true;
      moveToTarget = closest;
      UpdateMoveDir((Vector2)closest.transform.position);
    }

  }

  //Attack handling
  public void MoveAttack() {
    //Debug.Log("interacting");
    Collider2D[] hitColliders = Physics2D.OverlapCircleAll((Vector2)transform.position, interactSearchRadius, enemyLayerMask);
    GameObject closest = null;
    foreach (var hit in hitColliders) {
      if (closest == null) closest = hit.gameObject;
      if (Vector3.Distance(hit.transform.position, transform.position) < Vector3.Distance(closest.transform.position, transform.position)) {
        closest = hit.gameObject;
      }
    }
    //Debug.Log("Closest : " + closest.name);

    if (closest != null) {
      attacking = true;
      moveToTarget = closest;
      UpdateMoveDir((Vector2)closest.transform.position);
    }

  }

  void UpdateMoveDir() {
    if (UIOpen) return;
    // lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    lastClickedPos = Camera.main.ScreenToWorldPoint(touchPosition);
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
    if (moving) {
      _playerStat.PlayerMove((Vector2)transform.position);
    }
    lastPosition = transform.position;
    // rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    if (moving && (((Vector2)transform.position - lastClickedPos).sqrMagnitude > .15f)) {
      //Handle interact
      if (interacting) {
        WorldObject targetObject = moveToTarget.GetComponent<WorldObject>();
        if (targetObject.canInteract(transform.position)) {
          moving = false;
          interacting = false;
          targetObject.Interact(gameObject);
        } else {
          rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);
        }
      }
      //Handle use
      else if (isUsing) {
        Useable targetObject = moveToTarget.GetComponent<WorldObject>() as Useable;
        if (targetObject.canInteract(transform.position)) {
          moving = false;
          isUsing = false;
          targetObject.UseObject();
        } else {
          rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);
        }

      }
      //handle attack
       else if (attacking) {
        Enemy targetObject = moveToTarget.GetComponent<Enemy>();
        if (canAttack(targetObject)) {
          moving = false;
          attacking = false;

          //Applying damage to enemy
          if (_inventory.handSlot != null && _inventory.handSlot.itemData is Weapon) {
            targetObject.Hurt((_inventory.handSlot.itemData as Weapon).damage);
          } else {
            targetObject.Hurt(10);
          }
        } else {
          rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);
        }
      }
      //handle move only
       else {
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);
      }
    } else {
      moving = false;
    }
  }

  private bool canAttack(Enemy e) {
    if ((transform.position - e.transform.position).sqrMagnitude <= attackRadius * attackRadius) {
      return true;
    }
    return false;
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

  private Vector2 touchPosition = Vector2.zero;
  private bool isMoveHold = false;

  public void OnTouch() {
    if (!(IsPointerOverUIObject())) {
      //do raycast to get hit obj
      RaycastClickedObject();

      UpdateMoveDir();
    }
  }

  public void OnTouchPosition(InputValue value) {
    touchPosition = value.Get<Vector2>();
    //Debug.Log(touchPosition);
  }

  public void OnHold(InputValue value) {
    //Debug.Log("OnHold");
    if (value.isPressed) {
      if (!IsPointerOverUIObject()) isMoveHold = true;
    } else {
      isMoveHold = false;
    }
  }

  public void OnAttack() {
    MoveAttack();
  }

  public void OnInteract() {
    //Debug.Log("on interact");
    MoveInteract();
  }

  public void OnPause() {
    TimeManager.Instance.togglePause();
  }

  public void CancelMovement() {
    Debug.Log("cancelling movement");
    moving = false;
    isUsing = false;
  }

  public void OnUIOpen() {
    playerInput.SwitchCurrentActionMap("UI");
    UIOpen = true;
  }

  public void OnUIClosed() {
    playerInput.SwitchCurrentActionMap("Gameplay");
    UIOpen = false;
  }

  private bool IsPointerOverUIObject() {
    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    eventDataCurrentPosition.position = touchPosition;
    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    return results.Count > 0;
  }

  private void RaycastClickedObject() {
    Vector2 origin = Vector2.zero;
    Vector2 dir = Vector2.zero;
    Ray ray = Camera.main.ScreenPointToRay(touchPosition);
    origin = ray.origin;
    dir = ray.direction;

    RaycastHit2D hit = Physics2D.Raycast(origin, dir);

    //Check if we hit anything
    if (hit) {
      if (hit.collider.gameObject.layer == 6) { //if hit on worldobject
        WorldObject obj = hit.collider.gameObject.GetComponent<WorldObject>();
        if (obj != null && obj is Useable) {
          if (obj.canInteract(transform.position)) {
            ((Useable)obj).UseObject();
          } else {
            isUsing = true;
            moveToTarget = obj.gameObject;
          }
        }
      }
    }
  }
}

