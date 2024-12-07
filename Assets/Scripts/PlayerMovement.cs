using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputAction pointerPos, pointerDown;
    [SerializeField] private Transform charRot;
    [SerializeField] private float speed;

    private Transform platform;
    private Quaternion nextRot, currRot;
    private MovementType type;

    private float timer, t;
    private bool canMove = true;
    public bool Active { get; set; }

    private void Awake()
    {
        Runner r = GetComponent<Runner>();
        r.changeDirection += ChangeDir;
        r.gameEnd.AddListener(EndGame);
    }

    private void OnEnable()
    {
        pointerPos.Enable();
        pointerDown.Enable();
    }

    private void OnDisable()
    {
        pointerPos.Disable();
        pointerDown.Disable();
    }

    private void Update()
    {
        if (Active)
        {
            if (canMove)
            {
                if (pointerDown.ReadValue<float>() > 0)
                {
                    float input = pointerPos.ReadValue<float>();

                    transform.localPosition += new Vector3(input * speed * Time.deltaTime, 0f, 0f);

                    if (transform.localPosition.x > 2.3f)
                        transform.localPosition = new Vector3(2.3f, transform.localPosition.y, transform.localPosition.z);
                    else if (transform.localPosition.x < -2.3f)
                        transform.localPosition = new Vector3(-2.3f, transform.localPosition.y, transform.localPosition.z);

                    if (input < 0 && type != MovementType.Left)
                    {
                        type = MovementType.Left;
                        nextRot = Quaternion.Euler(0f, -45f, 0f);
                        currRot = charRot.localRotation;
                        t = 0;
                    }
                    else if(input > 0 && type != MovementType.Right)
                    {
                        type = MovementType.Right;
                        nextRot = Quaternion.Euler(0f, 45f, 0f);
                        currRot = charRot.localRotation;
                        t = 0;
                    }
                    else if(input == 0 && type != MovementType.None)
                    {
                        type = MovementType.None;
                        nextRot = Quaternion.Euler(0f, 0f, 0f);
                        currRot = charRot.localRotation;
                        t = 0;
                    }
                }
                else
                {
                    if(type != MovementType.None)
                    {
                        type = MovementType.None;
                        nextRot = Quaternion.Euler(0f, 0f, 0f);
                        currRot = charRot.localRotation;
                        t = 0;
                    }
                }
            }

            if (!canMove)
            {
                timer += Time.deltaTime;
                if(timer >= 0.7f)
                {
                    canMove = true;
                    timer = 0f;
                }
            }
        }
        if (t < 1)
        {
            t += Time.deltaTime * 5f;
            charRot.localRotation = Quaternion.Slerp(currRot, nextRot, t);
        }
    }

    public void EndGame()
    {
        type = MovementType.None;
        nextRot = Quaternion.Euler(0f, 0f, 0f);
        currRot = charRot.localRotation;
        t = 0;
        Active = false;
    }

    public void StartGame()
    {
        Active = true;
        canMove = true;

        if (Physics.Raycast(transform.position + Vector3.up * 3f, -transform.up, out RaycastHit hit, 5f, 1 << 8))
        {
            platform = hit.transform;
            transform.parent = platform;
        }
    }

    public void ChangePlatform(Transform platform)
    {
        this.platform = platform;
        transform.parent = platform;
    }

    private void ChangeDir(Vector3 _, Vector3 __)
    {
        canMove = false;

        type = MovementType.None;
        nextRot = Quaternion.Euler(0f, 0f, 0f);
        currRot = charRot.localRotation;
        t = 0;
    }
}

public enum MovementType
{
    None,
    Left,
    Right,
}
