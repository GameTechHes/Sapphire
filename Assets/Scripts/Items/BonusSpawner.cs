using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Items
{
    public class BonusSpawner : MonoBehaviour
    {
        public List<PowerUpBase> powerupPrefabs;
        public PowerUpSapphire sapphire;
        public int threshold;
        private const int MAX_SPAWNPOINT = 3;
        void Start()
        {
            var spawnPoints = GameObject.FindGameObjectsWithTag("SpawningPoint");
            var spawnPointsList = spawnPoints.ToList();
            int toSpawnItems = MAX_SPAWNPOINT;
            if (spawnPointsList.Count <= MAX_SPAWNPOINT){
                toSpawnItems = spawnPointsList.Count;
            }
            for (int i = 0; i < toSpawnItems; ++i)
            {
                int currentIndex = Random.Range(0, spawnPointsList.Count);
                GameObject currentSpawnPoint = spawnPointsList[currentIndex];
                Vector3 spawnPosition = currentSpawnPoint.transform.position;
                spawnPosition.y += 1;
                Instantiate(sapphire, spawnPosition, Quaternion.identity);
                spawnPointsList.RemoveAt(currentIndex);
            }

            foreach (GameObject spawnPoint in spawnPointsList)
            {
                float randomNumber = Random.Range(0, 100);
                if (randomNumber >= threshold)
                {
                    Vector3 spawnPosition = spawnPoint.transform.position;
                    spawnPosition.y += 1;

                    int randomIndex = Random.Range(0, powerupPrefabs.Count);

                    var obj = Instantiate(powerupPrefabs[randomIndex], spawnPosition, Quaternion.Euler(260, 0, 0));
                }
            }
        }
    }
}