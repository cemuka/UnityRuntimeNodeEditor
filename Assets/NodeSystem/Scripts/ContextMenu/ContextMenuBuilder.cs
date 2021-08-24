using System;
using System.Collections.Generic;

public class ContextMenuBuilder
{
    private ContextMenuData _root;

    public ContextMenuBuilder()
    {
        _root = new ContextMenuData("");
    }

    public ContextMenuBuilder Add(string name)
    {
        //_root.children.Add(new ContextMenuData(name)
        //{
        //    parent = _root,
        //});
        BuildHierarchy(name, null);
        return this;
    }

    public ContextMenuBuilder Add(string name, Action callback)
    {
        //_root.children.Add(new ContextMenuData(name)
        //{
        //    parent = _root,
        //    callback = callback
        //});
        BuildHierarchy(name, callback);
        return this;
    }
    public void BuildHierarchy(string path, Action callback)
    {
        path = path.Replace("\\", "/");
        string[] split = path.Split('/');
        ContextMenuData menu_item = _root;
        int index = 0;

        while (menu_item != null && index < split.Length)
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
                var new_node = new ContextMenuData(split[index]) { parent = menu_item };
                new_node.callback = callback;
                menu_item.children.Add(new_node);
                menu_item = new_node;
                ++index;
                found = true;
            }
        }
        return;
    }

    public ContextMenuBuilder AddChild(ContextMenuData child)
    {
        int lastIndex = _root.children.Count - 1;
        var targetNode = _root.children[lastIndex];
        child.parent = targetNode;
        targetNode.children.Add(child);
        return this;
    }

    public ContextMenuData Build()
    {
        return _root;
    }
}


public class ContextMenuData
{
    public string name;
    public ContextMenuData parent;
    public List<ContextMenuData> children;
    public Action callback;

    public bool IsRoot => parent == null;
    public int Level => IsRoot ? 0 : parent.Level + 1;

    public ContextMenuData(string name)
    {
        this.name = name;
        children = new List<ContextMenuData>();
    }





}