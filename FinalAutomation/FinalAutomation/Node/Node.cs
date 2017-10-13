using System;
using System.Linq;
using System.Collections.Generic;

public class Node {
    public Int32 Id { get; private set; }
    public String InputsSymbols { get; private set; } = String.Empty;
    public String OutputsSymbols { get; private set; } = String.Empty;

    public Node(Int32 id) {
        Id = id;
    }

    public void AddInputSymbol(Char symbol) {
        if(!InputsSymbols.Contains(symbol))
            InputsSymbols += symbol;
    }
    public void AddOutputSymbol(Char symbol) {
        if(!OutputsSymbols.Contains(symbol))
            OutputsSymbols += symbol;
    }

    public Boolean IsEnd() => OutputsSymbols.IsEmpty();
    public Boolean IsStart() => InputsSymbols.IsEmpty();
    public Boolean ExistOutputSymbol(Char symbol) => OutputsSymbols.Contains(symbol);
    public Boolean ExistInputSymbol(Char symbol) => InputsSymbols.Contains(symbol);

    public IEnumerable<Char> GetIntersectSymbolsWithNext(Node nextNode) =>
        OutputsSymbols.Intersect(nextNode.InputsSymbols);
    public IEnumerable<Char> GetIntersectSymbolsWithPrevious(Node previousNode) =>
        previousNode.GetIntersectSymbolsWithNext(this);

    public override Boolean Equals(Object obj) {
        if(obj == null)
            return false;
        var node = obj as Node;
        if(node == null)
            return false;
        return node.Id == Id;
    }
    public override Int32 GetHashCode() => Id;
}
