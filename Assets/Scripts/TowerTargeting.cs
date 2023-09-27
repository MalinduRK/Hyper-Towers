using UnityEngine;

public class TowerTargeting : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // TODO: Set the tower to target the enemy nearest to the mouse pointer when holding RMB

        /*
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Mouse position
        Vector3 lookDirection = mousePos - transform.position; // Difference between mouse and tower
        lookDirection.z = 0; // Ensure the object stays in the same plane.

        // Calculate the rotation to look at the mouse pointer.
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f; // Calculate the angle between the direction vector and the X-axis, and then convert it (rad value) to degrees
        // Adding a -90f makes it so that the barrel of the tower is pointing towards the target
        */

        //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Apply the rotation to the object.
        //transform.rotation = rotation;
    }
}
