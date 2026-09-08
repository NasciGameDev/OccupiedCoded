using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Vector2        velocity;
    [SerializeField]
    private LayerMask      groundCheckLayers;
    [SerializeField]
    private Light2D         deathLight;
    
    [SerializeField, Header("Camera Points")]
    private Transform      cameraTarget1;

    [SerializeField, Header("Jump Stuff")]
    private float          gravityScaling = 2.0f;
    [SerializeField]
    private float          jumpMaxDuration = 0.1f;
    [SerializeField]
    private Transform      groundCheck;
    [SerializeField]
    private float          groundCheckRadius = 2.0f;

    [SerializeField, Header("Animator")]
    private Animator       Animator;


    
    private bool isMovementDeactivated;
    private float jumpTimer;
    private Rigidbody2D    rb;
    private string         horizontalAxisName = "Horizontal";
    private Camera         mainCamera;
    private bool           isGrounded;
    private float          originalGravity;
    private Vector2        currentVelocity;
    private Vector2        originalVelocity;
    private HealthSystem   health;
    private Collider2D playerCollider;
    private PlayerHiding playerHiding;



    private EnemyAI enemy;
    private Collider2D enemyCollider;
    private HouseSafeAnim finalScene;
    private bool startFinalScene;

    public bool isPlayerDead {  get; private set; }

    private CheckpointManager checkpointManager;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        health = GetComponent<HealthSystem>();
        playerHiding = GetComponent<PlayerHiding>();
        originalVelocity = velocity;
        enemy = FindFirstObjectByType<EnemyAI>();
        enemyCollider = enemy.GetComponent<Collider2D>();
        playerCollider = GetComponent<Collider2D>();
        finalScene = FindFirstObjectByType<HouseSafeAnim>();
        isPlayerDead = false;
        deathLight.enabled = false;

        checkpointManager = FindFirstObjectByType<CheckpointManager>();

        transform.position = checkpointManager.LastCheckpointPosition();

        ActivateMovement();
    }

    // Update is called once per frame
    void Update()
    {

        if (isMovementDeactivated)
        {
            rb.linearVelocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
            return;
        }
        ComputeGrounded();

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float moveDir = Input.GetAxis(horizontalAxisName);


        currentVelocity = rb.linearVelocity;

        currentVelocity.x = moveDir * velocity.x;


        if (mousePos.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        else if (mousePos.x > transform.position.x)
        {
            transform.rotation = Quaternion.identity;
        }


        // Jump Button
        
        /*if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                currentVelocity.y = velocity.y;
                jumpTimer = 0.0f;
                rb.gravityScale = gravityScaling;
            }
        }
        else if (jumpTimer < jumpMaxDuration)
        {
            jumpTimer += Time.deltaTime;
            if (Input.GetButton("Jump"))
            {
                rb.gravityScale = Mathf.Lerp(gravityScaling, originalGravity, jumpTimer / jumpMaxDuration);
            }
            else
            {
                jumpTimer = jumpMaxDuration;
                rb.gravityScale = originalGravity;
            }
        }   
        else
        {
            rb.gravityScale = originalGravity;
        }*/
        
        // Sneak Button
        if (Input.GetKey(KeyCode.LeftShift) == true)
        {
            currentVelocity.x /= 3;
        }


        // Crouch Button
        if (Input.GetKey(KeyCode.LeftControl) == true)
        {
            currentVelocity.x /= 3;
        }   


        rb.linearVelocity = currentVelocity;

        PlayerKill();

        IsPlayerHiding();

        startFinalScene = finalScene.activateFinalScene;
        if (startFinalScene == true)
        {
            GameObject flashlight = GameObject.Find("Flashlight");
            if (flashlight != null)
            {
                flashlight.SetActive(false);
            }

        }
    }
    public float GetCurrentVelocity()
    {
        return currentVelocity.x;
    }

    private void ComputeGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundCheckLayers);

        if (collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public Transform GetCameraTarget()
    {
        return cameraTarget1;
    }

    private void PlayerKill()
    {
        if (health.getHealth <= 0)
        {
            velocity = new Vector2(0, 0);
            rb.linearVelocity = velocity;
            deathLight.enabled = true;
            isPlayerDead = true;

            Animator.SetTrigger("Die");
            StartCoroutine(RespawnCooldown());

            IEnumerator RespawnCooldown()
            {
                yield return new WaitForSeconds(2f); 
                SceneTransition.TransitionToScene("Main Menu Restart");
            }
            GameObject flashlight = GameObject.Find("Flashlight");
            if (flashlight != null)
            {
                flashlight.SetActive(false);
            }
        }
    }

    private void IsPlayerHiding()
    {


        if (playerHiding.currentlyHiding == true)
        {
            velocity = new Vector2(0, 0);


            Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);
        }
        else
        {
            velocity = originalVelocity;
            Physics2D.IgnoreCollision(playerCollider, enemyCollider, false);
        }
    }

    public void DeactivateMovement() => isMovementDeactivated = true;
    public void ActivateMovement() => isMovementDeactivated = false;



}
