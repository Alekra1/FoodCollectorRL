using UnityEngine;
using System.Collections.Generic;

public class FoodCollectorArea : MonoBehaviour
{
    public float range = 9f;
    public float spawnHeight = 0.5f;
    public Vector3 centerOffset = new Vector3(-0.91f, 0f, -26.1f);
    public float minDistance = 2f;
    public int maxSpawnAttempts = 30;
    private List<Vector3> usedPositions = new List<Vector3>();
    public Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-range, range);
        float randomZ = Random.Range(-range, range);
        Vector3 localPos = centerOffset + new Vector3(randomX, spawnHeight, randomZ);
        return transform.TransformPoint(localPos);
    }

    private Vector3 FindSafePosition()
    {
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Vector3 testPosition = GetRandomSpawnPosition();
            bool isGood = true;
            foreach (Vector3 usedPos in usedPositions)
            {
                if (Vector3.Distance(testPosition, usedPos) < minDistance)
                {
                    isGood = false;
                    break;
                }
            }
            if (isGood)
            {
                return testPosition;
            }
        }
        return GetRandomSpawnPosition();
    }
    public void ResetArea()
    {
        usedPositions.Clear();
        FoodLogic[] allFood = GetComponentsInChildren<FoodLogic>();
        foreach (FoodLogic food in allFood)
        {
            Vector3 newPos = FindSafePosition();
            food.transform.position = newPos;
            usedPositions.Add(newPos);
        }
    }
}
