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

    [Header("Weapons")]
    public GameObject sword;
    public GameObject bow;

    int currentWeapon = 1;

    [Header("Other Scripts Reference")]
    public Bow bowScript;
    public Sword swordScript;




    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        rb.freezeRotation = true;

        EquipSword();

    }

    void Update()
    {
        if (MenuManager.IsGamePaused)
            return;

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        isMoving = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f;
        isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving;

        HandleMovementState();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleAttack();
        }


        HandleWeaponSwitch();
    }
    void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeapon != 1)
        {
            EquipSword();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon != 2)
        {
            EquipBow();
        }
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
        swordScript.HitSword();
    }
    void BowAttack()
    {
        if (arrowCount <= 0)
            return;

        bowScript.Shoot();

        arrowCount--;
        arrowCountText.text = arrowCount.ToString();
    }



    private void FixedUpdate()
    {
        Vector3 moveDir = transform.forward * moveZ + transform.right * moveX;
        moveDir *= currentSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + moveDir);

        if (!isGrounded)
        {
            rb.AddForce(Physics.gravity * extraGravityMultiplier, ForceMode.Acceleration);
        }
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
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        audioSource.PlayOneShot(jumpClip);
        isGrounded = false;
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
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
