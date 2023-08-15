using UnityEngine;

public class menuScreenAnim : MonoBehaviour
{
    private Animator anim;
    public float rotationSpeed = 100.0f; // Adjust this value as needed

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for keyboard input to rotate the player
        float rotationInput = Input.GetAxis("Horizontal"); // Assumes "Horizontal" input axis for rotation

        // Calculate rotation amount based on input and time
        float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;

        // Rotate the player around the Y-axis
        transform.Rotate(Vector3.up, rotationAmount);
    }
}
