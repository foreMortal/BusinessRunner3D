using UnityEngine;

public class DollarSequence : MonoBehaviour, ICollectable
{
    [SerializeField] private ManagerObject manager;
    private Collectables[] collectables;

    private void Awake()
    {
        collectables = GetComponentsInChildren<Collectables>();
    }

    public void SelfAwake()
    {
        float level = manager.GetUpgrade(0).GetValue();

        foreach (var c in collectables)
        {
            c.CapitalChange = Initialize(c.CapitalChange, level);
        }
    }

    private float Initialize(float val, float level)
    {
        return level * val;
    }
}
