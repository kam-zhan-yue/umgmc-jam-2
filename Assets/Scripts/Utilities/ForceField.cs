using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// This script provides a simplified gravitational pull to objects within its range.
[RequireComponent(typeof(Collider2D))]
public class ForceField : MonoBehaviour
{
    [Tooltip("The strength of the gravitational pull.")]
    [SerializeField] private float gravityStrength = 100f;

    [Tooltip("Layer mask to filter objects that are affected by gravity.")]
    [SerializeField] private LayerMask affectedLayers;
    [SerializeField] private string[] affectedTages = { "Physics" };
    [SerializeField] private float radius = 2f;
    [SerializeField] private GravityType gravityType = GravityType.OrbitalGravity;

    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the collided object is in the affected layers
        if (((1 << other.gameObject.layer) & affectedLayers) != 0 && !affectedTages.Contains(other.gameObject.tag))
        {
            Rigidbody2D otherRigidbody = other.attachedRigidbody;

            // Ensure the object has a Rigidbody2D
            if (otherRigidbody != null)
            {

                switch (gravityType)
                {
                    case GravityType.OrbitalGravity:
                        ApplyAdjustmentField(otherRigidbody, radius);
                        break;
                    case GravityType.SimpeRotation:
                        ApplyRotationalForceField(otherRigidbody);
                        break;
                    case GravityType.UniformGravityField:
                        ApplyForceField(otherRigidbody);
                        break;
                    default:
                        Debug.LogError("Gravity type not assigned" + gameObject);
                        break;
                }
            }
        }
    }


    private void ApplyForceField(Rigidbody2D targetRigidbody)
    {
        Vector2 directionToCenter = (Vector2)transform.position - targetRigidbody.position;
        targetRigidbody.AddForce(directionToCenter.normalized * gravityStrength);
    }


    private void ApplyRotationalForceField(Rigidbody2D targetRigidbody)
    {
        Vector2 directionToCenter = Vector2.Perpendicular((Vector2)transform.position - targetRigidbody.position);
        targetRigidbody.AddForce(directionToCenter.normalized * gravityStrength * 0.3f);

    }


    private void ApplyAdjustmentField(Rigidbody2D targetRigidbody, float radius)
    {
        Vector2 directionToCenter = (Vector2)transform.position - targetRigidbody.position;
        float distanceToCenter = directionToCenter.magnitude;

        // Avoid division by zero
        if (Mathf.Abs(distanceToCenter) < Mathf.Epsilon)
        {
            return; // Early return if the object is at the center
        }

        // Calculate the adjustment factor (k)
        float k = (1 - radius / distanceToCenter);

        // Apply the adjustment force
        if (Mathf.Abs(k) > 0.01f)
        {

            float stringConstant = gravityStrength;
            //F(t) = - 2sqrt(mk) * v - kx
            Vector2 adjustmentForce = -2 * Mathf.Sqrt(targetRigidbody.mass * stringConstant) * targetRigidbody.velocity + stringConstant * k * directionToCenter;
            Vector2 rotationalForce = Vector2.Perpendicular((Vector2)transform.position - targetRigidbody.position) * gravityStrength;

            targetRigidbody.AddForce(adjustmentForce + rotationalForce);
            //Debug.Log(adjustmentForce);




        }
    }




    private void RotateAroundTheCenter(Rigidbody2D targetRigidbody, float desiredRadius)
    {
        Vector2 directionToCenter = (Vector2)transform.position - targetRigidbody.position;
        float currentDistance = directionToCenter.magnitude;

        // Check if the object is approximately at the desired radius
        if (Mathf.Abs(currentDistance - desiredRadius) > 0.1f) // 0.1f is a tolerance value
        {
            // Apply a force towards or away from the center to adjust the radius
            float radialForceDirection = currentDistance > desiredRadius ? -1 : 1;
            Vector2 radialForce = directionToCenter.normalized * radialForceDirection * gravityStrength;
            targetRigidbody.AddForce(radialForce);
        }

        // Apply a force perpendicular to the direction to the center to maintain orbital motion
        Vector2 tangentialForceDirection = Vector2.Perpendicular(directionToCenter).normalized;
        float orbitalVelocity = Mathf.Sqrt(gravityStrength * desiredRadius); // Simplified orbital velocity formula
        Vector2 tangentialForce = tangentialForceDirection * orbitalVelocity;
        targetRigidbody.velocity = tangentialForce; // Set the velocity directly for stable orbit
    }




    private void ApplyGravity(Rigidbody2D targetRigidbody)
    {
        Vector2 directionToCenter = (Vector2)transform.position - targetRigidbody.position;
        float distance = directionToCenter.magnitude;

        // Inverse square law for gravity (can be adjusted if a different behavior is desired)
        float forceMagnitude = gravityStrength / (distance * distance);

        // Apply the gravitational force
        targetRigidbody.AddForce(directionToCenter.normalized * forceMagnitude);
    }



    private void OnDrawGizmos()
    {

    }
}



public enum GravityType
{
    UniformGravityField,
    OrbitalGravity,
    SimpeRotation,
}
