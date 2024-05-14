using System.Collections;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput input;
    InputAction moves;
    InputAction moveCam;
    public CombatManager combatManager;
    public Animator animator;
    [SerializeField] Debugger debug;

    [SerializeField] Rigidbody rb;

    Vector3 jump;
    readonly float _speed = 0.000001f;
    bool RayHit;
    float speed = 12f;
    float camSpeed = 200f;

    public bool move = true;
    public bool uimove;
    public LayerMask PlayerMask;

    Ray ray_forward;
    Ray ray_back;
    Ray ray_left;
    Ray ray_right;
    Ray ray_leftforward;
    Ray ray_leftback;
    Ray ray_rightforward;
    Ray ray_rightback;
    RaycastHit hit;

    Vector2 currentVelo;
    Vector2 smoothVelo;
    Vector2 currentMouseVelo;
    Vector2 smoothMouseVelo;
    [SerializeField] float smoothInputSpeed = 0.2f;

    private void Start()
    {
        jump = new Vector3(0, 2, 0);
        Physics.gravity *= 2.5f;
        Debug.Log("PlayerMovement script started.");
        StartRays();
        input = GetComponent<PlayerInput>();
        moves = input.actions.FindAction("move");
        moveCam = input.actions.FindAction("moveCam");
    }

    void Update()
    {
        if (move)
            HandleMovementInput();
        UpdateRays();

        Vector2 moveInput = moves.ReadValue<Vector2>();
        if (!combatManager.CheckCombatState())
        {
            if (moveInput.magnitude > 0)
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Idle", false);
            }
            else
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Walk", false);
            }
        }
        else
        {
            if (animator.GetBool("Walk"))
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Idle", true);
            }
        }
    }

    void HandleMovementInput()
    {
        if (RayHit) MovePlayer(_speed);
        if (!RayHit) MovePlayer(speed);
        //rotCam();
    }
    /* Old
     * void MovePlayer(float speed)
    {
        Vector2 moveInput = moves.ReadValue<Vector2>();
        currentVelo = Vector2.SmoothDamp(currentVelo, moveInput, ref smoothVelo, smoothInputSpeed);

        if (moveInput.magnitude > 0)
        {
            Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            transform.position += speed * Time.deltaTime * moveDir;

            // Determine the direction the player should face
            Vector3 lookDir = new Vector3(moveInput.x, 0f, moveInput.y);
            if (lookDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDir, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720 * Time.deltaTime);
            }
        }
        animator.SetBool("isWalking", true);
        if (debug.debugerEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.position -= new Vector3(0f, 1f, 0f);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.position += new Vector3(0f, 1f, 0f);
            }
        }
    } */

    // New and Experimental
    void MovePlayer(float speed)
    {
        Vector2 moveInput = moves.ReadValue<Vector2>();
        currentVelo = Vector2.SmoothDamp(currentVelo, moveInput, ref smoothVelo, smoothInputSpeed);

        if (moveInput.magnitude > 0)
        {
            // Calculate the movement direction based on player's rotation
            Vector3 moveDir = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * new Vector3(moveInput.x, 0f, moveInput.y);
            transform.position += speed * Time.deltaTime * moveDir;

            // Calculate the direction the player should face based on input
            Vector3 lookDir = new Vector3(moveInput.x, 0f, moveInput.y);

            if (lookDir != Vector3.zero)
            {
                // Get the camera's forward direction
                Vector3 cameraForward = Camera.main.transform.forward;

                // Project the camera's forward direction onto the XZ plane
                cameraForward.y = 0f;
                cameraForward.Normalize();

                // Calculate the rotation needed to align the player with the movement direction relative to the camera
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward * lookDir.z + Camera.main.transform.right * lookDir.x, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720 * Time.deltaTime);
            }
        }
        if (debug.debugerEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.position -= new Vector3(0f, 1f, 0f);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.position += new Vector3(0f, 1f, 0f);
            }
        }
    }
    void rotCam()
    {
        Vector2 rotInput = moveCam.ReadValue<Vector2>();
        currentMouseVelo = Vector2.SmoothDamp(currentMouseVelo, rotInput, ref smoothMouseVelo, smoothInputSpeed);
        //Debug.Log(currentMouseVelo);
        //float rotAmount;
        //if (Input.GetJoystickNames().Length > 0) rotAmount = rotInput.x * camSpeed * Time.deltaTime;
        //rotAmount = rotInput.x * (camSpeed * 2) * Time.deltaTime;
        float rotAmount = currentMouseVelo.x * camSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotAmount, Space.World);
    }
    void StartRays()
    {
        ray_forward = new(transform.position, transform.forward);
        ray_back = new(transform.position, transform.forward * -1);
        ray_right = new(transform.position, transform.right);
        ray_left = new(transform.position, transform.right * -1);
        ray_leftforward = new(transform.position, transform.forward + transform.right);
        ray_leftback = new(transform.position, transform.forward - transform.right);
        ray_rightforward = new(transform.position, transform.forward * -1 + transform.right);
        ray_rightback = new(transform.position, transform.forward * -1 - transform.right);
    }

    void UpdateRays()
    {
        ray_forward.origin = transform.position;
        ray_back.origin = transform.position;
        ray_right.origin = transform.position;
        ray_left.origin = transform.position;
        ray_forward.direction = transform.forward;
        ray_back.direction = transform.forward * -1;
        ray_right.direction = transform.right;
        ray_left.direction = transform.right * -1;
        ray_leftforward.origin = transform.position;
        ray_leftforward.direction = transform.forward + transform.right;
        ray_leftback.origin = transform.position;
        ray_leftback.direction = transform.forward - transform.right;
        ray_rightforward.origin = transform.position;
        ray_rightforward.direction = transform.forward * -1 + transform.right;
        ray_rightback.origin = transform.position;
        ray_rightback.direction = transform.forward * -1 - transform.right;
        DrawRays();
        CheckRaycastHits();
    }

    void DrawRays()
    {
        Debug.DrawRay(ray_forward.origin, ray_forward.direction * 10);
        Debug.DrawRay(ray_back.origin, ray_back.direction * 10);
        Debug.DrawRay(ray_right.origin, ray_right.direction * 10);
        Debug.DrawRay(ray_left.origin, ray_left.direction * 10);
        Debug.DrawRay(ray_leftforward.origin, ray_leftforward.direction * 10);
        Debug.DrawRay(ray_leftback.origin, ray_leftback.direction * 10);
        Debug.DrawRay(ray_rightforward.origin, ray_rightforward.direction * 10);
        Debug.DrawRay(ray_rightback.origin, ray_rightback.direction * 10);
    }

    void CheckRaycastHits()
    {
        if (debug.collision)
        {
            RayHit = false;
            return;
        }
        if (Physics.Raycast(ray_forward, out hit, 0.5f, ~PlayerMask) ||
            Physics.Raycast(ray_back, out hit, 0.5f, ~PlayerMask) ||
            Physics.Raycast(ray_right, out hit, 0.5f, ~PlayerMask) ||
            Physics.Raycast(ray_left, out hit, 0.5f, ~PlayerMask) ||
            Physics.Raycast(ray_leftforward, out hit, 0.5f, ~PlayerMask) ||
            Physics.Raycast(ray_leftback, out hit, 0.5f, ~PlayerMask) ||
            Physics.Raycast(ray_rightforward, out hit, 0.5f, ~PlayerMask) ||
            Physics.Raycast(ray_rightback, out hit, 0.5f, ~PlayerMask))
        {
            if (!hit.collider.gameObject.CompareTag("OBELISK") && !hit.collider.gameObject.CompareTag("COMBAT")&& !hit.collider.gameObject.CompareTag("gen"))
            {
                RayHit = true;
            }

        }
        else
        {
            RayHit = false;
        }
    }
}
