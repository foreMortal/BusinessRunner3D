using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapitalBar : MonoBehaviour
{
    [SerializeField] private GameObject hud;
    [SerializeField] private CapitalManager manager;
    [SerializeField] private Text prefab;
    [SerializeField] private RectTransform stamina;

    private List<ShowTextObj> ActiveTexts = new List<ShowTextObj>();
    private List<ShowTextObj> HidedTexts= new List<ShowTextObj>();
    private List<ShowTextObj> buffer= new List<ShowTextObj>();

    private RectTransform bar;
    private Text moneyText;
    private float maxLength, staminaMaxLength;

    public void SelfAwake()
    {
        manager.onCapitalChange += RefreshBar;
        moneyText = transform.GetChild(2).GetComponent<Text>();
        bar = transform.GetChild(1).GetComponent<RectTransform>();
        stamina = transform.GetChild(4).GetComponent<RectTransform>();

        maxLength = bar.sizeDelta.x;
        staminaMaxLength = stamina.sizeDelta.y;
    }

    public void Update()
    {
        for(int i = 0; i < ActiveTexts.Count; i++)
        {
            ShowTextObj c = ActiveTexts[i];

            c.lifeTime += Time.deltaTime * 0.33f;
            c.text.transform.localPosition = Vector3.Lerp(new Vector3(0f, -180f), new Vector3(0f, 20f), c.lifeTime);
            c.text.color = Color.Lerp(c.startColor, new Color(c.text.color.r, c.text.color.g, c.text.color.b, 0f), c.lifeTime);
            ActiveTexts[i] = c;

            if (c.lifeTime >= 1f)
                buffer.Add(ActiveTexts[i]);
        }

        if(buffer.Count > 0)
        {
            foreach(var i in buffer)
            {
                ActiveTexts.Remove(i);
                HidedTexts.Add(i);

                i.text.gameObject.SetActive(false);
            }

            buffer.Clear();
        }
    }

    public void RefreshStamina(float current, float max)
    {
        float y = current / max * staminaMaxLength;

        stamina.sizeDelta = new Vector2(stamina.sizeDelta.x, y);
    }

    public void RefreshBar(CollectablesInfo collect)
    {
        float x = collect.current / collect.max * maxLength;

        moneyText.text = collect.current.ToString("F0") + "/" + collect.max.ToString("F0") + "$";
        bar.sizeDelta = new Vector2 (x, bar.sizeDelta.y);

        ShowText(collect.change, collect.color);
    }

    private void ShowText(float change, Color color)
    {
        ShowTextObj newObj = new ShowTextObj();

        if (HidedTexts.Count <= 0)
            newObj.text = Instantiate(prefab, transform);
        else
        {
            newObj = HidedTexts[0];
            HidedTexts.RemoveAt(0);
        }

        newObj.text.gameObject.SetActive(true);
        newObj.text.text = change.ToString("F1") + "$";
        newObj.startColor = newObj.text.color = color;
        newObj.text.rectTransform.localPosition = new Vector2(0f, -180f);
        newObj.lifeTime = 0f;

        ActiveTexts.Add(newObj);
        CorrectText();
    }

    private void CorrectText()
    {
        if(ActiveTexts.Count > 1)
        {
            if (ActiveTexts[^1].lifeTime - ActiveTexts[^2].lifeTime < 0.25f)
            {
                for(int i = 0; i < ActiveTexts.Count - 1; i++)
                {
                    ShowTextObj c = ActiveTexts[i];
                    c.lifeTime += 0.15f;
                    ActiveTexts[i] = c;
                }
            }
        }
    }

    public void StartGame()
    {
        hud.SetActive(true);
    }
}

struct ShowTextObj
{
    public Text text;
    public float lifeTime;
    public Color startColor;
}
