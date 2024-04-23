using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class randomize : MonoBehaviour
{
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(RandomizeClick);
    }

    void RandomizeClick(){
        Cubes cubes = FindObjectOfType<Cubes>();
        cubes.PopulateCubes();
        cubes.PopulateCubesUI();
    }

}
