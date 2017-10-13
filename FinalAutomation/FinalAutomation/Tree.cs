using System;
using System.Collections.Generic;
using System.Linq;

public class Tree {
    private RelatedNode startNode;
    private List<RelatedNode> allNodes;
    private List<String> words;

    public Tree() {
        words = new List<String>();
        allNodes = new List<RelatedNode>();
        startNode = CreateNode();
    }

    public void AddWord(String inputWord) => words.Add(inputWord);
    public void Build() {
        SortWords();
        foreach(var word in words)
            Extend(word);
    }

    private void SortWords() => words.Sort();
    private void Extend(String word) {
        RelatedNode currentNode = startNode;
        foreach(var symbol in word) {
            var nextNode = currentNode.GetRelatedNodeByOutput(symbol);
            if(nextNode == null) {
                nextNode = CreateNode();
                currentNode.RelateWithNext(nextNode, symbol);
            }
            currentNode = nextNode;
        }
    }

    private void MergePossibleNodes() {
        Boolean wasMerge = true;
        while(wasMerge) {
            var equalsNodes = FindEqualsNodes();
            wasMerge = !equalsNodes.IsEmpty();
            for(Int32 index = 1; index < equalsNodes.Count; index++)
                equalsNodes[0].Merge(equalsNodes[index]);
        }
    }
    private List<RelatedNode> FindEqualsNodes() {
        var equalsNodes = new List<RelatedNode>();
        for(Int32 indexCurrentNode = 0; indexCurrentNode < allNodes.Count; indexCurrentNode++) {
            var currentNode = allNodes[indexCurrentNode];
            Boolean findEqualNode = false;
            for(Int32 index = indexCurrentNode + 1; index < allNodes.Count; index++) {
                var node = allNodes[index];
                if(currentNode.EqualByOutput(node)) {
                    if(!findEqualNode) {
                        findEqualNode = true;
                        equalsNodes.Add(currentNode);
                    }
                    equalsNodes.Add(node);
                }
            }
            if(findEqualNode)
                return equalsNodes;
        }
        return equalsNodes;
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
