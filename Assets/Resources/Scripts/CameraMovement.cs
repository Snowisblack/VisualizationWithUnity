using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // TODO:    - instead of using W A S D and Shift/Control/MouseWheel we wanna use mouse movement/dragging to get the p_velocity

    public float moveTowardSpeed = 7f;
    public float baseRotationSpeed = 40f;

    private float rotationSpeedOnDistance = 0f;

    public float minDistance, maxDistance;
    public float minAngle = 275f, maxAngle = 85f;

    public GameObject mainObject;
    private GameObject cameraObject;

    private Vector3 distance;

    void Start() {
        if(this.transform.childCount < 1){
            Debug.LogError("No child object found - please add the camera to this object: " + this.transform);
            return;
        }
        cameraObject = this.transform.GetChild(0).gameObject;
    }

    void Update () {
        if(cameraObject == null) {
            return;
        }
        var p = GetBaseInput();
        if(p.sqrMagnitude <= 0) return;
        // Set rotation speed dependend on the current distance
        var distance = transform.position - mainObject.transform.position;
        rotationSpeedOnDistance = baseRotationSpeed + distance.sqrMagnitude / 10;

        // Move towards object
        Vector3 xRot = new Vector3(p.x, 0, 0);
        Vector3 yRot = new Vector3(0, p.y, 0);
        if(xRot.x != 0){
            transform.Rotate(xRot, rotationSpeedOnDistance* Time.deltaTime);
        }if(yRot.y != 0){
            transform.Rotate(yRot, rotationSpeedOnDistance* Time.deltaTime, Space.World);
        }
    }

    private Vector3 GetBaseInput() { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey (KeyCode.A)){
            p_Velocity += new Vector3(0, 1 , 0);
        }
        if (Input.GetKey (KeyCode.D)){
            p_Velocity += new Vector3(0, -1, 0);
        }
        if (Input.GetKey (KeyCode.S) && (transform.localEulerAngles.x > minAngle || (transform.localEulerAngles.x >= 0 && transform.localEulerAngles.x <= maxAngle+5))){
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey (KeyCode.W) && (transform.localEulerAngles.x < maxAngle || (transform.localEulerAngles.x >= minAngle-5))){
            p_Velocity += new Vector3(1, 0, 0);
        }
        // to always keep the same distance we create the vector between the main object and current objects
        distance = cameraObject.transform.position - mainObject.transform.position;
        // make sure to just allow one movement at a time

        // TODO: Clean up to make it more readable
        // print(distance.sqrMagnitude);
        if (Input.GetKey(KeyCode.LeftShift) && (distance.sqrMagnitude > minDistance || distance.sqrMagnitude < -minDistance)){
            cameraObject.transform.position = Vector3.MoveTowards(cameraObject.transform.position, mainObject.transform.position, moveTowardSpeed * Time.deltaTime);
        }else if(Input.GetAxis("Mouse ScrollWheel") > 0 && (distance.sqrMagnitude > minDistance || distance.sqrMagnitude < -minDistance)){
            cameraObject.transform.position = Vector3.MoveTowards(cameraObject.transform.position, mainObject.transform.position, moveTowardSpeed * Time.deltaTime * 10);
        }else if (Input.GetKey(KeyCode.LeftControl) && (distance.sqrMagnitude < maxDistance && distance.sqrMagnitude > -maxDistance)){
            cameraObject.transform.position = Vector3.MoveTowards(cameraObject.transform.position, mainObject.transform.position, -moveTowardSpeed * Time.deltaTime);
        }else if(Input.GetAxis("Mouse ScrollWheel") < 0 && (distance.sqrMagnitude < maxDistance && distance.sqrMagnitude > -maxDistance)){
            cameraObject.transform.position = Vector3.MoveTowards(cameraObject.transform.position, mainObject.transform.position, -moveTowardSpeed * Time.deltaTime * 10);
        }
        
        
        return p_Velocity;
    }
        
}
