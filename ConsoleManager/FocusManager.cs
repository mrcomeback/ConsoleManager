using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleManager
{
    class FocusManager
    {
        

        public List<ListView> ChangeFocus(List<ListView> listViews, ConsoleKeyInfo key)
        {
            int focusedIndex = listViews.FindIndex(i => i._focused == true);
            
            if (key.Key == ConsoleKey.LeftArrow && focusedIndex > 0)
            {
                listViews[focusedIndex]._focused = false;
                listViews[focusedIndex - 1]._focused = true;
            }else if (key.Key == ConsoleKey.RightArrow && focusedIndex < listViews.Count - 1)
            {
                listViews[focusedIndex]._focused = false;
                listViews[focusedIndex + 1]._focused = true;
            }

            return listViews;
        }
    }
}
