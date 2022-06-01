using System;
using System.Linq;
using System.Collections;
using UnityEngine;

using MarkLight;
using MarkLight.Views.UI;

using PiMMORPG.Client;
using PiMMORPG.Models;

using tFramework.Factories;
using Scripts.Local.UI.Helpers;
using Scripts.Network.Requests.GameClient;
using Scripts.Local.Inventory;
using Devdog.InventoryPro;

public class CharacterSelectionView : UIView
{
    public _int Total;
    
    public _int Current;
    public _bool CanPrevious, CanNext;
    public _bool CanPlay, CanCreate, CanDelete;
    public _string Message;
    public Character[] Models;

    MainMenu Menu { get { return FindObjectOfType<MainMenu>(); } }
    public CharacterHelper Helper { get; set; }

    public virtual void CurrentChanged(object sender, EventArgs e)
    {
        CanPlay.Value = Current != -1;
        CanDelete.Value = Current != -1;
        CanPrevious.Value = Current > 0;
        CanNext.Value = Current < Total - 1;

        if (Current != -1)
        {
            var Character = Models[Current];
            Message.Value = FormatCharacter(Character);
            Helper.Spawn(Character.IsFemale, Load);
        }
    }

    private void OnEnable()
    {
        Current.ValueSet += CurrentChanged;
        if (Models != null)
            Current.Value = 0;
    }

    private void OnDisable()
    {
        Current.ValueSet -= CurrentChanged;
    }

    public override void Activate()
    {
        base.Activate();

        DestroyImmediate(GetComponent<CharacterHelper>());
        CanCreate.Value = true;
    }

    public void UpdateCharacters(Character[] Models, int Maximum)
    {
        Menu.SwitchToCharacterSelection();
        if (Models.Length > 0)
        {
            this.Models = Models;
            Total.Value = Models.Length;
            
            if (Helper == null)
                Helper = FindObjectOfType<CharacterHelper>();
            
            Current.Value = 0;
            //Current.Value = Array.IndexOf(Models, Models.FirstOrDefault(C => C.ID == Client.Account.LastCharacter));

            Current.Value = Current >= Models.Length || Current == -1 && Total > 0 ? 0 : Current;
            CanCreate.Value = Models.Length < Maximum;

            Message.Value = Current < 0 ? "Nenhum personagem selecionado" : FormatCharacter(Models[Current.Value]);
        }
    }

    IEnumerator Load()
    {
        Helper.Current.tag = "Player";
        Helper.Current.gameObject.layer = LayerMask.NameToLayer("Local");

        var character = Models[Current];
        Helper.Current.CopyFrom(character.Style);

        if (character.Items != null)
        {
            foreach (var item in character.Items)
            {
                var e = ItemManager.database.items.First(i => i.ID == item.Info.InventoryID);
                Helper.Current.Equip(e as NetworkEquippableItem);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    string FormatCharacter(Character Character)
    {
        return string.Format("{0} - Lv.{1}", Character.Name, 1);

        //string Gender = string.Format("CharacterClass.C{0}", Convert.ToInt32(Character.Type));
        //return string.Format("{0} - {1}: {2}", Character.Name, LanguageManager.CurrentLanguage.GetValue("Messages.Class"), LanguageManager.CurrentLanguage.GetValue(Gender));
    }

    public void Play()
    {
        var current = PiBaseClient.Current;
        Character Character = Models[Current];
        current.Character = Character;

        var Packet = new SelectCharacterRequest();
        Packet.CharacterID = Character.ID;
        current.Socket.Send(Packet);
    }

    public void Next()
    {
        Current.Value++;
    }

    public void Previous()
    {
        Current.Value--;
    }

    public void CreateCharacter()
    {
        Menu.SwitchToCharacterCreation();
    }

    public void Back()
    {
        Helper.Despawn();
        Menu.SwitchToChannels();
    }
}