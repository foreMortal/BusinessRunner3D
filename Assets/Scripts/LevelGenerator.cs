using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject island, sand;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ManagerObject manager;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private Vector3 offset = new Vector3(-0.263f, 0f, -0.101f);

    private GameObject origin;
    private Transform pos;
    private Vector3[] offsets = new Vector3[2] { new Vector3(0f, 0f, 2.25f), new Vector3(0f, 0f, 2.25f) };
    private Bounds bounds;

    private float[] angles = new float[2] { 90f, -90f};
    private int[] directions;
    private int length;

    private void Awake()
    {
        length = 3;
        directions = new int[length];

        origin = Instantiate(objects[0]);
        SequenceManager s = origin.GetComponent<SequenceManager>();

        origin.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        pos = origin.transform;
        bounds = origin.GetComponent<Collider>().bounds;

        s.SelfAwake(gameManager);

        for(int i = 0; i < length; i++)
        {
            ChoosePath(i);
            int dir = directions[i];

            int idx = dir == -1 ? 1 : 0;

            GameObject j = Instantiate(objects[idx], pos);
            SequenceManager js = j.GetComponent<SequenceManager>();

            j.transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, bounds.size.z) + offsets[idx], Quaternion.Euler(0f, angles[idx], 0f));
            pos = j.transform;

            js.SelfAwake(gameManager);
        }
    }

    public void SelfAwake()
    {
        length = (int)manager.GetUpgrade(2).GetValue();
        directions = new int[length];

        for (int i = 0; i < length; i++)
        {
            ChoosePath(i);

            int dir = directions[i];

            if (Physics.Raycast(pos.position + pos.forward * 48f, pos.right * dir, 55f, 1 << 8)) 
                dir = -dir;

            int idx = dir == -1 ? 1 : 0;

            GameObject j = Instantiate(objects[idx], pos);
            SequenceManager js = j.GetComponent<SequenceManager>();

            j.transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, bounds.size.z) + offsets[idx], Quaternion.Euler(0f, angles[idx], 0f));
            pos = j.transform;

            js.SelfAwake();
        }

        GameObject g = Instantiate(island, pos);
        g.transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, bounds.size.z + 10), Quaternion.identity);
        GameObject s = Instantiate(sand, pos);
        s.transform.SetLocalPositionAndRotation(new Vector3(0f, -0.5f, bounds.size.z + 20f), Quaternion.identity);
        s.transform.parent = null;
        s.transform.rotation = Quaternion.identity;
    }

    private void ChoosePath(int i)
    {
        int idx = 0;
        if(i > 1)
        {
            for (int j = 1; j < i; j++)
            {
                idx += directions[i - j];
            }

            if (idx > 0)
                directions[i] = -1;
            else if (idx < 0)
                directions[i] = 1;
            else
            {
                int t = Random.Range(0, 2);
                if (t == 0) t = -1;
                directions[i] = t;
            }
        }
        else
        {
            int t = Random.Range(0, 2);
            if (t == 0) t = -1;
            directions[i] = t;
        }
    }
}
