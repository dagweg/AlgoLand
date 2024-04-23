using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{

    [SerializeField] GameObject lookAtObj;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        FitAllObjectsInView();        
    }

    void FitAllObjectsInView()
    {
        // Get the bounds of all objects within the container
        Bounds bounds = Gh.CalculateBounds(lookAtObj);

        // Calculate the distance the camera should be placed from the center of the bounding box
        float distance = bounds.size.magnitude / (2 * Mathf.Tan(Mathf.Deg2Rad * Camera.main.fieldOfView / 2));

        // Position the camera at the center of the bounding box
        Camera.main.transform.position = bounds.center - Camera.main.transform.forward * distance;

        // Adjust camera's clipping planes if necessary
        Camera.main.nearClipPlane = distance - bounds.size.magnitude;
        Camera.main.farClipPlane = distance + bounds.size.magnitude;

        // Optionally, you can also zoom the camera's field of view to fit all objects within view
        Camera.main.fieldOfView = CalculateFieldOfView(bounds.size.magnitude);
    }

    float CalculateFieldOfView(float distance)
    {
        // Calculate field of view based on the bounding box's diagonal distance
        float fov = 2 * Mathf.Atan(lookAtObj.transform.lossyScale.x * 0.5f / distance) * Mathf.Rad2Deg;
        return fov;
    }
}
