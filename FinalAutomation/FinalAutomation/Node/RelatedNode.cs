﻿using System;
using System.Collections.Generic;
using System.Linq;

public class RelatedNode : Node {
    private List<RelatedNode> nextNodes;
    private List<RelatedNode> previousNodes;

    public RelatedNode(Int32 id) : base(id) {
        nextNodes = new List<RelatedNode>();
        previousNodes = new List<RelatedNode>();
    }

    public void RelateWithNext(RelatedNode nextNode, Char relatedSymbol) {
        AddNext(nextNode, relatedSymbol);
        nextNode.AddPrevious(this, relatedSymbol);
    }
    public void RelateWithPrevious(RelatedNode previousNode, Char relatedSymbol) {
        AddPrevious(previousNode, relatedSymbol);
        previousNode.AddNext(this, relatedSymbol);
    }

    public void Merge(RelatedNode mergeNode) {

    }

    public List<RelatedNode> GetNextNodes() => new List<RelatedNode>(nextNodes);
    public List<RelatedNode> GetPreviousNodes() => new List<RelatedNode>(previousNodes);
    public List<RelatedNode> GetDescendantNodes() {
        var descidentNodes = new List<RelatedNode>();
        foreach(var node in nextNodes) {
            descidentNodes.Add(node);
            descidentNodes.AddRange(node.GetDescendantNodes());
        }
        return descidentNodes;
    }
    public List<RelatedNode> GetDescendantNodesAndSelf() {
        var resultNodes = new List<RelatedNode>();
        resultNodes.Add(this);
        resultNodes.AddRange(GetDescendantNodes());
        return resultNodes;
    }
    public IEnumerable<RelatedNode> GetStartNodesArriveToEnd(Int32 lengthPath) {
        var nodeIterator = new NodeIterator(this);
        while(nodeIterator.MoveNext())
            if(nodeIterator.Current.CanMoveToEnd(lengthPath))
                yield return nodeIterator.Current;
    }

    public RelatedNode GetRelatedNodeByOutput(Char outputSymbol) {
        if(!ExistOutputSymbol(outputSymbol))
            return null;
        return nextNodes.FirstOrDefault(el => el.ExistInputSymbol(outputSymbol));
    }
    public RelatedNode GetRelatedNodeByInput(Char inputSymbol) {
        if(!ExistInputSymbol(inputSymbol))
            return null;
        return previousNodes.FirstOrDefault(el => el.ExistOutputSymbol(inputSymbol));
    }

    public Int32 GetCountOfDescendantNodes() {
        Int32 resultCount = 0;
        foreach(var node in nextNodes) {
            resultCount++;
            resultCount += node.GetCountOfDescendantNodes();
        }
        return resultCount;
    }

    public Boolean ExistPathToEnd(String symbols) {
        RelatedNode current = this;
        foreach(var symbol in symbols) {
            current = current.nextNodes.FirstOrDefault(el => 
                current.GetIntersectSymbolsWithNext(el).Contains(symbol));
            if(current == null)
                return false;
        }
        return current.IsEnd();
    }
    public Boolean CanMoveToEnd(Int32 lengthPath) {
        Int32 counter = 0;
        var nodeIterator = new NodeIterator(this);
        nodeIterator.AddActionOnMoveNext(() => counter--);
        while(nodeIterator.MoveNext()) {
            if(counter == lengthPath) {
                if(nodeIterator.Current.IsEnd())
                    return true;
                nodeIterator.MoveBack();
            }
            counter++;
        }
        return counter == lengthPath;
    }

    private void AddNext(RelatedNode nextNode, Char relatedSymbol) {
        AddNew(nextNodes, nextNode);
        AddOutputSymbol(relatedSymbol);
    }
    private void AddPrevious(RelatedNode previousNode, Char relatedSymbol) {
        AddNew(previousNodes, previousNode);
        AddInputSymbol(relatedSymbol);
    }
    private void AddNew(List<RelatedNode> nodes, RelatedNode node) {
        if(!nodes.Contains(node))
            nodes.Add(node);
    }
}
