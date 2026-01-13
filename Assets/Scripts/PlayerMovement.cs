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

    [Header("Character Stats")]
    public Camera playerCamera;
    float xRotation = 0f;
    private float mouseSensitivity = 250f;
    private float moveSpeed = 10f;
    private float runSpeed = 16f;
    float jumpForce = 7f;
    float moveX;
    float moveZ;
    bool isMoving;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        rb.freezeRotation = true;
    }

    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        isMoving = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f;
        if (isMoving)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = walkClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }

    }

    private void FixedUpdate()
    {
        Vector3 moveDir = transform.forward * moveZ + transform.right * moveX;
        moveDir *= moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + moveDir);
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
}
