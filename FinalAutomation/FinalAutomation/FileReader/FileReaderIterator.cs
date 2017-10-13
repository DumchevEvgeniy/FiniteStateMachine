using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;

public class FileReaderIterator : IEnumerator<String> {
    private StreamReader streamReader;
    private String current;
    private String filename;

    public FileReaderIterator(String filename) {
        this.filename = filename;
        Reset();
    }

    public void Dispose() {
        streamReader.Close();
        streamReader.Dispose();
    }
    public void Reset() {
        current = null;
        StreamInitialize();
    }
    private void StreamInitialize() => streamReader = new StreamReader(filename);

    public Boolean MoveNext() {
        if(streamReader.EndOfStream)
            return false;
        current = streamReader.ReadLine();
        return true;
    }

    public String Current => current;
    Object IEnumerator.Current => Current;
}
