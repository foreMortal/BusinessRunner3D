using System;
using UnityEngine;

public class CapitalManager : MonoBehaviour
{
    [SerializeField] private DataLoader loader;
    [SerializeField] private AudioSource source;
    [SerializeField] private ManagerObject manager;
    [SerializeField] private Vector3 offset, extents;
    private float capital, max = 30;
    private Collider[] results = new Collider[2];
    private LayerMask mask = 1 << 7;
    private bool active = true;
    public event Action<CollectablesInfo> onCapitalChange;

    public float Capital { get { return capital; } }
    public AudioSource Source { get { return source; } }

    private void Awake()
    {
        transform.GetChild(manager.GetUpgrade(0).Level).gameObject.SetActive(true);
    }

    private void Start()
    {
        GetComponent<Runner>().gameEnd.AddListener(EndGame);
    }

    public void SelfAwake()
    {
        max = manager.GetUpgrade(3).GetValue();
        Color c = new(0f, 0f, 0f, 0f);
        CollectablesInfo collect = new() { change = 0, current = capital, max = max, color = c };

        onCapitalChange?.Invoke(collect);
    }

    private void FixedUpdate()
    {
        if (active)
        {
            int t = Physics.OverlapCapsuleNonAlloc(transform.position, transform.position + new Vector3(0f, 2f, 0f), 0.3f, results, mask);

            if (t > 0)
            {
                for (int i = 0; i < t; i++)
                {
                    Color color;
                    float change = results[i].GetComponent<Collectables>().Collect(out color, out AudioClip clip);
                    source.clip = clip;
                    source.Play();
                    ChangeCapital(change, color);
                    Destroy(results[i].gameObject);
                }
            }
        }
    }

    public void EndGame()
    {
        manager.MoneyEarn += capital;
        active = false;

        loader.SaveData();
    }

    public void QuitGame()
    {
        manager.MoneyEarn += capital * 0.5f;

        loader.SaveData();
    }

    public void ChangeCapital(float change, Color color)
    {
        if (capital + change > max)
        {
            change = max - capital;
            capital = max;
        }
        else if(capital + change < 0)
        {
            change = 0 - capital;
            capital = 0;
        }
        else capital += change;

        CollectablesInfo collect = new() { change = change, current = capital, max = max, color = color};
        onCapitalChange?.Invoke(collect);
    }

    public void ChangeClothes()
    {
        for(int i = 0; i < 5; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        transform.GetChild(manager.GetUpgrade(0).Level).gameObject.SetActive(true);
    }
}

public struct CollectablesInfo
{
    public float change, current, max;
    public Color color;
}