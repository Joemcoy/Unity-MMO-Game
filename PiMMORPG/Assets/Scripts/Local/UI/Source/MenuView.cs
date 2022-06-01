using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MarkLight.Views.UI;
using tFramework.Factories;
using UnityEngine;

public class MenuView : UIView
{
    public void SwitchToLogin()
    {
        var Menu = FindObjectOfType<MainMenu>();
        Menu.SwitchToLogin();
    }

    public void SwitchToSettings()
    {
        var Menu = FindObjectOfType<MainMenu>();
        Menu.SwitchToSettings();
    }

    public void Quit()
    {
        SingletonFactory.DestroyAll();
        Application.Quit();
    }
}