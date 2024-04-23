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

    // [SerializeField] string CUBE_HEIGHT_TAG = "cube_height";

    Boolean rotateFinished = true;

    void Start()
    {
        Cursor.visible = true;
  
        PopulateCubes();
        PopulateCubesUI();
    }

    void Update(){
        
    }


    void PopulateCubes(){
        Vector3 curCubePos = cube.transform.position;
        
        Bounds cubeBounds = Gh.CalculateBounds(cube);

        for(int i = 0; i < 10; i ++){
            
            // Create a cube
            GameObject cubeInst = Instantiate(cube,curCubePos,Quaternion.identity);
            
            // Give different height for the instances
            float rand = Random.Range(1, Mathf.RoundToInt(heightDifference));
            Vector3 scale = cubeInst.transform.localScale;
            cubeInst.transform.localScale = new Vector3(scale.x,scale.y * rand,scale.z);
            
            // Make straight with the plane
            float cubeHeight = Gh.GetHeight(cube);
            float cubeInstHeight = Gh.GetHeight(cubeInst);

            float offsetY = (cubeInstHeight - cubeHeight)/2; 
            cubeInst.transform.position += transform.TransformPoint(0, offsetY,0);

            // Make ready for the next iteration by applying spacing
            curCubePos += spacing;       
            cubeInst.transform.SetParent(transform);     
            cubeInst.GetComponent<Renderer>().material.color = Color.black;
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
            // textInst.tag = CUBE_HEIGHT_TAG;
            
            TextMeshProUGUI textCompInst = textInst.AddComponent<TextMeshProUGUI>();
            textCompInst.alignment = TextAlignmentOptions.Center;
            textCompInst.fontSize = fontSize;            
            textCompInst.text = Gh.GetHeight(cube).ToString();

            canvasObjInst.transform.position += Gh.GetTopPosition(cube,cubeTextVerticalSpacing);
            
        }
    }

    public void BubbleSort() {
        int n = cubz.Count;
        StartCoroutine(BubbleSortCoroutine(n));
        
    }

    IEnumerator BubbleSortCoroutine(int n){
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n-i-1; j++) {
                int height1 = Mathf.RoundToInt(Gh.GetHeight(cubz[j]));
                int height2 = Mathf.RoundToInt(Gh.GetHeight(cubz[j + 1]));
                if (height1 > height2) {
                    yield return StartCoroutine(SwapAndRotate(j,j+1));
                }
            }
        }
        string o = "";
        foreach(GameObject go in cubz){
            o += Gh.GetHeight(go) + ", ";
        }
        Debug.Log(o);
        foreach(GameObject go in cubz){
            go.GetComponent<Renderer>().material.color = Color.green;
            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator SwapAndRotate(int i, int j){
        Material mat1 = cubz[i].GetComponent<Renderer>().material;
        Material mat2 = cubz[j].GetComponent<Renderer>().material;
        mat1.color = Color.magenta;
        mat2.color = Color.magenta;
        yield return StartCoroutine(RotateCoroutine(i,j));
        yield return StartCoroutine(SwapCoroutine(i,j));
        mat1.color = Color.black;
        mat2.color = Color.black;
    }   

    
    IEnumerator RotateCoroutine(int i, int j){
        yield return SwapRotateObjectsAlongCommonMidpoint(cubz[i],cubz[j]);
    }

    IEnumerator SwapCoroutine(int i, int j){
        GameObject c1 = cubz[i];
        GameObject c2 = cubz[j];
        // Gh.SwapXZPosition(ref c1, ref c2);
        cubz[i] = c2;
        cubz[j] = c1;
        yield return null;
    }

    public IEnumerator SwapRotateObjectsAlongCommonMidpoint(GameObject obj1, GameObject obj2, float angleIncrement=5f){
        Vector3 mp = Gh.GetMidpoint(obj1,obj2);
        int numRots = Mathf.RoundToInt(180 / Mathf.Abs(angleIncrement));
        
        for(int i = 0; i < numRots; i++){
            Gh.RotateAround(obj1,mp,Vector3.up,angleIncrement);
            Gh.RotateAround(obj2,mp,Vector3.up,angleIncrement);
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }


}