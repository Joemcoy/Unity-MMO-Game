using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MORPH3D;
using MarkLight;

using PiMMORPG.Models;

namespace Scripts.Local.Morph
{
    public class MorphStyle
    {
        public bool Changed { get; private set; }

        public M3DCharacterManager Manager;// { get; set; }
        public string Category;
        public string Morph;
        public string Alternative;
        public _float Minimum, Maximum, Value;
        Action<CharacterStyle, float> Setter;
        Func<CharacterStyle, float> Getter;

        public MorphStyle(string Category, string Morph, Action<CharacterStyle, float> Setter, Func<CharacterStyle, float> Getter) : this(Category, Morph, Setter, Getter, 0, 100) { }
        public MorphStyle(string Category, string Morph, Action<CharacterStyle, float> Setter, Func<CharacterStyle, float> Getter, float Minimum, float Maximum)
        {
            Changed = false;

            this.Category = Category;
            this.Morph = Morph;
            this.Setter = Setter;
            this.Getter = Getter;

            this.Minimum = new _float();
            this.Minimum.Value = Minimum;

            this.Maximum = new _float();
            this.Maximum.Value = Maximum;

            Value = new _float();
            Value.ValueSet += Value_ValueSet;
        }

        public void Reset()
        {
            Changed = false;
        }

        private void Value_ValueSet(object sender, EventArgs e)
        {
            if (!Changed)
                Changed = true;
            Manager.SetBlendshapeValue(Morph, Value > 0 ? Value : 0f);

            if (!string.IsNullOrEmpty(Alternative))
                Manager.SetBlendshapeValue(Alternative, Value < 0 ? -Value : 0f);
            Manager.SyncAllBlendShapes();
        }

        public void CopyFrom(CharacterStyle Model)
        {
            Value.Value = Getter(Model);
        }

        public void CopyTo(CharacterStyle Model)
        {
            Setter(Model, Value.Value);
        }
    }
}
