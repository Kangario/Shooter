using UnityEngine;
public interface IRotationPlaayer
{
    public void RotatePlayer();
}
public class RotationPlaayer : MonoBehaviour, IRotationPlaayer
{
    public float rotationSpeed = 10.0f;

    private bool isCombatMode = false;
    private Vector3 lookDirection;
    private CharacterController controller;
    public Transform cm;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        lookDirection = transform.forward;
    }

    void Update()
    {
        RotatePlayer();
    }
    public void RotatePlayer()
    {
        // Check if we switch between combat and non-combat mode
        if (Input.GetKeyDown(KeyCode.R))
        {
            isCombatMode = !isCombatMode;
        }

        // If in non-combat mode, rotate only when player moves
        if (!isCombatMode && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
        {
            Vector3 cameraForward = cm.forward;
            cameraForward.y = 0;
            lookDirection = Vector3.Slerp(lookDirection, cameraForward, Time.deltaTime * rotationSpeed);
        }

        // If in combat mode, smoothly rotate towards the camera's forward direction
        if (isCombatMode)
        {
            Vector3 cameraForward = cm.forward;
            cameraForward.y = 0;
            lookDirection = Vector3.Slerp(lookDirection, cameraForward, Time.deltaTime * rotationSpeed);
        }

        // Rotate the player towards the look direction
        if (lookDirection != Vector3.zero)
        {

            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}