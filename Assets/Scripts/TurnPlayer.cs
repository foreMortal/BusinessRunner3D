using UnityEngine;

public class TurnPlayer : MonoBehaviour
{
    private Vector3 newDir, newRight;
    private bool active = true;

    private void Start()
    {
        newDir = transform.parent.forward;
        newRight = transform.parent.right;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            other.GetComponent<Runner>().ChangeDirection(newDir, newRight);
            other.GetComponent<PlayerMovement>().ChangePlatform(transform.parent);
            active = false;
        }
    }
}
