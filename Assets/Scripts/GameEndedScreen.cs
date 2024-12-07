using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndedScreen : MonoBehaviour
{
    [SerializeField] private Text endName, distanceValue, moneyEarnValue, buttonText;
    [SerializeField] private GameObject menu;

    public void OpenEndGameMenu(ref GameEndInfo info)
    {
        menu.SetActive(true);
        endName.text = info.endType;
        distanceValue.text = info.distance.ToString("F0");

        string s = info.moneyEarn.ToString("F0");
        StringBuilder sb = new StringBuilder(20);
        MainMenu.FormatMoneyString(sb, ref s);
        moneyEarnValue.text = s;

        buttonText.text = info.buttonText;
    }

    public void Restartlevel()
    {
        SceneManager.LoadScene(0);
    }
}
