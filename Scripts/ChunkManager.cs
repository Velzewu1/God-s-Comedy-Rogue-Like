using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class ChunkManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    private Transform chunkRoot;
    private Dictionary<Transform, bool> entryPointCache = new Dictionary<Transform, bool>();

    private void Start()
    {
        InvokeRepeating("SpawnChild", 2f, 1f);
    }
    public void SpawnChild() 
    {
        List<Transform> availableEntryPoints = SelectRoot();
        if (availableEntryPoints == null || availableEntryPoints.Count == 0)
            return;

        Transform randEntryPoint = availableEntryPoints[UnityEngine.Random.Range(0, availableEntryPoints.Count)];

        float[] entryPointPositions = { randEntryPoint.localPosition.x, randEntryPoint.localPosition.z };

        Vector3 entryShift = new Vector3();

        for (int i = 0; i < entryPointPositions.Length; i++)
        {
            if (entryPointPositions[i] > 0)
                entryPointPositions[i] += 5;
            else if (entryPointPositions[i] < 0)
                entryPointPositions[i] -= 5;
            else
                entryPointPositions[i] = 0;

            switch (i)
            {
                case 0:
                    entryShift.x = entryPointPositions[i] + chunkRoot.transform.position.x;
                    break;
                case 1:
                    entryShift.z = entryPointPositions[i] + chunkRoot.transform.position.z;
                    break;
            }
        }

        GameObject chunkLoaded = Instantiate(chunkPrefab, entryShift, Quaternion.identity);
        chunkLoaded.transform.SetParent(transform);

        foreach (KeyValuePair<Transform, bool> entry in entryPointCache)
        {
            Debug.Log(entry);
        }
        entryPointCache[randEntryPoint] = false;
    }

    private List<Transform> SelectRoot()
    {
        if (transform.childCount == 0)
            return new List<Transform>();

        chunkRoot = transform.GetChild(UnityEngine.Random.Range(0, transform.childCount));

        List<Transform> entryPoints = new List<Transform>();
        for (int i = 0; i < chunkRoot.childCount; i++)
        {
            Transform entry = chunkRoot.GetChild(i);
            if (IsValidEntryPoint(entry))
            {
                entryPoints.Add(entry);
            }
        }

        if (entryPoints.Count > 0)
            return entryPoints;

        return new List<Transform>();
    }

    private bool IsValidEntryPoint(Transform entry)
    {
        if (!entryPointCache.ContainsKey(entry))
        {
            entryPointCache[entry] = !entry.GetComponent<CheckOverlap>().IsEntryPointOverlapping();
        }
        return entryPointCache[entry];
    }
}