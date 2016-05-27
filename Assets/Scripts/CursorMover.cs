﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CursorMover : MonoBehaviour {
    public GameObject selectedOption;
    public GameObject[] menuObjects;

    private float scaleOfCanvas;
    private Vector2 direction;
    private bool directionSet;

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
        directionSet = true;
    }

    public Vector2 GetDirection() {
        return direction;
    }

    public GameObject GetSelectedOption()
    {
        return selectedOption;
    }

    // Use this for initialization
    void Start () {
        scaleOfCanvas = GameObject.Find("MainMenu").transform.localScale.x;
    }
	
	// Update is called once per frame
	void Update () {
        if (directionSet) {    
            if (direction.x != 0) {
                selectedOption = GetClosestHorizontalSelection(direction);
            }
            if (direction.y != 0) {
                selectedOption = GetClosestVerticalSelection(direction);
            }
            gameObject.transform.position = new Vector2(GetCursorXPosition(selectedOption), selectedOption.transform.position.y);
            directionSet = false;
        }
	}

    private GameObject GetClosestHorizontalSelection(Vector2 direction)
    {
        var curr = selectedOption.transform.position;
        float smallestDistance = 9999999999999f;
        Debug.Log(direction);
        int pointer = -1;
        for (int i = 0; i < menuObjects.Length; i++)
        {
            if (samePositions(curr.y, menuObjects[i].transform.position.y))
            {

                //to fix: pushing down twice at length of array will cause the cursor to go up
                Vector2 midVector = menuObjects[i].transform.position - curr;
                Debug.Log("horizontal mid: " + midVector);
                Debug.Log("horizontal dir: " + direction);
                if (Vector2.Distance(curr, menuObjects[i].transform.position) < smallestDistance &&
                    midVector.normalized.x == direction.normalized.x && menuObjects[i] != selectedOption)
                {
                    //and if the vector between a and b normalized in y direction is same as direction
                    smallestDistance = Vector2.Distance(curr, menuObjects[i].transform.position);
                    pointer = i;

                }
            }

        }
        if (pointer == -1)
        {
            return selectedOption;
        }
        return menuObjects[pointer];
    }

    private GameObject GetClosestVerticalSelection(Vector2 direction)
    {
        var curr = selectedOption.transform.position;
        float smallestDistance = 9999999999999f;
        int pointer = -1;
        for (int i = 0; i < menuObjects.Length; i++)
        {
            if (!samePositions(curr.y, menuObjects[i].transform.position.y))
            {
                Vector2 midVector = menuObjects[i].transform.position - curr;
                Vector2 directionOfMid = new Vector2(0, 0);
                if (midVector.normalized.y < 0)
                {
                    directionOfMid = new Vector2(0, -1);
                }
                else
                {
                    directionOfMid = new Vector2(0, 1);
                }
                if (Vector2.Distance(curr, menuObjects[i].transform.position) < smallestDistance && 
                    directionOfMid.normalized.y == direction.normalized.y && menuObjects[i] != selectedOption)
                {
                    //and if the vector between a and b normalized in y direction is same as direction
                    smallestDistance = Vector2.Distance(curr, menuObjects[i].transform.position);
                    pointer = i;
                }
            }

        }
        if (pointer == -1)
        {
            return selectedOption;
        }
        return menuObjects[pointer];
    }

    private bool samePositions(float pos1, float pos2)
    {
        if (Mathf.Approximately(pos1, pos2))
        {
            return true;
        }
        return false;
    }

    private bool CompareVectors(Vector2 a, Vector2 b, float angleError) {
        if (!Mathf.Approximately(a.magnitude, b.magnitude))
        {
            return false;
        }
        var cosAngleError = Mathf.Cos(angleError * Mathf.Deg2Rad);

        var cosAngle = Vector3.Dot(a.normalized, b.normalized);
        if (cosAngle >= cosAngleError)
        {
            //If angle is greater, that means that the angle between the two vectors is less than the error allowed.
            return true;
        }
        else
        {
            return false;
        }
    }




    private float GetCursorXPosition(GameObject selected)
    {
        RectTransform rt = selected.GetComponent<RectTransform>();
        float x = selected.transform.position.x / scaleOfCanvas;
        float cursorPos = x - (rt.rect.width / 2.0f);
        cursorPos = cursorPos - 40.0f;

        //have to multiply by the scale set in canvas
        cursorPos = cursorPos * scaleOfCanvas;


        return cursorPos;
    }


}