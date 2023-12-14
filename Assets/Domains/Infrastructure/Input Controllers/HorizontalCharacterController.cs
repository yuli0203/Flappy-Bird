// Basic movement controller
using Logging;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class HorizontalCharacterController : TickableSubscriber
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float limitMovement = 1.5f;
    [SerializeField] bool controllable = true;

    Transform movingObject;

    [Inject]
    public void Construct(CharacterViewModel characterViewModel)
    {
        this.movingObject = characterViewModel.transform;
    }

    public void SetControllable(bool enabled)
    {
        controllable = enabled;
    }

    protected override void MakeTick()
    {
        if (movingObject == null || !controllable)
        {
            return;
        }

        HandleKeyboardInput();
        HandleSwipeInput();
    }

    void HandleKeyboardInput()
    {
        // Keyboard Controls
        float horizontalInput = Input.GetAxis("Horizontal");
        MoveCharacter(horizontalInput);
    }

    void HandleSwipeInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Detect swipe direction
            if (touch.phase == TouchPhase.Moved)
            {
                float swipeDelta = touch.deltaPosition.x;

                if (Mathf.Abs(swipeDelta) > 10f) // Adjust the sensitivity of the swipe
                {
                    // Swipe to the right
                    if (swipeDelta > 0)
                    {
                        MoveCharacter(1f);
                    }
                    // Swipe to the left
                    else
                    {
                        MoveCharacter(-1f);
                    }
                }
            }
        }
    }

    void MoveCharacter(float horizontalInput)
    {
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f);
        Vector3 newPosition = movingObject.position + movement * moveSpeed * Time.deltaTime;

        // Limit movement within the specified range
        newPosition.x = Mathf.Clamp(newPosition.x, -limitMovement, limitMovement);

        movingObject.position = newPosition;
    }
}
