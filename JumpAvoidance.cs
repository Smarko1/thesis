using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class JumpAvoidance : Agent
{
    public float force = 15f;
    public Transform reset = null;
    public TextMesh score = null;
    public GameObject thrust = null;
    private Rigidbody rb = null;
    private float points = 0;
    private bool isJumping = false;
    public float maxJumpHeight = 5.0f;
    public float lateralMoveSpeed = 5f;

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
        sensor.AddObservation(transform.localPosition);

        RaycastHit hit;
        float maxDistance = 15.0f;
        Vector3 forward = transform.forward;


        if (Physics.Raycast(transform.position, forward, out hit, maxDistance))
        {
            sensor.AddObservation(hit.distance / maxDistance);
           
            
            if (hit.collider.CompareTag("obstacleJumpable"))
            {
                sensor.AddObservation(1.0f);
            }
            else
            {
                sensor.AddObservation(0.0f);
            }

            if (hit.collider.CompareTag("obstacleAvoidable"))
            {
                sensor.AddObservation(1.0f);
            }
            else
            {
                sensor.AddObservation(0.0f);
            }
            
        }
        else
        {
            sensor.AddObservation(1.0f);
            sensor.AddObservation(0.0f);
            sensor.AddObservation(0.0f);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int actionValue = actionBuffers.DiscreteActions[0];
        Debug.Log("Action value: " + actionValue);
        if (actionValue == 1 && !isJumping && transform.position.y < maxJumpHeight){
            UpForce();
            thrust.SetActive(true);
            isJumping = true; 
        }
        else if(actionValue == 2)
        {
            transform.localPosition += Vector3.left * lateralMoveSpeed * Time.fixedDeltaTime;
            Debug.Log("Balra");
        }
        else if (actionValue == 3)
        {
            transform.localPosition += Vector3.right *lateralMoveSpeed * Time.fixedDeltaTime;
            Debug.Log("Jobbra");
        }
    }

    private void DetectObstacleAndReact(ActionBuffers actionBuffers)
    {
        Debug.Log("Meghívódott");
        RaycastHit hit;
        float rayDistance = 10.0f;
        Vector3 forward = transform.forward * rayDistance;
        var discreteActions = actionBuffers.DiscreteActions;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isJumping = false;
        }
    }

    private void UpForce()
    {
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    private void ResetMyAgent()
    {
        transform.position = new Vector3(reset.position.x, reset.position.y, reset.position.z);
        isJumping = false;
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacleJumpable") || collision.gameObject.CompareTag("obstacleAvoidable"))
        {
            AddReward(-1.0f);
            Destroy(collision.gameObject);
            EndEpisode();
        }
        else if (collision.gameObject.CompareTag("TopWall") || collision.gameObject.CompareTag("Wall"))
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
}
