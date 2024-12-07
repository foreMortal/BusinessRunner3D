using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    [SerializeField] private InstatiatebleObjects objects;
    private Vector3 firstPos = new Vector3(0f, 1f, 12.6f), offset = new Vector3(0f, 0f, 15f);
    
    public void SelfAwake(GameManager gm)
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject f;
            int rand = Random.Range(0, 3);
            if (rand == 0) 
            { 
                f = Instantiate(objects.GetRandPassThrough(), transform);

                f.transform.SetLocalPositionAndRotation(firstPos + offset * i, Quaternion.identity);
            }
            else 
            {
                f = Instantiate(objects.GetRandDoolar(), transform);

                if(Random.Range(0, 2) == 0) 
                    f.transform.SetLocalPositionAndRotation(firstPos + offset * i, Quaternion.Euler(0f, 180f, 0f));
                else
                    f.transform.SetLocalPositionAndRotation(firstPos + offset * i, Quaternion.identity);
            }

            gm.startEvent.AddListener(f.GetComponent<ICollectable>().SelfAwake);
        }
    }

    public void SelfAwake()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject f;
            int rand = Random.Range(0, 3);
            if (rand == 0) { f = Instantiate(objects.GetRandPassThrough(), transform); }

            else { f = Instantiate(objects.GetRandDoolar(), transform); }

            f.transform.SetLocalPositionAndRotation(firstPos + offset * i, Quaternion.identity);

            f.GetComponent<ICollectable>().SelfAwake();
        }
    }
}
