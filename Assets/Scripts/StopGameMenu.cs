using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class StopGameMenu : MonoBehaviour
{
    [SerializeField] private SettingsScriptableObject settings;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private Text[] texts;
    [SerializeField] private Runner runner;
    [SerializeField] private CapitalManager capitalManager;
    private bool active, leaveActive;
    private StringBuilder sb;

    private void Awake()
    {
        sb = new StringBuilder(20);
    }

    public void SetMenuActive()
    {
        if (active)
        {
            YandexGame.GameplayStop();
            settings.GameState = false;
        }
        else
        {
            YandexGame.GameplayStart();
            settings.GameState = true;
        }

        objects[0].SetActive(!active);
        objects[1].SetActive(active);

        string money = capitalManager.Capital.ToString("F0");
        MainMenu.FormatMoneyString(sb, ref money);
        texts[0].text = money;
        texts[1].text = runner.Distance.ToString("F0") + "ì.";

        leaveActive = active;
        active = !active;

        if (active) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

    public void SetLeaveMenuActive()
    {
        objects[1].SetActive(!leaveActive);
        leaveActive = !leaveActive;
    }

    public void QuitGame()
    {
        capitalManager.QuitGame();
        SceneManager.LoadScene(0);
    }
}
