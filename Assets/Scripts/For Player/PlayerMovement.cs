using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    private AudioSource audioSource;

    [Header("Clips")]
    public AudioClip walkClip;
    public AudioClip runClip;
    public AudioClip jumpClip;
    [SerializeField] private AudioClip switchWeaponClip;
    public AudioClip hurtClip;

    [Header("Character UI")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI arrowCountText;
    private int health = 300;
    int killCount = 0;
    int arrowCount = 50;


    [Header("Character Stats")]
    public Camera playerCamera;
    float xRotation = 0f;
    private float mouseSensitivity = 250f;
    private float moveSpeed = 10f;
    private float runSpeed = 18f;
    float currentSpeed;
    float jumpForce = 10f;
    float moveX;
    float moveZ;
    bool isMoving;
    bool isRunning;
    bool isGrounded;
    [SerializeField] float extraGravityMultiplier = 2.5f;

    [Header("Ground Check With Raycast")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.3f;
    [SerializeField] private LayerMask groundLayer;


    [Header("Weapons")]
    public GameObject sword;
    public GameObject bow;

    int currentWeapon = 1;

    bool isAttacking = false;
    bool isChangingWeapon = false;

    [Header("Other Scripts Reference")]
    public Bow bowScript;
    public Sword swordScript;

    [Header("Game Over UI")]
    public GameObject losePanel;
    public GameObject winPanel;
    public Button restartButton;
    public Button quitButton;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        healthText.text = health.ToString();
        killCountText.text = killCount.ToString();
        arrowCountText.text = arrowCount.ToString();

        EquipSword();
    }


    void Update()
    {
        if (MenuManager.IsGamePaused)
            return;

        if (health <= 0)
        {
            GameOver(false);
            return;
        }

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        isMoving = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f;
        isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving;

        HandleMovementState();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }


        if (Input.GetMouseButtonDown(0) && !isAttacking && !isChangingWeapon)
        {
            HandleAttack();
        }



        HandleWeaponSwitch();
    }
    void HandleWeaponSwitch()
    {
        if (isAttacking || isChangingWeapon) return;

        if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeapon != 1)
            StartCoroutine(ChangeWeaponRoutine(1));

        if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon != 2)
            StartCoroutine(ChangeWeaponRoutine(2));
    }

    IEnumerator ChangeWeaponRoutine(int weapon)
    {
        isChangingWeapon = true;

        yield return null;

        if (weapon == 1)
            EquipSword();
        else
            EquipBow();

        isChangingWeapon = false;
    }

    void EquipSword()
    {
        currentWeapon = 1;

        sword.SetActive(true);
        bow.SetActive(false);

        if (switchWeaponClip != null)
            audioSource.PlayOneShot(switchWeaponClip);
    }

    void EquipBow()
    {
        currentWeapon = 2;

        sword.SetActive(false);
        bow.SetActive(true);

        if (switchWeaponClip != null)
            audioSource.PlayOneShot(switchWeaponClip);
    }

    void HandleAttack()
    {
        if (currentWeapon == 1)
        {
            SwordAttack();
        }
        else if (currentWeapon == 2)
        {
            BowAttack();
        }
    }
    void SwordAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        swordScript.HitSword();
    }

    void BowAttack()
    {
        if (arrowCount <= 0)
            return;

        bool shot = bowScript.Shoot();

        if (!shot)
            return;

        arrowCount--;
        arrowCountText.text = arrowCount.ToString();
    }

    public void SetAttacking(bool value)
    {
        isAttacking = value;
    }




    private void FixedUpdate()
    {
        CheckGround();

        Vector3 velocity = transform.forward * moveZ + transform.right * moveX;
        velocity *= currentSpeed;
        velocity.y = rb.linearVelocity.y;

        if (!isGrounded)
        {
            Vector3 origin = transform.position + Vector3.up * 0.5f;
            RaycastHit hit;
            if (Physics.Raycast(origin, transform.forward, out hit, 0.6f))
            {
                velocity.x = 0f;
                velocity.z = 0f;
            }

            rb.AddForce(Physics.gravity * extraGravityMultiplier, ForceMode.Acceleration);
        }

        rb.linearVelocity = velocity;
    }

    void HandleMovementState()
    {
        if (!isGrounded)
        {
            currentSpeed = isRunning ? runSpeed : moveSpeed;
            audioSource.Stop();
            return;
        }

        if (!isMoving)
        {
            currentSpeed = 0f;
            audioSource.Stop();
            return;
        }

        if (isRunning)
        {
            currentSpeed = runSpeed;

            if (audioSource.clip != runClip)
            {
                audioSource.clip = runClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            currentSpeed = moveSpeed;

            if (audioSource.clip != walkClip)
            {
                audioSource.clip = walkClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }
    void Jump()
    {
        if (!isGrounded) return;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        if (jumpClip != null)
            audioSource.PlayOneShot(jumpClip);
    }
    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundRadius,
            groundLayer
        );
    }

    private void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Animal"))
        {
            health -= 20;
            health = Mathf.Max(health, 0);
            healthText.text = health.ToString();
            if (hurtClip != null)
                audioSource.PlayOneShot(hurtClip);
        }
    }

    public void AddKill()
    {
        killCount++;
        killCountText.text = killCount.ToString();
        if (killCount >= 50)
        {
            GameOver(true);
        }
    }
    void GameOver(bool isWin)
    {
        Time.timeScale = 0f;

        if (isWin)
        {
            winPanel.SetActive(true);
            restartButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);

        }

        else
        {
            losePanel.SetActive(true);
            restartButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
