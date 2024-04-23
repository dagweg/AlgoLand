using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsertionSort : MonoBehaviour
{
    Button button;
    void Start(){
      button = GetComponent<Button>();
      button.onClick.AddListener(ButtonClickListener);
    }


    void ButtonClickListener(){
      Cubes cubes = FindObjectOfType<Cubes>();
      cubes.InsertionSort();
    }
}
