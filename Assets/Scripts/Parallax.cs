using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxFactor; // Define o quanto esse layer se move em rela��o � c�mera
    private Transform cam; // A c�mera principal
    private Vector3 previousCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;
    }

    void Update()
    {
        Vector3 deltaMovement = cam.position - previousCamPos;
        transform.position += deltaMovement * parallaxFactor;
        previousCamPos = cam.position;
    }
}
