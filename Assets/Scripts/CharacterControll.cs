using UnityEngine;
using CnControls;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;


public class CharacterControll : MonoBehaviour {

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float directionDumpTime = .25f;
    [SerializeField]
    private CameraFollow gamecam;
    [SerializeField]
    private float directionSpeed = 1.0f;
    [SerializeField]
    private float rotationDegreePerSecound = 120f;

    private CharacterController controller;

    private float speed = 0.0f;
    private float direction = 0f;
    private float verticalVelocity;
    private float gravity = 14.0f;
    private float jumpForce = 10.0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        if (animator.layerCount >= 2)
        {
            animator.SetLayerWeight(1, 1);
        }
    }

    void Update()
    {
        if (animator)
        {
            if (controller.isGrounded)
            {
                verticalVelocity = -gravity * Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    verticalVelocity = jumpForce;
                }
            }
            else
            {
                verticalVelocity -= gravity * Time.deltaTime;
            }

            Vector3 moveVector = Vector3.zero;
            moveVector.x = Input.GetAxis("Horizontal");
            moveVector.y = verticalVelocity;
            moveVector.z = Input.GetAxis("Vertical");
            controller.Move(moveVector * Time.deltaTime);

            animator.SetFloat("speed", speed);
            animator.SetFloat("direction", direction, directionDumpTime, Time.deltaTime);
        }
    }
}
