using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleSort : MonoBehaviour
{
    Button button;
    void Start(){
      button = GetComponent<Button>();
      button.onClick.AddListener(ButtonClickListener);
    }

    void Update(){
    }

    void ButtonClickListener(){
      Cubes cubes = FindObjectOfType<Cubes>();
      cubes.BubbleSort();
    }
}
