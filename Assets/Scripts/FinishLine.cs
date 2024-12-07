using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Transform finishPoint;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Runner>().Finish(finishPoint.position);

        anim.Play("GatesOpen");
    }
}
