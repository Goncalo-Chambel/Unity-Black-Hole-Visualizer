
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Body : MonoBehaviour
{

    public bool isSimPlaying = false;
    public static float G = 0.01f;
    [SerializeField]
    private float mass, radius;

    [SerializeField]
    private Vector3 initialVelocity, currentVelocity, initialPosition;

    [SerializeField]
    private Rigidbody blackHoleRb;

    public Rigidbody rb;

    private Transform camera;
    private void Awake()
    {
        currentVelocity = initialVelocity;
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        initialPosition = rb.position;
        Time.fixedDeltaTime = 0.01f;
        camera = transform.Find("Camera");
    }

    public Vector3 UpdateVelocity(float timeStep, Vector3 _currentPosition, Vector3 _currentVelocity)
    {
        float sqrtDist = (blackHoleRb.position - _currentPosition).sqrMagnitude;
        Vector3 forceDir = (blackHoleRb.position - _currentPosition).normalized;
        Vector3 force = forceDir * G * mass * blackHoleRb.mass / sqrtDist;
        Vector3 acceleration = force / mass;
        return _currentVelocity + acceleration * timeStep;
    }

    public Vector3 UpdatePosition(float timeStep, Vector3 _currentPosition, Vector3 _currentVelocity)
    { 
        Vector3 newVelocity = UpdateVelocity(timeStep, _currentPosition, _currentVelocity);
        return _currentPosition + newVelocity * timeStep;
    }

    public void FixedUpdate()
    {
        if (isSimPlaying)
        {
            currentVelocity = UpdateVelocity(Time.fixedDeltaTime, rb.position, currentVelocity);
            rb.position = UpdatePosition(Time.fixedDeltaTime, rb.position, currentVelocity);
            camera.LookAt(blackHoleRb.gameObject.transform.position);
        }


        if (Vector3.Distance(rb.position, blackHoleRb.position) < 1)
        {
            currentVelocity = Vector3.zero;
            transform.position = initialPosition;
        }

    }
}
