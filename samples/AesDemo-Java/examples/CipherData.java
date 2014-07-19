// CipherData.java
//
// ------------------------------------------------------------------
//
// A class representing the data stored in the transfer file.
// The transfer file is used to transfer configuration data between the
// .NET and Java versions of the app. 
//
// The file saves the Password, Salt, Initialization Vector, and
// cryptotext. 
//
// built on host: DINOCH-2
// Created Sat Aug 08 23:11:27 2009
//
// last saved: 
// Time-stamp: <2009-August-09 02:27:19>
// ------------------------------------------------------------------
//
// Copyright (c) 2005-2009 by Dino Chiesa
// All rights reserved!
//
// ------------------------------------------------------------------

package dpchiesa.examples;

public class CipherData {

    public byte[] InitializationVector;
    public String Passphrase;
    public byte[] Salt;
    public byte[] CipherText;

    public int getLength()
    {
        return InitializationVector.length +
            Passphrase.length() +
            Salt.length +
            CipherText.length;
    }
}
