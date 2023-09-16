using QuantumTek.QuantumUI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(OptionList))]
public class PopulateListWithCourses : MonoBehaviour
{
    OptionListImage listObj;
    QUI_OptionList listObjNoImg;
    public UnityEvent<int> onCourseSet;

    private List<CourseData> courses = new List<CourseData>();

    // Start is called before the first frame update
    void Start()
    {
        listObj = GetComponent<OptionListImage>();
        courses = Resources.LoadAll<CourseData>("Courses").ToList();
        if(listObj != null )
        {
            listObj.options = courses.Select(item => item.name).ToList();
            listObj.images = courses.Select(item => item.logo).ToList();
            listObj.ChangeOption(0);
        }
        listObjNoImg = GetComponent<QUI_OptionList>();
        if(listObjNoImg != null )
        {
            listObjNoImg.options.Clear();
            listObjNoImg.options = courses.Select(item => item.name).ToList();
            listObjNoImg.SetOption(0);
        }
        //onCourseSet.Invoke(0);
        
    }

    public void SetCourse(int index)
    {
        onCourseSet.Invoke(courses[index].sceneNum);
        SimManager.SetCourse(courses[index].name);
    }

}