using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public struct ButtonGroup
    {
        

    }


    public static MenuScript instance;
    public GameObject Buttons;
    public List<GameObject> GroupButtons;

    private int menuIndex = 0;


    public void Awake()
    {
        instance = this;
    }

    public void playGame()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Back()
    {
        StartCoroutine(rotate(5));
        toggleButtons(false, menuIndex);
        menuIndex--;
        toggleButtons(true, menuIndex);
    }

    public void Help()
    {
        StartCoroutine(rotate(-5));
        toggleButtons(false, menuIndex);
        menuIndex++;
        toggleButtons(true, menuIndex);
    }

    public void toggleButtons(bool tof, int index)
    {
        int size = GroupButtons[index].transform.childCount;
        for(int i = 0; i < size; i++)
        {
            GroupButtons[index].transform.GetChild(i).GetComponent<MainMenuButton>().enabled = tof;
        }
    }
    
    IEnumerator rotate (float degree)
    {
        float totalDegree = 0;

        while(Mathf.Abs(totalDegree) < 90)
        {
            Buttons.transform.Rotate(Vector3.up, degree);
            totalDegree += degree;
            yield return null;
        }

        if(Mathf.Abs(totalDegree) != 90)
        {
            if(degree < 0)
                Buttons.transform.Rotate(Vector3.up, (totalDegree + 90) * -1);
            else
                Buttons.transform.Rotate(Vector3.up, (totalDegree - 90) * -1);
        }
    }
}
