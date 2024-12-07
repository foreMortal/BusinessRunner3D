using UnityEngine;
using UnityEngine.UI;
using YG;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private SettingsScriptableObject settings;
    [SerializeField] private Runner runner;
    [SerializeField] private GameObject[] canvases;
    [SerializeField] private GameManager manager;

    private void Awake()
    {
        SetCanvases();
    }

    public void SetCanvases()
    {
        int i = -1;

        if (settings.DeviceType == DeviceType.Desktop)
        {
            i = 0;
        }
        else if (settings.DeviceType == DeviceType.Mobile)
        {
            i = 1;
        }

        if (i != -1)
        {
            canvases[i].SetActive(true);

            CapitalBar b = canvases[i].GetComponentInChildren<CapitalBar>(true);
            manager.startEvent.AddListener(canvases[i].GetComponentInChildren<MainMenu>().StartGame);
            manager.startEvent.AddListener(b.StartGame);

            runner.GameEndedScreen = canvases[i].GetComponent<GameEndedScreen>();
            runner.CapitalBar = b;

            b.SelfAwake();
        }
    }
}
