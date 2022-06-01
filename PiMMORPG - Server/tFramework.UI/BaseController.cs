using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tFramework.UI
{
    public class BaseController<TView> where TView : BaseView
    {
        public static TView View { get; private set; }
        public static void SetView(TView view)
        {
            View = view;
        }
    }
}
