.SUFFIXES:  .java .class

JAVA_HOME=c:\ibmjdk
ECLIPSE_SWT_JAR=c:\eclipse\plugins\org.eclipse.swt.win32.win32.x86_3.5.0.v3550b.jar


javac=$(JAVA_HOME)\bin\javac.exe -g
java=$(JAVA_HOME)\bin\java.exe
jar=$(JAVA_HOME)\bin\jar.exe
swtjars=$(ECLIPSE_SWT_JAR)
classpath=.;$(swtjars)
pkgdir=dpchiesa\examples
CLASSES=$(pkgdir)\AesDemo.class $(pkgdir)\InvokeTab.class $(pkgdir)\AboutTab.class $(pkgdir)\Tab.class $(pkgdir)\PBKDF2.class $(pkgdir)\CipherStore.class $(pkgdir)\CipherData.class $(pkgdir)\Base64.class $(pkgdir)\TextUtils.class

SOURCES=$(pkgdir)\AesDemo.java $(pkgdir)\InvokeTab.java $(pkgdir)\AboutTab.java $(pkgdir)\Tab.java $(pkgdir)\PBKDF2.java $(pkgdir)\CipherStore.java $(pkgdir)\CipherData.java $(pkgdir)\Base64.java $(pkgdir)\TextUtils.java


all: $(pkgdir)\AesDemo.class

$(CLASSES): $(SOURCES) 

.java.class:
        -del $(@D)\*.class 
        $(javac) -classpath $(classpath) -Xlint  $(@D)\$(@B).java

jar: $(CLASSES)
        $(jar) -cvf AesEncryptor.jar  dpchiesa 

clean:
        -del *~
        -del AesEncryptor.jar
        -del /s /q $(pkgdir)\*.class


zip: clean
        -del AesEncryptor.zip
        c:\dinoch\bin\backdir.bat .  AesEncryptor.zip
