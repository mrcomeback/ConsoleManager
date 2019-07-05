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
            int focusedIndex = listViews.FindIndex(i => i.Focused == true);
            
            if (key.Key == ConsoleKey.LeftArrow && focusedIndex > 0)
            {
                listViews[focusedIndex].Focused = false;
                listViews[focusedIndex - 1].Focused = true;
            }
            else if (key.Key == ConsoleKey.RightArrow && focusedIndex < listViews.Count - 1)
            {
                listViews[focusedIndex].Focused = false;
                listViews[focusedIndex + 1].Focused = true;
            }
            return listViews;
        }
    }
}
