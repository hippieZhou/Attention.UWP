using HENG.Models;
using System;

namespace HENG.Helpers
{
    public static class ModelHelper
    {
        public static void ParseModel(this object model, Action<BingItem> bingAction, Action<PicsumItem> PicsumAction, Action<PaperItem> PaperActon)
        {
            var type = model.GetType();

            if (typeof(BingItem) == type)
            {
                bingAction((BingItem)model);
            }
            else if (typeof(PicsumItem) == type)
            {
                PicsumAction((PicsumItem)model);
            }
            else if (typeof(PaperItem) == type)
            {
                PaperActon((PaperItem)model);
            }
        }
    }
}
