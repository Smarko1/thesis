using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab = null;
    public Transform spawn1 = null;
    public Transform spawn2 = null;
    public float minTime = 1.0f;
    public float maxTime = 3.0f;

    private void Start()
    {
        Invoke("SpawnObstacle", Random.Range(minTime, maxTime));
    }

    private void SpawnObstacle()
    {
        /* Transform chosenSpawn = Random.Range(0, 2) == 0 ? spawn1 : spawn2; */
        Transform chosenSpawn;
        if (Random.Range(0, 2) == 0)
        {
            chosenSpawn = spawn1;
        }
        else
        {
            chosenSpawn = spawn2;
        }

        GameObject go = Instantiate(prefab, chosenSpawn.position, Quaternion.identity);

        Invoke("SpawnObstacle", Random.Range(minTime, maxTime));
    }
}