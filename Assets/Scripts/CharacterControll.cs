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

    public float fallZone;

    private float speed = 0.0f;
    private float direction = 0f;
    private float horizontal = 0.0f;
    private float vertical = 0.0f;
    private AnimatorStateInfo stateInfo;
    private Vector3 lastPositionInScene;

    private int m_LocomotionID = 0;

    void Start()
    {
        animator = GetComponent<Animator>();

        if(animator.layerCount >= 2)
        {
            animator.SetLayerWeight(1, 1);
        }
        m_LocomotionID = Animator.StringToHash("Base Layer.Locomotion");
    }

    void FixedUpdate()
    {
        if(IsInLocomotion() && ((direction>=0 && horizontal>=0) || (direction <0 && horizontal < 0)))
        {
            Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, rotationDegreePerSecound * (horizontal < 0f ? -1f : 1f), 0f), Mathf.Abs(horizontal));
            Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
            this.transform.rotation = (this.transform.rotation * deltaRotation);
        }
    }

    public bool IsInLocomotion()
    {
        return stateInfo.nameHash == m_LocomotionID;
    }

    void Update()
    {
        if (animator)
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            horizontal = CnInputManager.GetAxis("Horizontal");
            vertical = CnInputManager.GetAxis("Vertical");

            StickToWorldspace(this.transform, gamecam.transform, ref direction, ref speed);

            animator.SetFloat("speed", speed);
            animator.SetFloat("direction", direction, directionDumpTime, Time.deltaTime);
        }
    }

    void LateUpdate()
    {
        if (this.transform.position.y < fallZone)
        {
            Debug.Log("Character to low");

            this.transform.position = lastPositionInScene;
        }
    }

    public void StickToWorldspace(Transform root, Transform camera, ref float directionOut, ref float speedOut)
    {
        Vector3 rootDirection = root.forward;

        Vector3 stickDirection = new Vector3(horizontal, 0, vertical);

        speedOut = stickDirection.sqrMagnitude;

        // Get camera rotation
        Vector3 CameraDirection = camera.forward;
        CameraDirection.y = 0.0f; // kill Y
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, Vector3.Normalize(CameraDirection));

        // Convert joystick input in Worldspace coordinates
        Vector3 moveDirection = referentialShift * stickDirection;
        Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

        float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);
        
        angleRootToMove /= 180f;

        directionOut = angleRootToMove * directionSpeed;
    }
}
