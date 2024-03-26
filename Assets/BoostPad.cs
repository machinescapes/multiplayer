using UnityEngine;

public class BoostPad : MonoBehaviour
{
    public float boostForce = 10f; // Adjust the force as needed

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure only the player gets boosted
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Apply the boost force vertically
                playerRb.AddForce(Vector3.up * boostForce, ForceMode.Impulse);
            }
        }
    }
}
