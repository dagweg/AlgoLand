using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cubes : MonoBehaviour
{

    List<GameObject> cubz = new();
    List<GameObject> cubeUis = new();
    [SerializeField] GameObject cube;        
    [SerializeField] Camera mainCamera;
    private GameObject textObject;
    [SerializeField] float fontSize = 0.7f;

    [SerializeField] float cubeTextVerticalSpacing = 1f;
    [SerializeField] Vector3 spacing;
    [SerializeField] float heightDifference;

    Color defaultColor = Color.black;

    void Start()
    {
        Cursor.visible = true;
  
        PopulateCubes();
        PopulateCubesUI();
    }

    void Update(){
        foreach(GameObject uigo in cubeUis){
            Vector3 lookDirection = uigo.transform.position - mainCamera.transform.position;
            uigo.transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }


    public void PopulateCubes(){
        DestroyGameObjects(cubz);
        
        cube.SetActive(true);
        Vector3 curCubePos = cube.transform.position;
        
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

        cube.SetActive(false);
    }

    public void PopulateCubesUI(){

        DestroyGameObjects(cubeUis);

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

            cubeUis.Add(canvasObjInst);
        }
    }

    public void DestroyGameObjects(List<GameObject> list){
        foreach(GameObject go in list) Destroy(go);
        list.Clear();
    }

    public void BubbleSort() {
        int n = cubz.Count;
        StartCoroutine(BubbleSortCoroutine(n));
        
    }

    public void InsertionSort(){
        int n = cubz.Count;
        StartCoroutine(InsertionSortCoroutine(n));
    }

    public void SelectionSort(){
        int n = cubz.Count;
        StartCoroutine(SelectionSortCoroutine(n));
    }

    public IEnumerator MergeSort(){
        PrintCubeHeights();
        yield return StartCoroutine(MergeSortCoroutine(cubz,0,cubz.Count));
        yield return StartCoroutine(ChangeColor(Color.Lerp(Color.white,Color.black,0.5f)));
        FindObjectOfType<SortedAnimationUI>().PlayAnimation();
        PrintCubeHeights();
    }

    IEnumerator MergeSortCoroutine(List<GameObject> list, int start, int end){ 
        if(start >= end - 1) yield break;
        int mid = (start + end) / 2;
        yield return StartCoroutine(MergeSortCoroutine(list, start, mid));
        yield return StartCoroutine(MergeSortCoroutine(list, mid, end));
        yield return StartCoroutine(Merge(list, start, mid, end));
    }
    
    IEnumerator Merge(List<GameObject> list, int start, int mid, int end){
        List<GameObject> merged = new(end - start);
        int left = start;
        int right = mid;         

        while(left < mid && right < end){
            if(Gh.GetHeight(list[left]) < Gh.GetHeight(list[right])){
                merged.Add(CloneGameObject(list[left++]));
            } else {
                merged.Add(CloneGameObject(list[right++]));
            }
        }
        
        while(left < mid){
            merged.Add(CloneGameObject(list[left++]));
        }
        while(right < end){
            merged.Add(CloneGameObject(list[right++]));
        }
        
        for(int i = 0; i < merged.Count; i++){
            Vector3 l = list[start+i].transform.position;
            merged[i].transform.position = new Vector3(l.x,merged[i].transform.position.y,l.z);
            StartCoroutine(ChangeColor(list[start+i],Color.magenta,0));
            StartCoroutine(ChangeColor(merged[i],Color.cyan,0));
            yield return new WaitForSeconds(0.1f);
            Destroy(list[start+i]);
            list[start+i] = merged[i];
            StartCoroutine(ChangeColor(list[start+i],defaultColor,0));
            StartCoroutine(ChangeColor(merged[i],defaultColor,0));
        }

        yield return StartCoroutine(ChangeColor(merged,Color.yellow));
        yield return StartCoroutine(ChangeColor(merged,default,0));
    }
    GameObject CloneGameObject(GameObject original){
        GameObject clone = Instantiate(original);
        // Copy any other necessary properties from the original to the clone if needed
        return clone;
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
        yield return StartCoroutine(ChangeColor(Color.green));
        FindObjectOfType<SortedAnimationUI>().PlayAnimation();
    }

    IEnumerator InsertionSortCoroutine(int n){
        GameObject key = null;
        for(int i = 1; i < n; i++){
            int j = i;
            key = cubz[j];
            while (j > 0 && Gh.GetHeight(cubz[j-1]) > Gh.GetHeight(key)){
                //Swap here
                yield return StartCoroutine(SwapAndRotate(j,j-1));
                j--;
            }
        }
        yield return StartCoroutine(ChangeColor(Color.Lerp(Color.green,Color.blue,0.1f)));
        FindObjectOfType<SortedAnimationUI>().PlayAnimation();
    }

    IEnumerator SelectionSortCoroutine(int n){
        for(int i= 0; i< n;i++){
            for(int j =i+1; j<n;j++){
                if(Gh.GetHeight(cubz[i]) > Gh.GetHeight(cubz[j])){
                    // swap here
                    yield return StartCoroutine(SwapAndRotate(i,j));
                }
            }
        }
        yield return StartCoroutine(ChangeColor(Color.Lerp(Color.magenta,Color.black,0.1f)));
        FindObjectOfType<SortedAnimationUI>().PlayAnimation();
    }

    IEnumerator ChangeColor(Color color, float wait = 0.15f){
        foreach(GameObject go in cubz){
            go.GetComponent<Renderer>().material.color = color;
            yield return new WaitForSeconds(wait);
        }
        yield return null;
    }
    

    IEnumerator ChangeColor(GameObject obj, Color color, float wait=0.15f){
        obj.GetComponent<Renderer>().material.color = color;
        yield return new WaitForSeconds(wait);
    }
    IEnumerator ChangeColor(List<GameObject> objs, Color color, float wait=0.15f){
        foreach(GameObject obj in objs){
            obj.GetComponent<Renderer>().material.color = color;
            yield return new WaitForSeconds(wait);
        }
    }

    IEnumerator SwapAndRotate(int i, int j){
        Material mat1 = cubz[i].GetComponent<Renderer>().material;
        Material mat2 = cubz[j].GetComponent<Renderer>().material;
        mat1.color = Color.magenta;
        mat2.color = Color.magenta;
        yield return null;
        yield return StartCoroutine(RotateCoroutine(i,j));
        yield return StartCoroutine(SwapCoroutine(i,j));
        mat1.color = defaultColor;
        mat2.color = defaultColor;
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


    public void PrintCubeHeights(){
        string awt = "";
        foreach(GameObject go in cubz){
            awt += Gh.GetHeight(go) + ",";
        }
        Debug.Log(awt);
        Debug.Log("");
    }
}