public class Upgrade
{
    private int level;
    private int max = 4;
    
    public int Level { get { return level; } set { if (value <= max) level = value; } }
    public int Max { get { return max; } }

    private float[] values, cost;
    private string[] showValues;
    private string[] description;

    public void SetLevel(int level)
    {
        this.level = level;
    }
    public string GetShowValue(int delta=0)
    {
        return showValues[level + delta];
    }
    public string GetDescription(string lang)
    {
        switch (lang)
        {
            case "ru": return description[0];
            case "en": return description[1]; 
            case "tr": return description[2];
            case "es": return description[3]; 
            case "de": return description[4];
            default: return description[0];
        }
    }
    public float GetValue(int delta=0)
    {
        return values[level + delta];
    }
    public float GetCost(int delta=0)
    {
        return cost[level + delta];
    }

    public Upgrade() { }

    public Upgrade(float[] values, float[] cost, string[] showValues, params string[] description)
    {
        this.values = values;
        this.cost = cost;
        this.description = description;
        this.showValues = showValues;
    }
}