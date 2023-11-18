using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPopulator : MonoBehaviour
{
    public GameObject optionPrefab;
    // Start is called before the first frame update
    void Start()
    {
        CourseData course = SimManager.CurrentCourse;
        List<string> options = SimManager.CurrentCourse.getOptionsList();
        foreach (string option in options)
        {
            GameObject obj = GameObject.Instantiate(optionPrefab,transform);
            //obj.GetComponentInChildren<TextMeshProUGUI>().text = option;
            obj.GetComponentInChildren<Text>().text = option;
            Toggle toggleButton = obj.GetComponentInChildren<Toggle>();
            toggleButton.onValueChanged.AddListener((val) => course.setOptions(option, val));
        }
    }

}
