using UnityEngine;

public class TextLookToCamera : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(new Vector3(0, 180, 0));
    }
}
