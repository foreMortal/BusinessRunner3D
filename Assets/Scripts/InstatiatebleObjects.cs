using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InstatiatebleScriptableObject", order = 1)]
public class InstatiatebleObjects : ScriptableObject
{
    [SerializeField] private GameObject[] dollars, doors;

    public GameObject GetRandDoolar()
    {
        return dollars[Random.Range(0, dollars.Length)];
    }
    public GameObject GetRandPassThrough()
    {
        return doors[Random.Range(0, doors.Length)];
    }
}
