using System;
using System.Collections;
using System.Collections.Generic;

public class FileReaderEnumerator : IEnumerable<String> {
    private FileReaderIterator iterator;
    private String fileName;

    public FileReaderEnumerator(String fileName) {
        this.fileName = fileName;
    }

    public IEnumerator<String> GetEnumerator() {
        iterator = new FileReaderIterator(fileName);
        while(iterator.MoveNext())
            yield return iterator.Current;
        iterator.Dispose();
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
