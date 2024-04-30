using UnityEngine;
using UnityEngine.SocialPlatforms;

public class TargetController : MonoBehaviour
{
    [SerializeField]
    private float minSpeed = 5.0f;

    [SerializeField]
    private float maxSpeed = 165.0f;

    [SerializeField]
    private Avoidance agent = null;

    private float resetPoint = -3.5f;
    private float movementSpeed = 0;
    private Vector3 startPosition;

    void Awake()
    {
        startPosition = transform.localPosition;
        movementSpeed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        if (resetPoint >= transform.localPosition.z)
        {
            ResetTarget();
        }
        else
        {
            transform.localPosition -= new Vector3(0, 0, movementSpeed * Time.deltaTime);
        }
    }

    public void ResetTarget()
    {
        movementSpeed = Random.Range(minSpeed, maxSpeed);
        transform.localPosition = startPosition;
        transform.localRotation = Quaternion.identity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("Award gained");
            agent.Award();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Pentalty gained");
            agent.Penalty();
        }
    }


}
