using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ManagerScriptableObject", order = 0)]
public class ManagerObject : ScriptableObject
{
    public Action millionGavered;

    private float money;
    public float MoneyEarn { get { return money; } set { money = value; if (money >= 1000000) millionGavered?.Invoke(); } } 

    private Upgrade[] upgrades = new Upgrade[]
    {
        new Upgrade(new float[]{1f, 50f, 150f, 300f, 500f }, new float[]{150f, 7000f, 50000f, 250000f}, new string[] { "1x", "50x", "150x", "300x", "500x" },
            "Увеличивает получение денег, но и потерю", "Increases money received, but also loss", "Alınan para artar ama aynı zamanda kayıp da artar", "Aumenta el dinero recibido, pero también las pérdidas.", "Erhöht erhaltenes Geld, aber auch Verlust"),//collectablesStrength
        new Upgrade(new float[]{100f, 200f, 300f, 400f, 600f }, new float[]{100f, 2000f, 50000f, 90000f}, new string[] { "100", "200", "300", "400", "600" },
            "Позволяет пробегать большее расстояние", "Allows you to run longer distances", "Daha uzun mesafeler koşmanızı sağlar", "Te permite correr distancias más largas.", "Ermöglicht das Laufen längerer Strecken"),//stamina
        new Upgrade(new float[]{0f, 2f, 4f, 6f, 8f }, new float[]{5000f, 2000f, 50000f, 120000f}, new string[] { "4", "6", "8", "10", "12" },
            "Увеличивает длину уровней", "Increases the length of levels", "Seviyelerin uzunluğunu artırır", "Aumenta la duración de los niveles.", "Erhöht die Länge von Levels"),//highHeels
        new Upgrade(new float[]{60f, 700f, 8000f, 30000f, 100000f }, new float[]{200f, 3500f, 25000f, 100000f}, new string[] { "60$", "700$", "8.000$", "30.000$", "100.000$" },
            "Увеличивает максимальное количество денег в кармане", "Increases the maximum amount of money in your pocket", "Cebinizdeki maksimum para miktarını artırır", "Aumenta la cantidad máxima de dinero en tu bolsillo", "Erhöht den maximalen Geldbetrag in Ihrer Tasche"),//pocketSize
    };


    public Upgrade GetUpgrade(int index)
    {
        return upgrades[index];
    }

    public void IncreaseParameter(int index)
    {
        upgrades[index].Level++;
    }

    public void ResetAll()
    {
        money = 0;

        upgrades = new Upgrade[]
        {
        new Upgrade(new float[]{1f, 50f, 150f, 300f, 500f }, new float[]{150f, 7000f, 50000f, 250000f}, new string[] { "1x", "50x", "150x", "300x", "500x" },
            "Увеличивает получение денег, но и потерю", "Increases money received, but also loss", "Alınan para artar ama aynı zamanda kayıp da artar", "Aumenta el dinero recibido, pero también las pérdidas.", "Erhöht erhaltenes Geld, aber auch Verlust"),//collectablesStrength
        new Upgrade(new float[]{100f, 200f, 300f, 400f, 600f }, new float[]{100f, 2000f, 50000f, 90000f}, new string[] { "100 м.", "200 м.", "300 м.", "400 м.", "600 м." },
            "Позволяет пробегать большее расстояние", "Allows you to run longer distances", "Daha uzun mesafeler koşmanızı sağlar", "Te permite correr distancias más largas.", "Ermöglicht das Laufen längerer Strecken"),//stamina
        new Upgrade(new float[]{0f, 2f, 4f, 6f, 8f }, new float[]{5000f, 2000f, 50000f, 120000f}, new string[] { "4 п.", "6 п.", "8 п.", "10 п.", "12 п." },
            "Увеличивает длину уровней", "Increases the length of levels", "Seviyelerin uzunluğunu artırır", "Aumenta la duración de los niveles.", "Erhöht die Länge von Levels"),//highHeels
        new Upgrade(new float[]{60f, 700f, 8000f, 30000f, 100000f }, new float[]{200f, 3500f, 25000f, 100000f}, new string[] { "60$", "700$", "8.000$", "30.000$", "100.000$" },
            "Увеличивает максимальное количество денег в кармане", "Increases the maximum amount of money in your pocket", "Cebinizdeki maksimum para miktarını artırır", "Aumenta la cantidad máxima de dinero en tu bolsillo", "Erhöht den maximalen Geldbetrag in Ihrer Tasche"),//pocketSize
        };
    }
}
