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
            SpawnAll();
        }

        public void SpawnAll()
        {
            // Only the server will spawn powerups
            if (!Object.HasStateAuthority)
                return;

            SpawnSapphires();
            SpawnPowerups();
        }

        void SpawnSapphires()
        {
            int spawnedSapphire = 0;
            // Each element contains all the spawning points in its own room.
            var spawnPointsPerRoom = GameObject.FindGameObjectsWithTag("SpawningPoints");
            foreach (GameObject spawnPointRoom in spawnPointsPerRoom)
            {
                if (spawnedSapphire >= MAX_SPAWNPOINT)
                {
                    return;
                }

                int nbChildren = spawnPointRoom.transform.childCount;
                int randIndex = Random.Range(0, nbChildren);
                GameObject chosenChild = spawnPointRoom.transform.GetChild(randIndex).gameObject;
                Vector3 spawnPosition = chosenChild.transform.position;
                spawnPosition.y += 1;
                Runner.Spawn(sapphire, spawnPosition, Quaternion.identity, Object.StateAuthority);
                spawnedSapphire += 1;

                chosenChild.GetComponent<SpawningPoint>().isTaken = true;
            }
        }


        List<GameObject> shuffleList(List<GameObject> listToShuffle)
        {
            var randomized = listToShuffle.OrderBy(item => Random.Range(0, listToShuffle.Count)).ToList();
            return randomized;
        }

        void SpawnPowerups()
        {
            spawnPointsList = shuffleList(GameObject.FindGameObjectsWithTag("SpawningPoint").ToList());


            foreach (GameObject spawnPoint in spawnPointsList)
            {
                if (spawnPoint.GetComponent<SpawningPoint>().isTaken)
                {
                    continue;
                }

                float randomNumber = Random.Range(0, 100);
                if (randomNumber >= threshold)
                {
                    Vector3 spawnPosition = spawnPoint.transform.position;
                    spawnPosition.y += 1;

                    int randomIndex = Random.Range(0, powerupPrefabs.Count);
                    Quaternion prefabRotation = powerupPrefabs[randomIndex].transform.rotation;
                    Debug.Log(prefabRotation);
                    Runner.Spawn(powerupPrefabs[randomIndex], spawnPosition, prefabRotation,
                        Object.StateAuthority);
                    spawnPoint.GetComponent<SpawningPoint>().isTaken = true;
                }
            }
        }
    }
}