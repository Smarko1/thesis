using UnityEngine;

public class SpawnerThird : MonoBehaviour
{
    public GameObject prefab1 = null;
    public GameObject prefab2 = null;
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
        
        Transform chosenSpawn = Random.Range(0, 2) == 0 ? spawn1 : spawn2;

        GameObject chosenPrefab = (chosenSpawn == spawn1) ? (Random.Range(0, 2) == 0 ? prefab1 : prefab2) : prefab1;

        /*
        Transform chosenSpawn;
        if (Random.Range(0, 2) == 0)
        {
            chosenSpawn = spawn1;
        }
        else
        {
            chosenSpawn = spawn2;
        }

        GameObject chosenPrefab;
        if (chosenSpawn == spawn1)
        {
            if (Random.Range(0, 2) == 0)
            {
                chosenPrefab = prefab1;
            }
            else
            {
                chosenPrefab = prefab2;
            }
        }
        else
        {
            chosenPrefab = prefab1;
        }
        */

        GameObject go = Instantiate(chosenPrefab, chosenSpawn.position, Quaternion.identity);

        Invoke("SpawnObstacle", Random.Range(minTime, maxTime));
    }
}