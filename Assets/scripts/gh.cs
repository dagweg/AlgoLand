using System;
using UnityEngine;
public class gh{
    // Return -1 if o1 is greater, 0 if equal, 1 otherwise
    public static int CompareSize(GameObject o1, GameObject o2){
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

    public static Bounds CalculateBounds(GameObject obj){
        Renderer rend = obj.GetComponent<Renderer>();
        if(rend) return rend.bounds;
        Collider coll = obj.GetComponent<Collider>();
        if(coll) return coll.bounds;

        Debug.Log("Bounds coundnt be calculated.. falling back to default");
        return new Bounds(obj.transform.position,Vector3.one);
    }

}