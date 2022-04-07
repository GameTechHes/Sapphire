using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    public List<PowerUpBase> powerupPrefabs;
    public int threshold;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject spawnPoint in GameObject.FindGameObjectsWithTag("SpawningPoint")){
            float randomNumber = Random.Range(0, 100);
            if(randomNumber >= threshold){
                Vector3 spawnPosition = spawnPoint.transform.position;
                spawnPosition.y += 1;
                Instantiate(powerupPrefabs[0], spawnPosition, Quaternion.identity);
            }
        }
    }
}
