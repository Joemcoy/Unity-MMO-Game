using System;
using System.Collections;
using System.Linq;

using MarkLight;
using MarkLight.Views.UI;

using UnityEngine;
using UnityEngine.EventSystems;

using PiMMORPG.Client;
using tFramework.Factories;

using Scripts.Local.Morph;
using Scripts.Local.Helper;
using Scripts.Local.UI.Helpers;
using Scripts.Network.Requests.GameClient;
using tFramework.Extensions;

public class CreateCharacterView : UIView
{
    public InputField txtName;
    public _bool CanBack, CanCreate, MorphPrevious, MorphNext;
    public _string Message, MorphName;

    [ChangeHandler("GenderChanged")]
    public _bool Gender;

    public ObservableList<string> Hairs, HMaterials;
    
    public TabPanel tbStyles;

    public ViewSwitcher CreationSwitcher, MorphSwitcher;
    public CharacterHelper Helper { get { return FindObjectOfType<CharacterHelper>(); } }
    MainMenu Menu;
    EventSystem System;
    bool HUpdating = false;

    void Awake()
    {
        System = EventSystem.current;
        Menu = Parent.GetComponent<MainMenu>();
    }
    
    public MorphStyle GetMorph(string Name)
    {
        return Helper.Current.Styles.First(s => s.Morph == Name);
    }

    public override void Initialize()
    {
        base.Initialize();

        Hairs = new ObservableList<string>();
        Hairs.ListChanged += Hairs_ListChanged;
        HMaterials = new ObservableList<string>();

        CanBack.Value = true;
        CanCreate.Value = false;
        MorphPrevious.Value = false;
        UpdateMorphButtons();
    }

    private void Hairs_ListChanged(object sender, ListChangedEventArgs e)
    {
        Debug.LogFormat("Event action: {0}", e.ListChangeAction);
        if (e.ListChangeAction == ListChangeAction.Clear)
            Debug.Log("Cleared list!");
        else
            foreach(var item in Hairs.Skip(e.StartIndex).Take(e.EndIndex - e.StartIndex))
                switch (e.ListChangeAction)
                {
                    case ListChangeAction.Add:
                        Debug.LogFormat("Added item {0}!", item);
                        break;
                    case ListChangeAction.Remove:
                        Debug.LogFormat("Added item {0}!", item);
                        break;
                }
        }

    void UpdateMorphButtons()
    {
        bool has = false;
        var name = ResourceDictionary.GetValue("Localization", MorphSwitcher.ActiveView.Id, out has);

        MorphName.Value = has ? name : "Unknown";
        MorphNext.Value = MorphSwitcher.ActiveView != MorphSwitcher.Last();
        MorphPrevious.Value = MorphSwitcher.ActiveView != MorphSwitcher.First();
    }

    public void PreviousMorph()
    {
        MorphSwitcher.Previous();
        UpdateMorphButtons();
    }

    public void NextMorph()
    {
        MorphSwitcher.Next();
        UpdateMorphButtons();
    }

    public void Advanced()
    {
        CreationSwitcher.SwitchTo(2);
    }

    public void Hair()
    {
        CreationSwitcher.SwitchTo(1);
    }

    public void General()
    {
        CreationSwitcher.SwitchTo(0);
    }

    public void TabChanged()
    {
        if (EditorHelper.IsEditor)
        {

        }
    }

    public void Show()
    {
        if (EditorHelper.IsPlaying)
        {
            Helper.Spawn(Gender.Value, Load);
        }
    }

    public void Hide()
    {
        if (EditorHelper.IsPlaying)
        {
            Helper.Despawn();
        }
    }

    public void NameChanged()
    {
        txtName.Text.Value = txtName.Text.Value.Replace(" ", "");
        CanCreate.Value = txtName.Text.Value.Length > 0;
    }

    public void GenderChanged()
    {
        if (EditorHelper.IsPlaying && IsActive)
        {
            FindObjectOfType<CharacterHelper>().Spawn(Gender.Value, Load);
        }
    }

    public void HairChanged()
    {
        if (EditorHelper.IsPlaying && IsActive && !HUpdating)
        {
            var logger = LoggerFactory.GetLogger(this);
            logger.LogInfo("A:{0} H:{1}", IsActive, !HUpdating);
            logger.LogInfo("SH: {0}", Hairs.SelectedIndex);

            Helper.SetHair(Hairs.SelectedIndex - 1, () => LoadMaterials());
        }
    }

    public void HMChanged()
    {
        if (EditorHelper.IsPlaying && IsActive && Helper.HairIndex > -1)
        {
            Helper.Current.GetComponent<HairSetter>().Current.Current = Convert.ToInt16(HMaterials.SelectedIndex);
        }
    }

    public IEnumerator Load()
    {
        HUpdating = true;
        Helper.Current.tag = "Player";
        Helper.Current.gameObject.layer = LayerMask.NameToLayer("Local");

        /*Hairs.Clear();
        Hairs.Add("Bald");
        Hairs.AddRange(Helper.Current.GetComponent<HairSetter>().Hairs.Select(H => H.name.Replace("Hair", string.Empty)));*/

        var hairs = Helper.Current.GetComponent<HairSetter>().Hairs.Select(h => char.ToUpper(h.First()) + h.Substring(1).ToLower());
        hairs.ForEach(h => Debug.LogFormat("Triggered {0}", h));
        Hairs.Clear();
        Hairs.Add("Bald");
        Hairs.AddRange(hairs);
        yield return null;

        //LoadMaterials();
        QueueChangeHandler("SelectHFirst");
    }

    public void SelectHFirst()
    {
        Hairs.SelectedIndex = 0;
        Helper.UpdateHair();
        SelectMFirst();
    }

    public void SelectMFirst()
    {
        HMaterials.SelectedIndex = 0;
        HUpdating = false;
    }

    public void LoadMaterials()
    {
        HMaterials.Clear();

        var Helper = FindObjectOfType<CharacterHelper>();
        if (Helper.HairIndex == -1)
            HMaterials.Add("None");
        else
        {
            var setter = Helper.Current.GetComponent<HairSetter>();
            HMaterials.AddRange(setter.Current.Colors.Select(c => char.ToUpper(c.First()) + c.Substring(1).ToLower()));
        }
        QueueChangeHandler("SelectMFirst");
    }

    public void Create()
    {
        CanCreate.Value = false;
        CanBack.Value = false;

        var Helper = FindObjectOfType<CharacterHelper>();
        var Packet = new CreateCharacterRequest();
        Packet.Name = txtName.Text;
        Packet.IsFemale = Gender;
        Packet.Style = new PiMMORPG.Models.CharacterStyle();

        Helper.Current.CopyTo(Packet.Style);

        var client = PiBaseClient.Current;
        client.Socket.Send(Packet);
    }

    public void Reset()
    {
        var Helper = FindObjectOfType<CharacterHelper>();
        if (EditorHelper.IsPlaying && Helper != null && Helper.Current != null)
        {
            Helper.Current.View = this;
            Helper.Current.Reset();
        }
    }

    public void Back()
    {
        Helper.Despawn();
        Menu.SwitchToCharacterSelection();
    }

    public string GetResource(string Field)
    {
        if (string.IsNullOrEmpty(Field))
            return "Empty?";
        else
        {
            bool Has;
            var Value = ResourceDictionary.GetValue("Localization", Field, out Has);
            return !Has ? "Not found!" : Value;
        }
    }
}