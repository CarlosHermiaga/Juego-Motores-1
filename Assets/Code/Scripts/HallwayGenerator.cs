using System.Collections.Generic;
using UnityEngine;

public class HallwayGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] segmentPrefabs;

    private Transform currentExit;
    private List<GameObject> spawnedSegments = new List<GameObject>();

    [SerializeField] int maxSegments = 5;

    public Transform contenedorPasillos;

    void Start()
    {
        SpawnInitialSegments();
    }

    void SpawnInitialSegments()
    {
        for (int i = 0; i < maxSegments; i++)
        {
            SpawnSegment();
        }
    }

    public void SpawnSegment()
    {
        GameObject prefab =
            segmentPrefabs[
                Random.Range(0, segmentPrefabs.Length)
            ];

        GameObject segment;

        if (currentExit == null)
        {
            segment = Instantiate(prefab,
                Vector3.zero,
                Quaternion.identity);
        }
        else
        {
            segment = Instantiate(prefab,
                currentExit.position,
                currentExit.rotation,
                contenedorPasillos);
        }

        currentExit = segment.transform.Find("Exit");

        spawnedSegments.Add(segment);

        if (spawnedSegments.Count > maxSegments)
        {
            Destroy(spawnedSegments[0]);
            spawnedSegments.RemoveAt(0);
        }
    }
}