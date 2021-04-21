using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Menu
{
    public class MenuFactory
    {
        private readonly List<MenuTree> _items;

        public ToolStripMenuItem[] MenuItems => _items.Select(x => (ToolStripMenuItem) x).ToArray();

        public MenuFactory(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var sr = new StreamReader(fs, Encoding.UTF8);

            _items = Parse(
                sr.ReadToEnd()
                    .Split("\n")
                    .Select(x => x.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries))
                    .ToArray()
                );
        }

        private List<MenuTree> Parse(string[][] data)
        {
            var res = new List<MenuTree>();

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Length == 3)
                {
                    var parentLayoutIndex = int.Parse(data[i][0]);
                    var extraMenu = 
                        data
                            .Skip(i + 1)
                            .TakeWhile(x => int.Parse(x[0]) > parentLayoutIndex)
                            .ToArray();

                    res.Add(new MenuTree(data[i][1], int.Parse(data[i][2]), Parse(extraMenu)));

                    i += extraMenu.Length;
                }
                else
                {
                    var message = data[i][3];
                    res.Add(new MenuTree(data[i][1], int.Parse(data[i][2]), clickHandler:(_, __) => MessageBox.Show(message)));
                }
            }

            return res;
        }
    }
}
