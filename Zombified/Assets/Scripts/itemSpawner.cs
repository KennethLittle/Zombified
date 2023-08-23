using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemSpawner : MonoBehaviour
{
    [SerializeField] GameObject itemToSpawn;
    [SerializeField] Transform[] itemSpawnPos;
    [SerializeField] int numberItemToSpawn;
    [SerializeField] float timeBetweenItemSpawns;
    [SerializeField] List<GameObject> itemList = new List<GameObject>();

    int itemsSpawned;
    bool itemIsSpawning;
    bool startItemSpawning;
    bool itemDeSpawn;

    void Update()
    {
        if (startItemSpawning && !itemIsSpawning && itemsSpawned < numberItemToSpawn)
        {
            StartCoroutine(SpawnItem());
        }
    }

    IEnumerator SpawnItem()
    {
        itemIsSpawning = true;

        GameObject itemSpawned = Instantiate(itemToSpawn, itemSpawnPos[Random.Range(0, itemSpawnPos.Length)].position, itemToSpawn.transform.rotation);

        itemList.Add(itemSpawned);
        itemsSpawned++;
        yield return new WaitForSeconds(timeBetweenItemSpawns);

        itemIsSpawning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startItemSpawning = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            itemDeSpawn = true;

            for (int i = 0; i < itemList.Count; i++)
            {
                Destroy(itemList[i]);
            }

            itemList.Clear();
            itemsSpawned = 0;
            startItemSpawning = false;
        }
    }
}
