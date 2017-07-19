using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAnimator : MonoBehaviour {

	[SerializeField]
	float speedMultiplier = 0.1f;

	[SerializeField]
	bool alignTiltToPath = true;

	[SerializeField]
	GameObject targetObject;

    [SerializeField]
    int rotateDirection = 0;

    int noOfNodes = 0;
	int currentIndex = 0;

	Vector3 currentPosition;
	Vector3 nextPosition;

	bool moving = false;

	float movementFraction;
	float currentFraction = 0f;

	Vector3 dirVector;

	void Start()
	{
		noOfNodes = gameObject.transform.childCount;
	}

	void Update()
	{
		if (!moving)
		{
			if (currentIndex < noOfNodes - 1)
			{
				UpdateDirection();
				MoveInDirection();
			}
		}
		else
		{
			MoveInDirection();
		}
	}

	void MoveInDirection()
	{
		currentFraction += movementFraction;

		if (currentFraction > 1f)
		{
			currentFraction = 1f;
			currentIndex++;
			moving = false;
		}

		targetObject.transform.position = Vector3.Lerp (currentPosition, nextPosition, currentFraction);
	}
    
	void UpdateDirection()
	{
		Transform currentNode = gameObject.transform.GetChild(currentIndex);
        Transform nextNode = gameObject.transform.GetChild(currentIndex + 1);

		currentPosition = currentNode.transform.position;
		nextPosition = nextNode.transform.position;

		targetObject.transform.position = currentPosition;
        targetObject.transform.LookAt(nextPosition);

        if (rotateDirection == 1)
        {
            targetObject.transform.Rotate(Vector3.up * (90));
        }
        else if (rotateDirection == 2)
        {
            targetObject.transform.Rotate(Vector3.up * (180));
        }
        else if (rotateDirection == 3)
        {
            targetObject.transform.Rotate(Vector3.up * (270));
        }
        
        if (!alignTiltToPath)
        {
            targetObject.transform.localEulerAngles = new Vector3(
                0,
                targetObject.transform.localEulerAngles.y,
                targetObject.transform.localEulerAngles.z
                );
        }

        currentFraction = 0f;
		movementFraction = 1 / Vector3.Distance (currentPosition, nextPosition) * speedMultiplier;

		moving = true;

	}
}
