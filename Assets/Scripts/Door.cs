using UnityEngine;

public class Door : MonoBehaviour, ICollectable
{
    [SerializeField] float value;
    [SerializeField] private Color color;
    [SerializeField] private ManagerObject manager;

    private DoorType type;

    private int positive;
    private float minValue = 1f, maxValue = 10;

    private void Awake()
    {
        positive = value < 0 ? -1 : 1;
        type = Random.Range(0, 3) == 0 ? DoorType.procent : DoorType.regular;
    }

    public void SelfAwake()
    {
        float mul = manager.GetUpgrade(0).GetValue();

        if (type == DoorType.procent) minValue = 0.01f;

        if(positive > 0)
        {
            if(type == DoorType.regular) maxValue *= mul * 1.75f;

            else maxValue = 0.45f;
        }
        else
        {
            if (type == DoorType.regular) maxValue *= mul * 2f;

            else maxValue = 0.65f;

            maxValue = -maxValue;
        }

        value = Random.Range(minValue, maxValue);
    }

    public void Collect(CapitalManager capManager)
    {
        if(type == DoorType.procent)
        {
            value = capManager.Capital * value;
        }

        capManager.ChangeCapital(value, color);
    }

    public float GetInfo(out DoorType type)
    {
        type = this.type;
        return value;
    }
}

public enum DoorType
{
    regular,
    procent,
    mystic
}

/*public struct DoorCapitalInfo
{
    public int positive;
    public float minValue, maxValue;
    public DoorType type;

    public void Initialize(int level)
    {
        if (type == DoorType.procent)
        {
            switch (level)
            {
                case 0:
                    if (positive > 0) SetValues(0.1f, 0.3f);
                    else SetValues(-0.2f, -0.4f);
                    break;
                case 1:
                    if (positive > 0) SetValues(0.2f, 0.15f);
                    else SetValues(-0.5f, -0.35f);
                    break;
                case 2:
                    if (positive > 0) SetValues(0.5f, 0.45f);
                    else SetValues(-0.15f, -0.65f);
                    break;
                case 3:
                    if (positive > 0) SetValues(0.10f, 0.72f);
                    else SetValues(-0.35f, -1f);
                    break;
                case 4:
                    if (positive > 0) SetValues(0.2f, 1f);
                    else SetValues(-0.5f, -2f);
                    break;
            }
        }
        else if (type == DoorType.regular)
        {
            switch (level)
            {
                case 0:
                    if (positive > 0) SetValues(1f, 15f);
                    else SetValues(-1f, -20f);
                    break;
                case 1:
                    if (positive > 0) SetValues(25f, 100f);
                    else SetValues(-30f, -135f);
                    break;
                case 2:
                    if (positive > 0) SetValues(300f, 900f);
                    else SetValues(-500f, -2000f);
                    break;
                case 3:
                    if (positive > 0) SetValues(2000f, 5000f);
                    else SetValues(-2000f, -7000f);
                    break;
                case 4:
                    if (positive > 0) SetValues(10000f, 100000f);
                    else SetValues(-5000f, -200000f);
                    break;
            }
        }
    }

    private void SetValues(float min, float max)
    {
        minValue = min;
        maxValue = max;
    }
}
*/