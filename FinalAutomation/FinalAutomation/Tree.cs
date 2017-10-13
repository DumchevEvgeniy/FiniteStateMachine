using System;
using System.Collections.Generic;
using System.Linq;

public class Tree {
    private RelatedNode startNode;
    private List<RelatedNode> allNodes;

    public Tree() {
        allNodes = new List<RelatedNode>();
        startNode = CreateNode();
    }

    public void Extend(String inputWord) {
        String word = inputWord;
        RelatedNode currentNode = startNode;
        while(true) {
            if(word.IsEmpty())
                return;
            if(TryRelateWithExistPathToEnd(currentNode, word))
                return;
            if(!ExistPathToNext(currentNode, word.First())) {
                var newNode = CreateNode();
                currentNode.RelateWithNext(newNode, word.First());
                currentNode = newNode;
            }
            word = word.Remove(0, 1);
        }
    }

    private RelatedNode CreateNode() {
        var newNode = new RelatedNode(allNodes.Count);
        allNodes.Add(newNode);
        return newNode;
    }

    private IEnumerable<RelatedNode> GetStartNodesArriveToEnd(Int32 lengthPath) {
        foreach(var node in allNodes)
            if(node.CanMoveToEnd(lengthPath))
                yield return node;
    }

    private Boolean TryRelateWithExistPathToEnd(RelatedNode currentNode, String word) {
        foreach(var startNode in GetStartNodesArriveToEnd(word.Length))
            if(startNode.ExistPathToEnd(word)) {
                currentNode.RelateWithNext(startNode, word.First());
                return true;
            }
        return false;
    }
    private Boolean ExistPathToNext(RelatedNode currentNode, Char symbol) {
        var node = currentNode.GetRelatedNodeByOutput(symbol);
        if(node == null)
            return false;
        currentNode = node;
        return true;
    }

    public Int32 GetCountNodes() => allNodes.Count;
}
