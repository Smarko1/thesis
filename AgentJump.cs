using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class AgentJump : Agent
{
    public float force = 15f;
    public Transform reset = null;
    public TextMesh score = null;
    public GameObject jumpObject = null;
    private Rigidbody rb = null;
    private float points = 0;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        ResetMyAgent();
    }

    public override void OnEpisodeBegin()
    {
        ResetMyAgent();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hitObstacle, 100f))
        {
            sensor.AddObservation(hitObstacle.distance / 100f);
        }
        else
        {
            sensor.AddObservation(1.0f);
        }
        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hitCeiling, 10f))
        {
            sensor.AddObservation(hitCeiling.distance / 10f);
        }
        else
        {
            sensor.AddObservation(1.0f);
        }

        sensor.AddObservation(rb.velocity.y / 10f);

        sensor.AddObservation(transform.position.y / 5f);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float actionValue = actionBuffers.DiscreteActions[0];
        Debug.Log("Action value: " + actionValue);
        if (actionValue == 1.0f)
        {
            Jump();
            jumpObject.SetActive(true);
        }
        else
        {
            jumpObject.SetActive(false);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            continuousActions[0] = 1f;
        }
        else
        {
            continuousActions[0] = 0f;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacleJumpable"))
        {
            AddReward(-1.0f);
            Destroy(collision.gameObject);
            EndEpisode();
        }
        else if (collision.gameObject.CompareTag("TopWall"))
        {
            AddReward(-0.9f);
            EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WallReward"))
        {
            AddReward(0.1f);
            points++;
            score.text = points.ToString();
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * force, ForceMode.Acceleration);
    }

    private void ResetMyAgent()
    {
        transform.position = new Vector3(reset.position.x, reset.position.y, reset.position.z);
    }
}