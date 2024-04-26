using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
public class Gh{
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

    public static Vector3 GetTopPosition(GameObject obj, float offset=0f){
        float y = CalculateBounds(obj).extents.y;
        return obj.transform.position + obj.transform.up * (y + offset);
    }

    public static Vector3 GetBottomPosition(GameObject obj, float offset=0f){
        float y = CalculateBounds(obj).extents.y;
        return obj.transform.position - obj.transform.up * (y + offset);
    }

    public static float GetHeight(GameObject obj){
        return CalculateBounds(obj).extents.y * 2;
    }

    public static GameObject GetChildWithTag(GameObject obj, string tag){
        foreach(Transform transform in obj.transform){
            if(transform.CompareTag(tag)){
                return transform.gameObject;
            }

            GameObject child = GetChildWithTag(transform.gameObject,tag);
            if(child) return child;

        }
        return null;
    }
    public static List<GameObject> GetChildrenWithTag(GameObject obj, string tag){
        List<GameObject> children = new();
        foreach(Transform transform in obj.transform){
            if(transform.CompareTag(tag)){
                children.Add(transform.gameObject);
            }
        }
        return children;
    }

    public static void SwapXZPosition(ref GameObject o1, ref GameObject o2){
        Vector3 pos1 = o1.transform.position;
        Vector3 pos2 = o2.transform.position;
        o1.transform.position = new(pos2.x,pos1.y,pos2.z);
        o2.transform.position = new(pos1.x,pos2.y,pos1.z);
    }

    

    public static void RotateAround(GameObject obj, Vector3 point, Vector3 axis, float angle){
        obj.transform.RotateAround(point,axis,angle);
    }

    public static Vector3 GetMidpoint(GameObject o1, GameObject o2){
        Vector3 v = o1.transform.position;
        Vector3 u = o2.transform.position;
        return new Vector3((v.x+u.x)/2,0,(v.z+u.z)/2);
    }

    public static Vector2 XZ(Vector3 vec){
        return new Vector2(vec.x,vec.z);
    }

    public static GameObject CloneGameObject(GameObject original){
        GameObject clone = GameObject.Instantiate(original);
        // Copy any other necessary properties from the original to the clone if needed
        return clone;
    }

}