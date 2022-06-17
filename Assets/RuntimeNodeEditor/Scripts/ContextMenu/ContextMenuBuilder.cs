using System;
using System.Collections.Generic;
namespace RuntimeNodeEditor
{
    public class ContextMenuBuilder
    {
        private ContextItemData _root;

        public ContextMenuBuilder()
        {
            _root = new ContextItemData("");
        }

        public ContextMenuBuilder Add(string name, Action callback)
        {
            BuildHierarchy(name).callback = callback;
            return this;
        }

        public ContextItemData Build()
        {
            return _root;
        }

        private ContextItemData BuildHierarchy(string path)
        {
            path = path.Replace("\\", "/");
            string[] split = path.Split('/');
            ContextItemData menu_item = _root;
            int index = 0;

            while (index < split.Length)
            {
                bool found = false;


                for (int i = 0; i < menu_item.children.Count; ++i)
                {
                    if (menu_item.children[i].name == split[index])
                    {
                        menu_item = menu_item.children[i];
                        ++index;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    var new_menu_item = new ContextItemData(split[index]) { parent = menu_item };
                    menu_item.children.Add(new_menu_item);
                    menu_item = new_menu_item;
                    ++index;
                    found = true;
                }
            }
            return menu_item;
        }
    }

    public class ContextItemData
    {
        public string name;
        public ContextItemData parent;
        public List<ContextItemData> children;
        public Action callback;

        public bool IsRoot => parent == null;
        public int Level => IsRoot ? 0 : parent.Level + 1;

        public bool IsTerminal => children.Count == 0;

        public ContextItemData(string name)
        {
            this.name = name;
            children = new List<ContextItemData>();
        }
    }
}