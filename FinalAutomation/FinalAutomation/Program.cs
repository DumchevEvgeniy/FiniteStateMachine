using System;

namespace FinalAutomation {
    class Program {
        private const String fileName = "fileWithSymbols.txt";

        static void Main(String[] args) {
            var tree = new Tree();
            var fileReader = new FileReaderEnumerator(fileName);
            foreach(var inputLine in fileReader)
                tree.AddWord(inputLine);
            tree.Build();
            Console.WriteLine(tree.GetCountNodes());
            Console.ReadKey();
        }
    }
}
