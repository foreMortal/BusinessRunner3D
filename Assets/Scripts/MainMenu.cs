using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Text adText;
    
    [Space]
    [SerializeField] private RectTransform mainGoal;
    [SerializeField] private Text moneyText;
    
    [Space]
    [SerializeField] private DataLoader loader;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private Text description, valueNow, newValue, levelNow, newLevel, cost;

    [SerializeField] private ManagerObject manager;

    [Space]
    [SerializeField] private GameObject millionGaveredScreen;

    private Transform mainMenu;
    private GameObject upgrades;
    private StringBuilder sb = new StringBuilder(20);

    private int idx;
    private bool upgradesOpen;
    public bool Active { get; set; }

    public UnityEvent<bool> OpenedUpgrades = new();
    public UnityEvent ClothesBought = new();

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += GiveMoney;
        manager.millionGavered += MillionGavered;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= GiveMoney;
        manager.millionGavered -= MillionGavered;
    }

    private void GiveMoney(int t)
    {
        if(t == 0)
        {
            manager.MoneyEarn += manager.GetUpgrade(0).GetValue() * 50f;
            UpdateMainGoal();
        }
    }

    private void Awake()
    {
        mainMenu = transform.GetChild(1);
        upgrades = mainMenu.GetChild(2).gameObject;
        moneyText = mainMenu.GetChild(0).GetChild(0).GetComponent<Text>();
        
        UpdateUI();
    }

    public static void FormatMoneyString(StringBuilder sb, ref string s)
    {
        bool negative = false;
        sb.Append(s);

        if (s[0] == '-')
        {
            sb.Remove(0, 1);
            negative = true;
        }

        int c = 0;
        for (int i = sb.Length - 1; i > 0; i--)
        {
            c++;
            if (c == 3)
            {
                sb.Insert(i, ".");
                c = 0;
            }
        }
        sb.Append("$");

        if (negative)
        {
            sb.Insert(0, '-');
        }
        s = sb.ToString();
        sb.Clear();
    }

    public void OpenUpgrades()
    {
        OpenedUpgrades.Invoke(upgradesOpen);

        upgrades.SetActive(!upgradesOpen);
        upgradesOpen = !upgradesOpen;

        SetUpgradeInfo(0);
    }

    private void MillionGavered()
    {
        millionGaveredScreen.SetActive(true);
    }

    public void RestartGAme()
    {
        manager.ResetAll();
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        mainMenu.gameObject.SetActive(false);
    }

    public void BuyUpgrade()
    {
        Upgrade up = manager.GetUpgrade(idx);
        float cost = up.GetCost();

        if(manager.MoneyEarn >= cost)
        {
            manager.MoneyEarn -= cost;
            up.Level++;

            if (idx == 0)
                ClothesBought.Invoke();

            UpdateUI();
            loader.SaveData();
        }
    }

    public void SetUpgradeInfo(int Idx)
    {
        Upgrade up = manager.GetUpgrade(Idx);
        idx = Idx;

        description.text = up.GetDescription(YandexGame.lang);

        valueNow.text = up.GetShowValue().ToString();
        levelNow.text = up.Level.ToString();

        if (up.Level + 1 <= up.Max)
        {
            buyButton.SetActive(true);

            newValue.text = up.GetShowValue(1).ToString();
            newLevel.text = (up.Level + 1).ToString();

            string s = up.GetCost().ToString("F0");
            FormatMoneyString(sb, ref s);
            cost.text = s;
        }
        else
        {
            buyButton.SetActive(false);

            newLevel.text = " ";
            newValue.text = " ";
            cost.text = " ";
        }
    }

    public void UpdateMainGoal()
    {
        string s = manager.MoneyEarn.ToString("F0");
        FormatMoneyString(sb, ref s);
        moneyText.text = s;

        string s1 = (manager.GetUpgrade(0).GetValue() * 50f).ToString("F0");
        FormatMoneyString(sb, ref s1);
        adText.text = "+" + s1;

        mainGoal.sizeDelta = new Vector2(mainGoal.sizeDelta.x, 650f * (manager.MoneyEarn / 1000000f));
    }

    public void GetMoneyForAd()
    {
        YandexGame.RewVideoShow(0);
    }

    private void UpdateUI()
    {
        UpdateMainGoal();

        SetUpgradeInfo(idx);
    }
}
