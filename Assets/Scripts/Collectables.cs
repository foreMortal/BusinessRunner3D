using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private float capitalChange;
    [SerializeField] private Color color;

    public float CapitalChange { get { return capitalChange; } set { capitalChange = value; } }

    public float Collect(out Color color, out AudioClip clip)
    {
        clip = this.clip;
        color = this.color;
        return capitalChange;
    }
}
