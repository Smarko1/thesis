using TMPro;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents;
using UnityEngine;

public class Avoidance : Agent
{
    [SerializeField]
    private float moveSpeed = 45.0f;
    [SerializeField]
    private Vector3 idlePosition = Vector3.zero;
    [SerializeField]
    private Vector3 leftPosition = Vector3.zero;
    [SerializeField]
    private Vector3 rightPosition = Vector3.zero;

    private TargetController targetController = null;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 lastPosition = Vector3.zero;
    private int errorCounter;

    void Awake()
    {
        targetController = transform.parent.GetComponentInChildren<TargetController>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = idlePosition;
        targetPosition = lastPosition = idlePosition;
        errorCounter = 0;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetController.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var vectorAction = actionBuffers.DiscreteActions;
        lastPosition = targetPosition;
        int moveDirection = vectorAction[0];
        targetPosition = idlePosition;

        /*
        switch (moveDirection)
        {
            case 0:
                targetPosition = idlePosition;
                break;
            case 1:
                targetPosition = leftPosition;
                break;
            case 2:
                targetPosition = rightPosition;
                break;
        }

        */

        if (moveDirection == 0)
        {
            targetPosition = idlePosition;
        }
        else if (moveDirection == 1)
        {
            targetPosition = leftPosition;
        }
        else if (moveDirection == 2)
        {
            targetPosition = rightPosition;
        }

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, Time.fixedDeltaTime * moveSpeed);

        if (lastPosition == targetPosition)
        {
            errorCounter++;
        }

        if (errorCounter > 3)
        {
            AddReward(-0.01f);
            errorCounter = 0;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        if (Input.GetKeyDown(KeyCode.DownArrow)) discreteActions[0] = 0;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) discreteActions[0] = 1;
        if (Input.GetKeyDown(KeyCode.RightArrow)) discreteActions[0] = 2;
    }

    public void Award()
    {
        AddReward(1.0f);
        targetController.ResetTarget();
        EndEpisode();
    }

    public void Penalty()
    {
        AddReward(-0.01f);
        targetController.ResetTarget();
        EndEpisode();
    }
}