using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

namespace Items
{
    public class BonusSpawner : NetworkBehaviour
    {
        public List<PowerUpBase> powerupPrefabs;
        public PowerUpSapphire sapphire;
        public int threshold;
        private const int MAX_SPAWNPOINT = 3;
        private List<GameObject> spawnPointsList;

        public override void Spawned()
        {
            // Only the server will spawn powerups
            if (!Object.HasStateAuthority)
                return;

            spawnSapphires();
            spawnPowerups();

        }

        void spawnSapphires()
        {
            var spawnPointsPerRoom = GameObject.FindGameObjectsWithTag("SpawningPoints");
            Debug.Log(spawnPointsPerRoom.Length);


            var spawnPoints = GameObject.FindGameObjectsWithTag("SpawningPoint");
            spawnPointsList = spawnPoints.ToList();
            int toSpawnItems = MAX_SPAWNPOINT;

            if (spawnPointsList.Count <= MAX_SPAWNPOINT)
            {
                toSpawnItems = spawnPointsList.Count;
            }

            for (int i = 0; i < toSpawnItems; ++i)
            {
                int currentIndex = Random.Range(0, spawnPointsList.Count);
                GameObject currentSpawnPoint = spawnPointsList[currentIndex];
                Vector3 spawnPosition = currentSpawnPoint.transform.position;
                spawnPosition.y += 1;
                Runner.Spawn(sapphire, spawnPosition, Quaternion.identity, Object.StateAuthority);
                spawnPointsList.RemoveAt(currentIndex);
            }
        }

        void spawnPowerups()
        {

            foreach (GameObject spawnPoint in spawnPointsList)
            {
                float randomNumber = Random.Range(0, 100);
                if (randomNumber >= threshold)
                {
                    Vector3 spawnPosition = spawnPoint.transform.position;
                    spawnPosition.y += 1;

                    int randomIndex = Random.Range(0, powerupPrefabs.Count);

                    Runner.Spawn(powerupPrefabs[randomIndex], spawnPosition, Quaternion.Euler(260, 0, 0),
                        Object.StateAuthority);
                }
            }
        }
    }
}