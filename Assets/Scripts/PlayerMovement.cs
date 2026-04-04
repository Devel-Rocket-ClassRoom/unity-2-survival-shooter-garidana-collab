using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static readonly int HashMove = Animator.StringToHash("Move");

    public float moveSpeed = 10f;
    private PlayerInput playerInput;
    private Rigidbody playerRigidBody;
    private AudioSource playerAudioSource;
    private Animator playerAnimator;



    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidBody = GetComponent<Rigidbody>();
        playerAudioSource = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = playerInput.MoveX != 0 || playerInput.MoveY != 0;
        playerAnimator.SetBool (HashMove, isMoving);
    }

    private void FixedUpdate()
    {
        Vector3 delta = new Vector3(playerInput.MoveX, 0f, playerInput.MoveY).normalized;
        playerRigidBody.MovePosition(playerRigidBody.position + delta * moveSpeed * Time.deltaTime);
    }
}
