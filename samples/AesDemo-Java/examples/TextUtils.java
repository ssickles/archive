// TextUtils.java
// ------------------------------------------------------------------
//
// Utilities for converting between "hex" strings and byte arrays.
// For example, the string "BABECAFE" will convert to an array
// { 0xBA, 0xBE, 0xCA, 0xFE}, and vice versa.
//
// This is needed just for the convenience of displaying byte
// arrays in textboxes in the UI. 
// 
// Author: Admin
// built on host: DINOCH-2
// Created Sat Aug 08 23:11:27 2009
//
// last saved: 
// Time-stamp: <2009-August-09 02:22:05>
// ------------------------------------------------------------------
//
// Copyright (c) 2009 by Dino Chiesa
// All rights reserved!
//
// ------------------------------------------------------------------

package dpchiesa.examples;


public class TextUtils
{

    public static byte[] HexStringToByteArray(String s)
    {
        byte[] r= new byte[s.length()/2];
        for (int i = 0; i < s.length(); i+=2)
        {
            r[i/2] = (byte) java.lang.Integer.parseInt(s.substring(i,2+i), 16);
        }
        return r;
    }

    public static String ByteArrayToHexString(byte[] b) {
        StringBuffer sb1= new StringBuffer();
        for (int i = 0 ; i < b.length; i++) {
            if (((int) b[i] & 0xff) < 0x10) sb1.append("0");
            sb1.append(Long.toString((int) b[i] & 0xff, 16));
        }
        return sb1.toString().toUpperCase();
    }


}

