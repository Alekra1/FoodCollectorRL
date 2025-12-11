using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class FoodAgent : Agent
{
    public float moveSpeed = 1.5f;
    public float turnSpeed = 200f;
    public float maxVelocity = 5f;
    public float randomSpawnRange = 2f;
    public FoodCollectorArea myArea;

    private Rigidbody rb;
    private Vector3 startPosition;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.localPosition;

        if (rb != null)
        {
            rb.linearDamping = 4f;
            rb.angularDamping = 5f;
        }
    }

    public override void OnEpisodeBegin()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        float randomX = Random.Range(-randomSpawnRange, randomSpawnRange);
        float randomZ = Random.Range(-randomSpawnRange, randomSpawnRange);
        transform.localPosition = startPosition + new Vector3(randomX, 0f, randomZ);
        transform.localRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        if (myArea != null)
        {
            myArea.ResetArea();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (rb != null)
        {
            Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
            sensor.AddObservation(localVelocity.x);
            sensor.AddObservation(localVelocity.z);
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float forwardAction = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        float strafeAction = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);
        float rotateAction = Mathf.Clamp(actions.ContinuousActions[2], -1f, 1f);

        Vector3 moveDirection = transform.forward * forwardAction + transform.right * strafeAction;
        //Vector3 moveDirection = transform.forward * forwardAction + transform.right * strafeAction;

        if (rb != null)
        {
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);

            if (rb.linearVelocity.magnitude > maxVelocity)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
            }
        }

        transform.Rotate(0f, rotateAction * turnSpeed * Time.fixedDeltaTime, 0f);

        AddReward(-0.0001f);
    }

    // Keyboard controls for testing
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.ContinuousActions;

        actions[0] = 0f;
        if (Input.GetKey(KeyCode.W)) actions[0] = 1f;
        if (Input.GetKey(KeyCode.S)) actions[0] = -1f;

        actions[1] = 0f;
        if (Input.GetKey(KeyCode.D)) actions[1] = 1f;
        if (Input.GetKey(KeyCode.A)) actions[1] = -1f;

        actions[2] = 0f;
        if (Input.GetKey(KeyCode.E)) actions[2] = 1f;
        if (Input.GetKey(KeyCode.Q)) actions[2] = -1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other.gameObject);
    }

    private void HandleCollision(GameObject obj)
    {
        if (obj.CompareTag("Food"))
        {
            AddReward(1.0f);

            FoodLogic food = obj.GetComponent<FoodLogic>();
            if (food != null) food.OnEaten();
        }
        else if (obj.CompareTag("Poison"))
        {
            AddReward(-1.0f);

            FoodLogic food = obj.GetComponent<FoodLogic>();
            if (food != null) food.OnEaten();
        }
        else if (obj.CompareTag("Wall"))
        {
            AddReward(-0.1f);
        }
    }
}
