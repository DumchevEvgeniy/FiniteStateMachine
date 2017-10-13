using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NodeIterator : IEnumerator<RelatedNode> {
    private RelatedNode startNode;
    private RelatedNode current;

    private List<RelatedNode> nodes;
    private List<RelatedNode> closeNodes;

    private Action actionOnMoveNext = null;

    public NodeIterator(RelatedNode startNode) {
        this.startNode = startNode;
        Reset();
    }

    public void Dispose() { }
    public void Reset() {
        nodes = new List<RelatedNode>();
        closeNodes = new List<RelatedNode>();
    }
    public void AddActionOnMoveNext(Action actionOnMoveNext) => 
        this.actionOnMoveNext += actionOnMoveNext;

    public Boolean MoveNext() {
        if(current == null) {
            if(closeNodes.Contains(startNode))
                return false;
            current = startNode;
            return true;
        }
        nodes.Add(current);
        while(true) {
            var nextNode = current.GetNextNodes().FirstOrDefault(el => !closeNodes.Contains(el));
            if(nextNode != null) {
                current = nextNode;
                return true;
            }
            MoveBack();
            if(current == null)
                return false;
        }
    }
    public Boolean MoveBack() {
        if(current == null)
            return false;
        closeNodes.Add(current);
        if(nodes.IsEmpty())
            current = null;
        else {
            nodes.RemoveAt(nodes.Count - 1);
            current = nodes.Count == 0 ? null : nodes.Last();
        }
        if(actionOnMoveNext != null)
            foreach(var action in actionOnMoveNext.GetInvocationList())
                ((Action)action)();
        return true;
    }

    public RelatedNode Current => current;
    Object IEnumerator.Current => Current;
}
