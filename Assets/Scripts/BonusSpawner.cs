using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BonusSpawner : MonoBehaviour
{
    public List<PowerUpBase> powerupPrefabs;
    public PowerUpSapphire Sapphir;
    public int threshold;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawningPoint");
        List<GameObject> spawnPointsList = spawnPoints.ToList();

        for(int i = 0; i < 3; ++i){
            int currentIndex = Random.Range(0, spawnPointsList.Count);
            GameObject currentSpawnPoint = spawnPointsList[currentIndex];
            Vector3 spawnPosition = currentSpawnPoint.transform.position;
            spawnPosition.y += 1;
            Instantiate(Sapphir, spawnPosition, Quaternion.identity);
            spawnPointsList.RemoveAt(currentIndex);
        }

        foreach(GameObject spawnPoint in spawnPointsList){
            float randomNumber = Random.Range(0, 100);
            if(randomNumber >= threshold){
                Vector3 spawnPosition = spawnPoint.transform.position;
                spawnPosition.y += 1;
                Instantiate(powerupPrefabs[0], spawnPosition, Quaternion.identity);
            }
        }
    }
}
