Sun, 09 Aug 2009  02:45

This is a .NET  app that exercises the Rijndael Encryption
facility (AES).  This app shows interop with Java/JCE, via the companion
Java app. 


Pre-requisites: 

  .NET Framework v3.5


The code is licensed under the Microsoft Public License.  See the
accompanying License.txt file for information.

--------------------------------------------

To build and run: 
  open the project in VS2008

you can also build from the command line using msbuild, 
which is part of the .NET SDK. 

--------------------------------------------

Instructions for Using the demo application: 

Type in the various inputs: password, Salt, IV, and a phrase or sentence
or paragraph to encrypt. Then click the Encrypt button. The encrypted
version of the text is shown, along with the binary key used to encrypt.
Click Save to store the encrypted version in a file.

You can then do the converse in the Java application:  
Click Load, then Decrypt.  

After decryption, it should show the original text.


--------------------------------------------

Bugs: 

???


