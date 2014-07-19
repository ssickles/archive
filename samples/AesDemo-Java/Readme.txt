Sat, 08 Aug 2009  18:13

This is a Java application that demonstrates how to perform AES
encryption from within Java, using the Java JCE.  It also demonstrates
interoperability with .NET Encryption, via the companion .NET app.

This Java application uses the Eclipse Standard Widget Toolkit (SWT) as
the basis for the graphical interface. 

Pre-requisites: 

  IBM JDK 1.5.0
  or
  Sun JDK 1.6

  Eclipse 3.5 platform (runtime) for win32

  nmake utility from the Microsoft .NET SDK

optionally: 
  the Bouncy Castle JCE PRovider, available for download from 
  www.bouncycastle.com 

The code is licensed under the Microsoft Public License.
See the accompanying License.txt file for full information.

--------------------------------------------

You do not need the Eclipse tool to build the Java application, although
the Java application relies on the Eclipse GUI framework runtime. 

Download the required Eclipse Platform at: 

    http://www.eclipse.org/downloads/download.php?file=/eclipse/downloads/drops/R-3.5-200906111540/eclipse-platform-3.5-win32.zip

--------------------------------------------

To build: 
You can drop this into your Eclipse 3.0 workspace, or, just use
the included makefile.  

If you use the makefile you will have
to modify the paths for the JDK and SWT to match your environment.  Also
you will need an nmake.exe utility, which is available in the .NET SDK,
usually at <netsdk 20>\bin\nmake.exe

to build with the makefile:
  nmake

--------------------------------------------

To run:  
  run.bat


(You may have to change paths in the run.bat file as well) 

--------------------------------------------

Instructions for Use: 

Type in a phrase or sentence or paragraph, then click the
encrypt button. The encrypted version is shown.  Click Save to
store the encryption data in a file.  

Then do the converse in the other app:  
Click Load, then Decrypt.  

If you have the same password set in each app, it should work.  
Modify the password on one side or the other to see the effect.  

--------------------------------------------

Bugs: 

This  displays JCE providers that may not beinstalled.  If you
select a JCE provider that is not installed, it won't work.

The IBMJCE provider will not work when using the Sun JVM.  The
SunJCE provider will not work with the IBM JVM.  The bouncy
castle provider works with both.  I didn't try the BEA/Oracle provider.


