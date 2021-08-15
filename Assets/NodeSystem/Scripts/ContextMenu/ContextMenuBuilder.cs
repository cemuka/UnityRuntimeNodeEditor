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
        _root.children.Add(new ContextMenuData(name)
        {
            parent = _root,
        });
        return this;
    }

    public ContextMenuBuilder Add(string name, Action callback)
    {
        _root.children.Add(new ContextMenuData(name)
        {
            parent = _root,
            callback = callback
        });
        return this;
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