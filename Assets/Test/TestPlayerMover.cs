using CCLBStudio.Inputs;
using UnityEngine;

public class TestPlayerMover : MonoBehaviour
{
    private static readonly int InputX = Animator.StringToHash("InputX");
    private static readonly int InputY = Animator.StringToHash("InputY");
    private static readonly int InputMagnitude = Animator.StringToHash("InputMagnitude");
    public InputReader inputReader;
    public Animator animator;
    public new Rigidbody rigidbody;
    public float speed = 5f;
    
    private Vector3 _moveVector;

    private void Awake()
    {
        inputReader.HardReset();
        inputReader.Init();
    }

    void Start()
    {
        inputReader.MoveEvent += Move;
        inputReader.AimEvent += Aim;
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + _moveVector * (speed * _moveVector.magnitude * Time.fixedDeltaTime));
        
        Vector3 localMove = transform.InverseTransformDirection(_moveVector);

        animator.SetFloat(InputX, localMove.x);
        animator.SetFloat(InputY, localMove.z);
        animator.SetFloat(InputMagnitude, localMove.magnitude);
    }

    private void Move(Vector2 input)
    {
        _moveVector = new Vector3(input.x, 0f, input.y);
    }

    private void Aim(Vector2 input)
    {
        Vector3 direction = new Vector3(input.x, 0f, input.y);
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        rigidbody.MoveRotation(targetRotation);
    }
}
