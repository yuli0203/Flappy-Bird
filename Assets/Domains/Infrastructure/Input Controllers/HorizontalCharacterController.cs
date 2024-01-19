// Basic movement controller
using Logging;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class HorizontalCharacterController : TickableSubscriber
{
    [SerializeField] float jumpForce = 2f;
    [SerializeField] bool controllable = true;

    Transform movingObject;
    private Rigidbody rigidBody;

    [Inject]
    public void Construct(CharacterViewModel characterViewModel)
    {
        this.movingObject = characterViewModel.transform;
        this.rigidBody = movingObject.GetComponent<Rigidbody>();
    }

    public void SetControllable(bool enabled)
    {
        controllable = enabled;
        if (enabled)
        {
            rigidBody.useGravity = true;
        }
    }

    protected override void MakeTick()
    {
        if (movingObject == null || !controllable)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))  // Change to Input.GetTouch(0).phase == TouchPhase.Began for mobile
        {
            Jump();
        }

        void Jump()
        {
            // Add an initial upward force to the Rigidbody
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f); // Reset vertical velocity before applying force
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }

}
