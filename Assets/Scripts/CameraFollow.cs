using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
    private float distanceAway;
    [SerializeField]
    private float distanceUp;
    [SerializeField]
    private float smooth;
    [SerializeField]
    private Transform follow;
    [SerializeField]
    private float camSmoothDampTime = 0.1f;

    private Vector3 lookDir;
    private Vector3 targetPosition;
    private Vector3 velocityCamSmopth = Vector3.zero;
    

    private void Start()
    {
        follow = GameObject.FindWithTag("FollowPlayer").transform;
    }

    private void Update()
    {
        
    }

    void LateUpdate()
    {
        Vector3 characterOffset = follow.position + new Vector3(0f, distanceUp, 0f);

        lookDir = characterOffset - this.transform.position;

        lookDir.y = 0;
        lookDir.Normalize();

        targetPosition = characterOffset + follow.up * distanceUp - lookDir * distanceAway;

        CompensateWalls(characterOffset, ref targetPosition);
        SmoothPosition(this.transform.position, targetPosition);

        transform.LookAt(follow);
    }

    private void SmoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmopth, camSmoothDampTime);
    }

    private void CompensateWalls(Vector3 fromObject, ref Vector3 toTarget)
    {
        RaycastHit wallHit = new RaycastHit();
        if (Physics.Linecast(fromObject, toTarget, out wallHit))
        {
            toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
        }
    }

}
