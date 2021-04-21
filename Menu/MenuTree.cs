using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Menu
{
    public class MenuTree
    { 
        public string Name { get; set; }
        public List<MenuTree> Children { get; set; }
        public EventHandler ClickHandler { get; set; }
        public ElementStatus Status { get; set; }

        public MenuTree(string name, int status, List<MenuTree> children = null, EventHandler clickHandler = null)
        {
            Name = name;
            Status = (ElementStatus)status;
            Children = children ?? new List<MenuTree>();
            ClickHandler = clickHandler;
        }

        public static explicit operator ToolStripMenuItem(MenuTree mt) => mt.ConvertToMenuItem();

        private ToolStripMenuItem ConvertToMenuItem()
        {
            var item = new ToolStripMenuItem(Name);

            if (ClickHandler != null)
            {
                item.Click += ClickHandler;
            }

            switch (Status)
            {
                case ElementStatus.Enabled:
                    item.DropDownItems.AddRange(Children.Select(x => (ToolStripMenuItem)x).ToArray());
                    break;
                case ElementStatus.Disabled:
                    item.Enabled = false;
                    break;
                case ElementStatus.Hiden:
                    item.Visible = false;
                    break;
            }

            return item;
        }
    }
}
