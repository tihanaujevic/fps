using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private float movementSpeed = 5;
    [SerializeField] private float gravity = -10;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float groundDistance = 0.5f;
    [SerializeField] LayerMask groundMask;
    private bool isGrounded;
    private Vector3 velocity;
    private float jumpHeight = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * movementSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * gravity);

        velocity.y -= gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        Crouching();

        //ako je pa sa plohe
        if (transform.position.y <= -5)
            ResetLevel();
    }

    private void Crouching()
    {
        bool isCrouching = Input.GetKey(KeyCode.C);

        float targetHeight = isCrouching ? 1f : 2f;
        float targetCameraY = isCrouching ? 0.5f : 1.5f;
        float targetCenterY = isCrouching ? 0.5f : 1f;

        characterController.height = Mathf.Lerp(characterController.height, targetHeight, 2 * Time.deltaTime);
        characterController.center = new Vector3(0, Mathf.Lerp(characterController.center.y, targetCenterY, 2 * Time.deltaTime), 0);
        playerCamera.localPosition = new Vector3(0, Mathf.Lerp(playerCamera.localPosition.y, targetCameraY, 2 * Time.deltaTime), 0);
    }

    private void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
