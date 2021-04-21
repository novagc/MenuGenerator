using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menu;

namespace Lab2
{
    public partial class MenuGenerator : Form
    {
        public MenuGenerator()
        {
            InitializeComponent();
#if DEBUG
            ExplicitLinking();
#else
            ImplicitLinking();
#endif
        }

        public void ExplicitLinking()
        {
            var mf = new MenuFactory("menu.tree");
            menuBar.Items.AddRange(mf.MenuItems);
        }

        public void ImplicitLinking()
        {
            var dll = Assembly.LoadFile($"{Directory.GetCurrentDirectory()}/../../../../Menu/bin/Debug/netcoreapp3.1/Menu.dll");
            var type = dll.GetType("Menu.MenuFactory");
            var obj = type.GetConstructors()[0].Invoke(new object?[] {"menu.tree"});
            menuBar.Items.AddRange((ToolStripMenuItem[])type.GetMethod("get_MenuItems").Invoke(obj, new object?[]{}));
        }
    }
}
