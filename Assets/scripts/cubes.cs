using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cubes : MonoBehaviour
{

    List<GameObject> cubz = new();

    [SerializeField] GameObject cube;        
    [SerializeField] Camera mainCamera;
    private GameObject textObject;
    [SerializeField] float fontSize = 0.7f;

    [SerializeField] float cubeTextVerticalSpacing = 1f;
    [SerializeField] Vector3 spacing;
    [SerializeField] float heightDifference;

    [SerializeField] string CUBE_HEIGHT_TAG = "cube_height";

    void Start()
    {
        Cursor.visible = true;
  
        PopulateCubes();
        PopulateCubesUI();
    }
    
    void PopulateCubes(){
        Vector3 curCubePos = cube.transform.position;
        
        Bounds cubeBounds = Gh.CalculateBounds(cube);

        for(int i = 0; i < 10; i ++){
            
            // Instantiate a new cube
            GameObject cubeInst = Instantiate(cube,curCubePos,Quaternion.identity);

            // Apply random y scaling
            float rand = Random.Range(1, Mathf.RoundToInt(heightDifference));
            Vector3 scale = cubeInst.transform.localScale;
            cubeInst.transform.localScale = new Vector3(scale.x,scale.y * rand,scale.z);
            
            // Normalize the distance from origin
            Bounds bounds = Gh.CalculateBounds(cubeInst);
            // Debug.Log(bounds.extents.y + " and " + cubeBounds.extents.y);
            float deltaFromOrigin = Mathf.Abs(bounds.extents.y - cubeBounds.extents.y);

            if(Gh.CompareSize(cubeInst,cube) == -1){
                cubeInst.transform.position += transform.TransformPoint(0, deltaFromOrigin,0);
            }
            else{
                cubeInst.transform.position -= transform.TransformPoint(0, deltaFromOrigin,0);
            }

            // Change cubePosition by spacing for the next iteration
            curCubePos += spacing;       
            cubeInst.transform.SetParent(transform);     
            cubz.Add(cubeInst);                 
        }
    }

    void PopulateCubesUI(){
        textObject = new GameObject("Text Component");
        GameObject canvasObj = new();
        foreach(GameObject cube in cubz){
            GameObject canvasObjInst = Instantiate(canvasObj,canvasObj.transform); 

            Canvas canvas = canvasObjInst.AddComponent<Canvas>();
            canvas.name = "Canvas";
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = mainCamera;

            GameObject textInst = Instantiate(textObject,textObject.transform);

            canvasObjInst.transform.SetParent(cube.transform);
            textInst.transform.SetParent(canvasObjInst.transform);
            textInst.tag = CUBE_HEIGHT_TAG;
            
            TextMeshProUGUI textCompInst = textInst.AddComponent<TextMeshProUGUI>();
            textCompInst.alignment = TextAlignmentOptions.Center;
            textCompInst.fontSize = fontSize;            
            textCompInst.text = Gh.GetHeight(cube).ToString();

            canvasObjInst.transform.position += Gh.GetTopPosition(cube,cubeTextVerticalSpacing);
            
        }
    }

    public void BubbleSort() {
        int n = cubz.Count;
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n-i-1; j++) {
                int height1 = Mathf.RoundToInt(Gh.GetHeight(cubz[j]));
                int height2 = Mathf.RoundToInt(Gh.GetHeight(cubz[j + 1]));
                if (height1 > height2) {
                    GameObject c1 = cubz[j];
                    GameObject c2 = cubz[j+1];
                    Gh.SwapXZPosition(ref c1,ref c2);
                    (cubz[j], cubz[j+1]) = (c2, c1);
                }
            }
        }
    }

}