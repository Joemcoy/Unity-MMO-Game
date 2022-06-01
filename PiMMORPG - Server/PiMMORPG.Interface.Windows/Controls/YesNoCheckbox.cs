using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PiMMORPG.Interface.Windows.Controls
{
    public class YesNoCheckbox : CheckBox
    {
        public string YesString { get; set; } = "Yes";
        public string NoString { get; set; } = "No";

        public override string Text
        {
            get { return base.Text; }
            set { base.Text = Checked ? YesString : NoString; }
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            Text = Text;
        }
    }
}
