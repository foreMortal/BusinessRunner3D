using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PassThrough : MonoBehaviour, ICollectable
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private Door[] doors;
    [SerializeField] private Text[] texts;

    private bool used, instatiated;

    private void Start()
    {
        if (!instatiated)
        {
            foreach (var t in texts)
                t.text = "???";
        }
    }

    public void SelfAwake()
    {
        foreach(var d in doors)
        {
            d.SelfAwake();
        }
        for (int i = 0; i < 2; i++)
        {
            DoorType type;
            float val = doors[i].GetInfo(out type);

            StringBuilder sb = new StringBuilder(20);

            int myl = type == DoorType.procent ? 100 : 1;

            string s = (val * myl).ToString("F0");
            MainMenu.FormatMoneyString(sb, ref s);

            sb.AppendFormat(s);
            sb.Replace("$", "");

            if (type == DoorType.procent) sb.Append("%");
            else sb.Append("$");

            if (val > 0)
                sb.Insert(0, "+");

            texts[i].text = sb.ToString();
        }

        instatiated = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!used)
        {
            used = true;
            CapitalManager c = other.GetComponent<CapitalManager>();

            float left = (c.transform.position - doors[0].transform.position).magnitude;
            float right = (c.transform.position - doors[1].transform.position).magnitude;

            if (left < right) doors[0].Collect(c);
            else doors[1].Collect(c);

            c.Source.clip = clip;
            c.Source.Play();
        }
    }
}
