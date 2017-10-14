using System;
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
        MergePreviousNodes(mergeNode);
        MergeNextNodes(mergeNode);
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
    public Boolean EqualByOutput(RelatedNode otherNode) {
        if(otherNode == null)
            return false;
        if(nextNodes.Count != otherNode.nextNodes.Count)
            return false;
        var nextNodesCurrentNode = GetNextNodes().OrderBy(el => el.Id);
        var nextNodesOtherNode = otherNode.GetNextNodes().OrderBy(el => el.Id);
        for(Int32 index = 0; index < nextNodes.Count; index++) {
            var nextNodeCurrentNode = nextNodesCurrentNode.ElementAt(index);
            var nextNodeOtherNode = nextNodesOtherNode.ElementAt(index);
            if(nextNodeCurrentNode.Id != nextNodeOtherNode.Id)
                return false;
            var firstSymbols = GetIntersectSymbolsWithNext(nextNodeCurrentNode);
            var secondSymbols = otherNode.GetIntersectSymbolsWithNext(nextNodeOtherNode);
            if(firstSymbols.Count() != secondSymbols.Count())
                return false;
            if(firstSymbols.Union(secondSymbols).Count() != firstSymbols.Count())
                return false;
        }
        return true;
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
    private void MergePreviousNodes(RelatedNode mergeNode) {
        foreach(var node in mergeNode.previousNodes) {
            var symbols = node.GetIntersectSymbolsWithNext(mergeNode);
            foreach(var symbol in symbols)
                node.RelateWithNext(this, symbol);
            node.nextNodes.Remove(mergeNode);
        }
        mergeNode.previousNodes.Clear();
    }
    private void MergeNextNodes(RelatedNode mergeNode) {
        foreach(var node in mergeNode.nextNodes) {
            var symbols = node.GetIntersectSymbolsWithPrevious(mergeNode);
            foreach(var symbol in symbols)
                node.RelateWithPrevious(this, symbol);
            node.previousNodes.Remove(mergeNode);
        }
        mergeNode.nextNodes.Clear();
    }
}
