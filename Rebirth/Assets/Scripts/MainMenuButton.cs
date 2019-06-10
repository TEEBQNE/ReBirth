using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    public enum ButtonType
    {
        PLAY, 
        HELP, 
        QUIT, 
        BACK
    };

    public ButtonType Button;

    Color color;
    MeshRenderer meshRender;
    MenuScript menu;

    public void Start()
    {
        meshRender = GetComponent<MeshRenderer>();
        color = meshRender.material.color;
        menu = MenuScript.instance;
    }

    private void OnMouseOver()
    {

        if (!this.enabled)
            return;

        meshRender.material.color = Color.red;
    }

    private void OnMouseExit()
    {
        if (!this.enabled)
            return;

        meshRender.material.color = color;
    }

    private void OnMouseDown()
    {
        if (!this.enabled)
            return;

        switch (Button)
        {
            case ButtonType.PLAY:
                menu.playGame();
                break;
            case ButtonType.HELP:
                menu.Help();
                break;
            case ButtonType.QUIT:
                menu.QuitGame();
                break;
            case ButtonType.BACK:
                menu.Back();
                break;
        }
    }
}
