using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player, blackHole;

    private Rigidbody playerRB, blackHoleRB;

    private float currentPlayerMass, currentBlackHoleMass;

    private LineRenderer orbit;

    [SerializeField]
    private int nrSteps;

    [SerializeField]
    private float lineWidth, stepSize;
    private float currentLineWidth, currentStepSize, currentNrSteps;

    [SerializeField]
    private Vector3 initialVelocity;
    [SerializeField]
    private Vector3 initialPosition;

    private Vector3 currentInitialVelocity, currentInitialPosition;

    Body playerScript;

    private void Start()
    {
        orbit = GetComponent<LineRenderer>();
        playerScript = player.GetComponent<Body>();
        currentInitialVelocity = initialVelocity;
        currentInitialPosition = player.transform.position;
        initialPosition = currentInitialPosition;
        currentLineWidth = lineWidth;
        currentNrSteps = nrSteps;
        currentStepSize = stepSize;
        playerRB = player.GetComponent<Rigidbody>();
        blackHoleRB = blackHole.GetComponent<Rigidbody>();
        currentPlayerMass = playerRB.mass;
        currentBlackHoleMass = blackHoleRB.mass;

        GenerateOrbit();

    }

    private void Update()
    {
        bool hasChanged = CheckForChanges();
        if (hasChanged)
            GenerateOrbit();

    }

    bool CheckForChanges()
    {
        if (initialVelocity != currentInitialVelocity)
        {
            currentInitialVelocity = initialVelocity;
            return true;
        }
        if (initialPosition != currentInitialPosition)
        {
            currentInitialPosition = initialPosition;
            return true;
        }
        if (currentNrSteps != nrSteps)
        {
            currentNrSteps = nrSteps;
            return true;
        }
        if(currentLineWidth != lineWidth)
        {
            currentLineWidth = lineWidth;
            return true;
        }
        if(currentStepSize != stepSize)
        {
            currentStepSize = stepSize;
            return true;
        }
        if(currentPlayerMass != playerRB.mass)
        {
            currentPlayerMass = playerRB.mass;
            return true;
        }
        if (currentBlackHoleMass != blackHoleRB.mass)
        {
            currentBlackHoleMass = blackHoleRB.mass;
            return true;
        }
        return false;
    }
    void GenerateOrbit()
    {
        orbit.startWidth = orbit.endWidth = lineWidth;
        Vector3[] points = new Vector3[nrSteps];
        points[0] = initialPosition;

        Vector3 currentVelocity = initialVelocity;
        
        for (int i = 1; i < nrSteps; i++)
        {
            currentVelocity = playerScript.UpdateVelocity(stepSize, points[i - 1], currentVelocity);
            Vector3 newPos = playerScript.UpdatePosition(stepSize, points[i - 1], currentVelocity);
            points[i] = newPos;
            if (Vector3.Distance(newPos, blackHole.transform.position) < 1)
                break;
        }
        
        orbit.positionCount = nrSteps;
        orbit.SetPositions(points);
    }
}
