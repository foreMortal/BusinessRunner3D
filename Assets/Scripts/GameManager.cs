using YG;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SettingsScriptableObject settings;
    [SerializeField] private ManagerObject obj;
    [SerializeField] private InputAction Press, Pos;

    private float screenWidth;
    private bool active = true;

    public UnityEvent Initialized = new();
    public UnityEvent startEvent = new();

    private void OnEnable()
    {
        obj.millionGavered += GameEnded;
        YandexGame.GetDataEvent += SelfAwake;
        YandexGame.onVisibilityWindowGame += OnVisibilityWindowGame;
        Press.Enable();
        Pos.Enable();
    }

    private void OnDisable()
    {
        obj.millionGavered -= GameEnded;
        YandexGame.GetDataEvent -= SelfAwake;
        YandexGame.onVisibilityWindowGame -= OnVisibilityWindowGame;
        Press.Disable();
        Pos.Disable();
    }

    private void Awake()
    {
        SelfAwake();

        Time.timeScale = 1f;
        screenWidth = Screen.width;
    }

    private void GameEnded()
    {
        active = false;
    }

    private void SelfAwake()
    {
        if (settings.GameLoaded != 1 && YandexGame.SDKEnabled)
        {
            if (YandexGame.EnvironmentData.isDesktop || YandexGame.EnvironmentData.isTablet)
                settings.SetDeviceType(DeviceType.Desktop);
            else if (YandexGame.EnvironmentData.isMobile)
                settings.SetDeviceType(DeviceType.Mobile);

            YandexGame.GameReadyAPI();
            settings.SetGameLoaded();

            Initialized.Invoke();
        }
    }

    private void Update()
    {
        if (active)
        {
            if (Press.ReadValue<float>() > 0)
            {
                float halfW = screenWidth / 2;
                float deltaW = screenWidth / 5;
                float pos = Pos.ReadValue<float>();

                if (pos > halfW - deltaW && pos < halfW + deltaW)
                {
                    startEvent?.Invoke();
                    active = false;

                    YandexGame.GameplayStart();
                    settings.GameState = true;

                    Press.Disable();
                    Pos.Disable();
                }
            }
        }
    }

    private void OnVisibilityWindowGame(bool state)
    {
        if(state && settings.GameState)
            YandexGame.GameplayStart();
        if (!state && settings.GameState)
            YandexGame.GameplayStop();
    }

    public void Active(bool ac)
    {
        active = ac;
    }
}
