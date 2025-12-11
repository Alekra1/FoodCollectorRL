using UnityEngine;

public class FoodLogic : MonoBehaviour
{
    public bool respawn = true;
    public FoodCollectorArea myArea;
    void Start()
    {
        if (myArea == null)
        {
            myArea = GetComponentInParent<FoodCollectorArea>();
        }
    }

    public void OnEaten()
    {
        if (respawn && myArea != null)
        {
            transform.position = FindSafePosition();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Vector3 FindSafePosition()
    {
        for (int i = 0; i < myArea.maxSpawnAttempts; i++)
        {
            Vector3 testPosition = myArea.GetRandomSpawnPosition();

            bool isGood = true;
            FoodLogic[] allFood = myArea.GetComponentsInChildren<FoodLogic>();

            foreach (FoodLogic other in allFood)
            {
                if (other == this) continue;

                if (Vector3.Distance(testPosition, other.transform.position) < myArea.minDistance)
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

        return myArea.GetRandomSpawnPosition();
    }
}
