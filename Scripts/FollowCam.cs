using UnityEngine;
using Mirror;

public class FollowCam : NetworkBehaviour
{
    public Transform PlayerTr;
    public float smoothTime = 0.1F;
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        Vector3 targetPosition = PlayerTr.TransformPoint(new Vector3(0, 0, -1));

        this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref velocity, smoothTime);  
    }

    public void setTarget(Transform target)
    {
        PlayerTr = target;
    }
}