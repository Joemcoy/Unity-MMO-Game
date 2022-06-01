using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MarkLight;
using UnityEngine;

using Application = Scripts.Local.Application;
using tFramework.Data.Manager;

public class SettingsView : View
{
    public _int MaxResolutions, MaxQualities;
    public ObservableList<Resolution> Resolutions = new ObservableList<Resolution>();
    public ObservableList<string> Qualities = new ObservableList<string>();
    public _string ResolutionString, QualityString;
    public _bool FullScreen;
    public _bool AntiAliasing, Bloom, Fog, AmbientOcclusion;
    public _bool DepthOfField, MotionBlur, ColorGrading;
    public _bool ChromaticAberration, UserLut, EyeAdaption;
    public _bool ScreenSpaceReflection;

    #region View Control
    public override void Initialize()
    {
        base.Initialize();

        UpdateResolutions();
        UpdateQualities();
    }

    public override void Activate()
    {
        base.Activate();

        if (Application.Configuration != null)
        {
            var graphics = Application.Configuration.Graphics;
            AntiAliasing.Value = graphics.AntiAliasing;
            Bloom.Value = graphics.Bloom;
            Fog.Value = graphics.Fog;
            AmbientOcclusion.Value = graphics.AmbientOcclusion;
            DepthOfField.Value = graphics.DepthOfField;
            MotionBlur.Value = graphics.MotionBlur;
            ColorGrading.Value = graphics.ColorGrading;
            ChromaticAberration.Value = graphics.ChromaticAberration;
            UserLut.Value = graphics.UserLut;
            EyeAdaption.Value = graphics.EyeAdaption;
            ScreenSpaceReflection.Value = graphics.ScreenSpaceReflection;
        }

        UpdateResolutions();
        UpdateQualities();
    }

    void UpdateResolutions()
    {
        var Configuration = Application.Configuration;
        Resolution Current = Screen.currentResolution;

        if (Configuration != null)
        {
            Current = Screen.currentResolution;
            FullScreen.Value = Screen.fullScreen;
        }
        else
            FullScreen.Value = Screen.fullScreen;

        Resolutions.Clear();
        foreach (var Res in Screen.resolutions)
            Resolutions.Add(Res);

        MaxResolutions.Value = Resolutions.Count - 1;
        QueueChangeHandler("SelectRes");
        ResolutionChanged();
    }

    void SelectRes()
    {
        Resolutions.SelectedIndex = Resolutions.IndexOf(Screen.currentResolution);
        if (Resolutions.SelectedIndex == -1)
            Resolutions.SelectedIndex = 0;
    }

    void ResolutionChanged()
    {
        var Res = Resolutions.SelectedItem;
        ResolutionString.Value = string.Format("{0}x{1}x{2}hz", Res.width, Res.height, Res.refreshRate + 1);
    }

    void UpdateQualities()
    {
        Qualities.Clear();
        foreach (var Level in QualitySettings.names)
            Qualities.Add(Level);

        QueueChangeHandler("SelectQuality");

        MaxQualities.Value = Qualities.Count - 1;
        QualityChanged();
    }

    void SelectQuality()
    {
        Qualities.SelectedIndex = QualitySettings.GetQualityLevel();
        if (Qualities.SelectedIndex == -1)
            Qualities.SelectedIndex = 0;
    }

    void QualityChanged()
    {
        QualityString.Value = Qualities.SelectedItem;
    }
    #endregion

    public void ApplySettings()
    {
        /*var video = Application.Configuration.Video;
        video.ScreenWidth = Resolutions.SelectedItem.width;
        video.ScreenHeight = Resolutions.SelectedItem.height;
        video.RefreshRate = Resolutions.SelectedItem.refreshRate;
        video.FullScreen = FullScreen.Value;
        video.QualityLevel = Qualities.SelectedIndex;*/

        var graphics = Application.Configuration.Graphics;
        graphics.AntiAliasing = AntiAliasing;
        graphics.Bloom = Bloom;
        graphics.Fog = Fog;
        graphics.AmbientOcclusion = AmbientOcclusion;
        graphics.DepthOfField = DepthOfField;
        graphics.MotionBlur = MotionBlur;
        graphics.ColorGrading = ColorGrading;
        graphics.ChromaticAberration = ChromaticAberration;
        graphics.UserLut = UserLut;
        graphics.EyeAdaption = EyeAdaption;
        graphics.ScreenSpaceReflection = ScreenSpaceReflection;

        Screen.SetResolution(Resolutions.SelectedItem.width, Resolutions.SelectedItem.height, FullScreen.Value, Resolutions.SelectedItem.refreshRate);
        QualitySettings.SetQualityLevel(Qualities.SelectedIndex, true);

        if (!ConfigurationManager.Save(Application.Configuration))
            Debug.LogWarning("Falha ao salvar!");
    }

    public void SwitchToMenu()
    {
        var Menu = FindObjectOfType<MainMenu>();
        Menu.SwitchToMenu();
    }
}