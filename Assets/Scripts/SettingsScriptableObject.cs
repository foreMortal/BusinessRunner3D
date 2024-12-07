using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SettingsScriptableObject", order = 3)]
public class SettingsScriptableObject : ScriptableObject
{
    private DeviceType deviceType;
    private int gameLoaded;

    public DeviceType DeviceType { get { return deviceType; } }
    public int GameLoaded { get { return gameLoaded; } }
    public bool GameState { get; set; }

    public void SetGameLoaded()
    {
        gameLoaded = 1;
    }

    public void SetDeviceType(DeviceType type)
    {
        deviceType = type;
    }
}

public enum DeviceType
{
    None,
    Desktop,
    Mobile,
}