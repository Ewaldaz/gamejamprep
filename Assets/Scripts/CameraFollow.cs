using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Player;
    public float TransitionSpeed = 5f;
    public Vector3 Offset;

    public void FixedUpdate()
    {
        Vector3 position = Player.position + Offset;
        Vector3 transition = Vector3.Lerp(transform.position, position, TransitionSpeed * Time.deltaTime);
        transform.position = transition;
    }
}