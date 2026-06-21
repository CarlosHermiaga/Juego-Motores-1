using UnityEngine;

public class SegmentFurniture : MonoBehaviour
{
    public Transform[] spawnPoints;

    public GameObject[] furniturePrefabs;

    [Range(0, 100)]
    public int spawnChance = 50;

    void Start()
    {
        foreach (Transform point in spawnPoints)
        {
            if (Random.Range(0, 100) < spawnChance)
            {
                GameObject furniture =
                    furniturePrefabs[
                        Random.Range(0, furniturePrefabs.Length)
                    ];

                Instantiate(
                    furniture,
                    point.position,
                    point.rotation,
                    transform
                );
            }
        }
    }
}