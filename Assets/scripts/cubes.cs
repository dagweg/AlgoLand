using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class cubes : MonoBehaviour
{

    List<GameObject> cubz = new();

    [SerializeField] GameObject cube;
    [SerializeField] Camera mainCamera;
    private GameObject textObject;
    [SerializeField] float fontSize = 0.7f;

    [SerializeField] float cubeTextVerticalSpacing = 1f;
    [SerializeField] Vector3 spacing;
    [SerializeField] float heightDifference;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        PopulateCubes();
        PopulateCubesUI();
    }

    void PopulateCubes(){
        Vector3 curCubePos = cube.transform.position;
        
        Bounds cubeBounds = gh.CalculateBounds(cube);

        for(int i = 0; i < 10; i ++){
            
            // Instantiate a new cube
            GameObject cubeInst = Instantiate(cube,curCubePos,Quaternion.identity);

            // Apply random y scaling
            float rand = Mathf.RoundToInt(Random.Range(1,heightDifference));
            Vector3 scale = cubeInst.transform.localScale;
            cubeInst.transform.localScale = new Vector3(scale.x,scale.y * rand,scale.z);
            
            // Normalize the distance from origin
            Bounds bounds = gh.CalculateBounds(cubeInst);
            Debug.Log(bounds.extents.y + " and " + cubeBounds.extents.y);
            float deltaFromOrigin = Mathf.Abs(bounds.extents.y - cubeBounds.extents.y);

            if(gh.CompareSize(cubeInst,cube) == -1){
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

    void PopulateCubesUI(){
        textObject = new GameObject("Text Component");
        GameObject canvasObj = new();
        foreach(GameObject cube in cubz){
            GameObject canvasObjInst = Instantiate(canvasObj,canvasObj.transform); 

            Canvas canvas = canvasObjInst.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = mainCamera;

            GameObject textInst = Instantiate(textObject,textObject.transform);
           
            canvasObjInst.transform.parent = cube.transform;
            textInst.transform.parent = canvasObjInst.transform;
            
            TextMeshProUGUI textCompInst = textInst.AddComponent<TextMeshProUGUI>();
            textCompInst.alignment = TextAlignmentOptions.Center;
            textCompInst.fontSize = fontSize;
            
            textCompInst.text = gh.GetHeight(cube).ToString();

            textInst.transform.position += gh.GetTopPosition(cube,cubeTextVerticalSpacing);
            
        }
    }


    void Update()
    {
        
    }
}