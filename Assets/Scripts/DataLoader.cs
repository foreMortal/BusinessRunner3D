using UnityEngine;
using UnityEngine.Events;
using YG;

public class DataLoader : MonoBehaviour
{
    [SerializeField] private ManagerObject manager;

    public UnityEvent dataLoaded = new();

    private void Awake()
    {
        LoadData();
    }

    public void LoadData()
    {
        manager.MoneyEarn = YandexGame.savesData.capital;
        manager.GetUpgrade(0).SetLevel(YandexGame.savesData.collectablesStrength);
        manager.GetUpgrade(1).SetLevel(YandexGame.savesData.stamina);
        manager.GetUpgrade(2).SetLevel(YandexGame.savesData.hihgHeels);
        manager.GetUpgrade(3).SetLevel(YandexGame.savesData.pocketSize);

        dataLoaded.Invoke();
    }

    public void SaveData()
    {
        YandexGame.savesData.capital = manager.MoneyEarn;

        YandexGame.savesData.collectablesStrength = manager.GetUpgrade(0).Level;
        YandexGame.savesData.stamina = manager.GetUpgrade(1).Level;
        YandexGame.savesData.hihgHeels = manager.GetUpgrade(2).Level;
        YandexGame.savesData.pocketSize = manager.GetUpgrade(3).Level;

        YandexGame.SaveProgress();
    }
}
