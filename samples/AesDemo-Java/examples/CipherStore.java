// CipherStore.java
//
// ------------------------------------------------------------------
//
// This class knows how to load and store an instance of CipherData.
//
// built on host: DINOCH-2
// Created Sat Aug 08 23:11:27 2009
//
// last saved: 
// Time-stamp: <2009-August-09 02:28:33>
// ------------------------------------------------------------------
//
// Copyright (c) 2005-2009 by Dino Chiesa
// All rights reserved!
//
// ------------------------------------------------------------------

package dpchiesa.examples;

import java.io.IOException;
import java.io.FileInputStream;
import java.io.FileOutputStream;


public class CipherStore {
    
    String FileName= "c:\\CipherText.bin";

    //public CipherStore() {}

    public CipherStore(String filename) {
        FileName = filename;
    }
    
    public void Save(CipherData data) throws IOException {
        FileOutputStream fs= new FileOutputStream(FileName);
                
        fs.write((byte)data.InitializationVector.length);
        fs.write(data.InitializationVector);
                
        byte[] b= data.Passphrase.getBytes();
        fs.write((byte)b.length);
        fs.write(b);
                
        fs.write((byte)data.Salt.length);
        fs.write(data.Salt);
                
        fs.write(data.CipherText); 
        fs.close();
    }
        
    public CipherData Load() throws IOException {
        // Read file here
        FileInputStream fs= new FileInputStream(FileName); 
        CipherData data= new CipherData();
        byte x= (byte) fs.read();
        data.InitializationVector = new byte[x];
        fs.read(data.InitializationVector, 0, data.InitializationVector.length); 

        x= (byte) fs.read();
        byte[] b = new byte[x];
        fs.read(b, 0, b.length);
        data.Passphrase = new String(b); // ascii encoding

        x= (byte) fs.read();
        data.Salt = new byte[x];
        fs.read(data.Salt, 0, data.Salt.length); 
                
        int length= fs.available();
        data.CipherText= new byte[length];
        fs.read(data.CipherText, 0, length); 
        fs.close();
                
        return data;
                
    }
}
