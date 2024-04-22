using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class cubes : MonoBehaviour
{

    List<GameObject> cubz = new();

    [SerializeField] GameObject cube;
    [SerializeField] Vector3 spacing;
    [SerializeField] float heightDifference;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    
        PopulateCubes();
    }

    void PopulateCubes(){
        Vector3 curCubePos = cube.transform.position;

        Bounds cubeBounds = CalculateBounds(cube);

        for(int i = 0; i < 10; i ++){
            
            // Instantiate a new cube
            GameObject cubeInst = Instantiate(cube,curCubePos,Quaternion.identity);

            // Apply random y scaling
            float rand = Random.Range(1,heightDifference);
            Vector3 scale = cubeInst.transform.localScale;
            cubeInst.transform.localScale = new Vector3(scale.x,scale.y * rand,scale.z);
            
            // Normalize the distance from origin
            Bounds bounds = CalculateBounds(cubeInst);
            Debug.Log(bounds.extents.y + " and " + cubeBounds.extents.y);
            float deltaFromOrigin = Mathf.Abs(bounds.extents.y - cubeBounds.extents.y);

            if(CompareSize(cubeInst,cube) == -1){
                cubeInst.transform.position += transform.TransformPoint(0, deltaFromOrigin,0);
            }
            else{
                cubeInst.transform.position -= transform.TransformPoint(0, deltaFromOrigin,0);
            }

            // Change cubePosition by spacing for the next iteration
            curCubePos += spacing;            
            cubz.Add(cubeInst);     
            
        }
    }

    // Return -1 if o1 is greater, 0 if equal, 1 otherwise
    int CompareSize(GameObject o1, GameObject o2){
        Vector3 s1 = CalculateBounds(o1).size;
        Vector3 s2 = CalculateBounds(o2).size;
        float v1 = s1.x * s1.y * s1.z;
        float v2 = s2.x * s2.y * s2.z;
        if (v1 > v2){
            return -1;
        }
        else if (v1 < v2){
            return 1;
        }
        
        return 0;
    }   

    Bounds CalculateBounds(GameObject obj){
        Renderer rend = obj.GetComponent<Renderer>();
        if(rend) return rend.bounds;
        Collider coll = obj.GetComponent<Collider>();
        if(coll) return coll.bounds;

        Debug.Log("Bounds coundnt be calculated.. falling back to default");
        return new Bounds(obj.transform.position,Vector3.one);
    }


    void Update()
    {
        
    }
}
