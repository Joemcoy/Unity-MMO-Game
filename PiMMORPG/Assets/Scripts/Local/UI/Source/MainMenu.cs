using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using MarkLight.Views.UI;
using MarkLight;

using Scripts.Local.UI;
using Scripts.Local.UI.Helpers;

public class MainMenu : UIView
{
    public ViewSwitcher MainSwitcher;

    //public _int width, height;

    public override void Initialize()
    {
        base.Initialize();
    }

    private void Update()
    {
        /*if (width.Value != Screen.width)
            width.Value = Screen.width;

        if (height.Value != Screen.height)
            height.Value = Screen.height;*/
    }

    public void SwitchToCharacterCreation() { MainSwitcher.SwitchTo(6); }
    public void SwitchToCharacterSelection() { MainSwitcher.SwitchTo(5); }
    public void SwitchToSettings() { MainSwitcher.SwitchTo(4); }
    public void SwitchToAbout() { MainSwitcher.SwitchTo(3); }
    public void SwitchToChannels() { MainSwitcher.SwitchTo(2); }
    public void SwitchToLogin() { MainSwitcher.SwitchTo(1); }
    public void SwitchToMenu()
    {
        var helper = FindObjectOfType<CharacterHelper>();
        helper.Despawn();

        MainSwitcher.SwitchTo(0);
    }
}