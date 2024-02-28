using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedEditorTools;
using AdvancedEditorTools.Attributes;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the movement of the camera
/// </summary>
public class CameraMovement : MonoBehaviour
{
    [BeginFoldout("Movement")]
    [SerializeField] Transform targetTransform;
    [SerializeField] float cameraSpeed;
    [EndFoldout]

    [BeginFoldout("Zoom")]
    [SerializeField] float minSize;
    [SerializeField] float maxSize;
    [SerializeField] float zoomSpeed;
    [SerializeField] InputActionReference zoomAction;

    

    void Update()
    {
        Vector3 targetPos = new Vector3(targetTransform.position.x, targetTransform.position.y, transform.position.z);
        Vector3 moveVector = Vector3.Slerp(transform.position, targetPos,cameraSpeed*Time.deltaTime);
        transform.position = moveVector;

        float zoomInput = zoomAction.action.ReadValue<float>();
        
        float cameraSize = Camera.main.orthographicSize;
        cameraSize += zoomInput * zoomSpeed * Time.deltaTime;
        if(cameraSize < minSize) cameraSize = minSize;
        else if (cameraSize > maxSize) cameraSize = maxSize;
        Camera.main.orthographicSize = cameraSize;
    }
}
