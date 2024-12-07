using UnityEngine;

public class CameraFollows : MonoBehaviour
{
    [SerializeField] private Transform player, water;


    private Vector3 direction, distDirection;
    private Quaternion distRotation;
    private float t = 2;

    private void Awake()
    {
        direction = player.position - transform.position;
        direction.y = 0f;
        direction.Normalize();

        player.GetComponent<Runner>().changeDirection += ChangeDirection;
    }

    private void LateUpdate()
    {
        if (t < 1)
        {
            t += Time.deltaTime * 0.35f;
            direction = Vector3.Lerp(direction, distDirection, t);
            transform.rotation = Quaternion.Slerp(transform.rotation, distRotation, t);
        }

        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z) - direction * 2f;
        //water.position = new Vector3(transform.position.x, water.position.y, transform.position.z);
    }

    private void ChangeDirection(Vector3 d, Vector3 _)
    {
        distDirection = d;
        distRotation = Quaternion.LookRotation(new Vector3(d.x, transform.forward.y, d.z), Vector3.up);

        t = 0;
    }
}
