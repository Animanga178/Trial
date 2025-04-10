using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform leftBarrier;
    [SerializeField] private Transform rightBarrier;

    private Vector3 initalCameraPosition;
    private Vector3 velocity = Vector3.zero;

    private float smoothTime = 0.25f;
    private float minY;
    public float LeftEdge { get; private set; }
    public float RightEdge { get; private set; }
    public float BottomEdge { get; private set; }

    private void Start()
    {
        initalCameraPosition = transform.position;
    }
    private void Update()
    {
        Vector3 targetPosition = target.position + initalCameraPosition;

        float camHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;

        targetPosition.x = Mathf.Clamp(targetPosition.x, leftBarrier.position.x + camHalfWidth, rightBarrier.position.x - camHalfWidth);

        BottomEdge = Camera.main.transform.position.y - Camera.main.orthographicSize;
        LeftEdge = Camera.main.transform.position.x - camHalfWidth;
        RightEdge = Camera.main.transform.position.x + camHalfWidth;

        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, float.MaxValue);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
