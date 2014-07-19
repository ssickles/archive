// InvokeTab.java
//
// ------------------------------------------------------------------
//
// This defines the 2nd tab in the tabbed UI for the demonstration app. 
// On the invoke tab, the user can set all the settings for the app,
// and can click buttons to encrypt and decrypt data. 
//
// built on host: DINOCH-2
// Created Sat Aug 08 23:11:27 2009
//
// last saved: 
// Time-stamp: <2009-August-09 02:29:29>
// ------------------------------------------------------------------
//
// Copyright (c) 2005-2009 by Dino Chiesa
// All rights reserved!
//
// ------------------------------------------------------------------

package dpchiesa.examples;

import org.eclipse.swt.SWT;
import org.eclipse.swt.custom.SashForm;
import org.eclipse.swt.events.DisposeEvent;
import org.eclipse.swt.events.DisposeListener;
import org.eclipse.swt.events.SelectionAdapter;
import org.eclipse.swt.events.SelectionEvent;
import org.eclipse.swt.events.SelectionListener;
import org.eclipse.swt.graphics.Cursor;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.layout.FillLayout;
import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Combo;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Display;
import org.eclipse.swt.widgets.Group;
import org.eclipse.swt.widgets.Label;
import org.eclipse.swt.widgets.List;
import org.eclipse.swt.widgets.Text;
import org.eclipse.swt.graphics.Color;


import java.security.GeneralSecurityException;
import java.security.InvalidKeyException;
import java.security.Provider;
import java.security.Security;
import java.util.Hashtable;

import javax.crypto.BadPaddingException;
import javax.crypto.Cipher;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;
import javax.crypto.spec.PBEKeySpec;
import javax.crypto.SecretKeyFactory;
import javax.crypto.SecretKey;


import dpchiesa.examples.PBKDF2;


class InvokeTab extends Tab {

    SashForm sashForm;
    Group grpInput, grpResults, grpStatus;
    Button btnEncrypt, btnDecrypt, btnSave, btnLoad, btnSwap, btnReset; 

    Text txtPassword, txtInput, txtResult, txtStatus,
        txtFile, txtIterations, txtSalt, txtKey, txtInitVector;
    
    Cursor waitCursor;
    Combo listProviders;

    String DefaultText="Far better is it to dare mighty things, to win glorious triumphs, even though checked by failures... than to rank with those poor spirits who neither enjoy nor suffer much, because they live in a gray twilight that knows not victory nor defeat. --Teddy Roosevelt";
    String currentEndpoint = "none";
    String[] ProviderStrings= {"IBMJCE", "SunJCE", "BC"};

    Cipher _aesCipher;
    String _providerName;
    byte[] LastEncrypted;

    Color pink, white;
    
    final int _keyLengthInBits = 128;

    
    InvokeTab(AesDemo instance) {
        super(instance);
    }

    
   
    String getTabText() {
        return "Invoke";
    }


    void createButtons() {

        int style = SWT.NONE;
        // buttons 
        Composite container= new Composite(tabFolderPage, SWT.NULL);
                
        GridLayout gridLayout = new GridLayout();
        gridLayout.numColumns = 8;
        container.setLayout(gridLayout);
        container.setLayoutData(new GridData(GridData.GRAB_HORIZONTAL));

        Label labelProv = new Label(container, style);  
        labelProv.setText("Provider:");
        
        listProviders= new Combo(container,SWT.SINGLE); 
        for (int i = 0; i < ProviderStrings.length; i++) {
            listProviders.add(ProviderStrings[i]);
        }
        listProviders.select(0);
                
        btnEncrypt = new Button(container, style);
        btnEncrypt.setText("Encrypt");
        btnDecrypt= new Button(container, style);
        btnDecrypt.setText("Decrypt");
        btnSave= new Button(container, style);
        btnSave.setText("Save");
        btnLoad = new Button(container, style);
        btnLoad.setText("Load");
        btnSwap = new Button(container, style);
        btnSwap.setText("Swap");
        btnReset = new Button(container, style);
        btnReset.setText("Reset");
                
        SelectionListener Encrypt_Click = new SelectionAdapter() {
                public void widgetSelected(SelectionEvent event) {
                    doEncrypt();
                };
            };
        btnEncrypt.addSelectionListener(Encrypt_Click);
                
        SelectionListener Decrypt_Click= new SelectionAdapter() {
                public void widgetSelected(SelectionEvent event) {
                    doDecrypt();
                };
            }; 
        btnDecrypt.addSelectionListener(Decrypt_Click);
                
        SelectionListener Save_Click=new SelectionAdapter() {
                public void widgetSelected(SelectionEvent event) { doSave(); };
            };
        btnSave.addSelectionListener(Save_Click);

        SelectionListener Load_Click=new SelectionAdapter() {
                public void widgetSelected(SelectionEvent event) { doLoad(); };
            };
        btnLoad.addSelectionListener(Load_Click);

        SelectionListener Swap_Click = new SelectionAdapter() {
                public void widgetSelected(SelectionEvent event) {
                    txtInput.setText(txtResult.getText());
                    txtResult.setText("");
                };
            };
        btnSwap.addSelectionListener(Swap_Click);

        SelectionListener Reset_Click= new SelectionAdapter() {
                public void widgetSelected(SelectionEvent event) {
                    txtInput.setText(DefaultText);
                    txtResult.setText("");
                };
            };
        btnReset.addSelectionListener(Reset_Click);
    }


    
    void createChildWidgets() {

        GridLayout gridLayout;
        Composite container;
        Label label;
        
        //              Display display = tabFolderPage.getShell().getDisplay();
        //              org.eclipse.swt.graphics.Font font= new org.eclipse.swt.graphics.Font (display, "Tahoma", 12,0); 
        //              GC gc= new GC(tabFolderPage);
        //              gc.setFont(font);


        //   +--tabFolderPage-------------------------------------------+
        //   |                                                          |
        //   |  +--holder2-------------------------------------------+  |
        //   |  | label select  btn1 btn2 .. btnN                    |  | 
        //   |  +----------------------------------------------------+  |
        //   |                                                          |
        //   |  +--sashForm1-------------+---------------------------+  |
        //   |  |                        |                           |  |
        //   |  | +--grpInput----------+ |  +--grpResults---------+  |  |
        //   |  | |                    | |  |                     |  |  |
        //   |  | |                    | |  |                     |  |  |
        //   |  | |                    | |  +---------------------+  |  |
        //   |  | |                    | |                           |  |
        //   |  | |                    | |                           |  |
        //   |  | |                    | |                           |  |
        //   |  | |                    | |                           |  |
        //   |  | +--------------------+ |                           |  |
        //   |  |                        |                           |  |
        //   |  |                        |                           |  |
        //   |  |                        |                           |  |
        //   |  +----------------------------------------------------+  |
        //   |                                                          |
        //   |  +--grpStatus-----------------------------------------+  |
        //   |  |                                                    |  |
        //   |  |                                                    |  |
        //   |  +----------------------------------------------------+  |
        //   |                                                          |
        //   +----------------------------------------------------------+
        //
        
        //             tabFolderPage.setLayout(new FillLayout());
        
        createButtons();

        container = new Composite(tabFolderPage, SWT.NULL);  // BORDER
        container.setLayout(new FillLayout());
        GridData data1 = new GridData(GridData.GRAB_HORIZONTAL |
                                      GridData.GRAB_VERTICAL |
                                      GridData.HORIZONTAL_ALIGN_FILL |
                                      GridData.VERTICAL_ALIGN_FILL);
        data1.verticalAlignment = GridData.FILL;
        data1.verticalSpan = 4;
        container.setLayoutData(data1);
        
        //         container
        //                .setLayoutData(new GridData(GridData.GRAB_HORIZONTAL |
        //                                            GridData.HORIZONTAL_ALIGN_FILL |
        //                                            GridData.VERTICAL_ALIGN_FILL
        //                                            ));
        
        sashForm = new SashForm(container, SWT.HORIZONTAL);
        
        //         new Button(sashForm, SWT.PUSH).setText("Left");
        //         new Button(sashForm, SWT.PUSH).setText("Right");

        //         // Create a group for the input widget */
        grpInput = new Group(sashForm, SWT.NULL);
        gridLayout = new GridLayout();
        gridLayout.numColumns = 1;
                
        // The input group gets a grid layout with one column
        // In that one column there is a composite and a button
        // The composite has 2 columns.
        grpInput.setLayout(gridLayout);
        grpInput
            .setLayoutData(new GridData(//GridData.GRAB_HORIZONTAL |
                                        GridData.GRAB_VERTICAL |
                                        GridData.HORIZONTAL_ALIGN_FILL |
                                        GridData.VERTICAL_ALIGN_FILL
                                        ));
        grpInput.setText("Input");

        container= new Composite(grpInput, SWT.NULL);
        gridLayout = new GridLayout();  
        gridLayout.numColumns = 2;
        container.setLayout(gridLayout);
        container.setLayoutData(new GridData(GridData.GRAB_HORIZONTAL 
                                             | GridData.HORIZONTAL_ALIGN_FILL ));

        label = new Label(container, SWT.NONE);
        label.setText("Transfer file:");
        txtFile = new Text(container, SWT.SINGLE | SWT.BORDER); 
        txtFile.setLayoutData(new GridData(GridData.FILL_HORIZONTAL));
        txtFile.setText("c:\\temp\\CipherText.out");

        label = new Label(container, SWT.NONE);  
        label.setText("Pass Phrase:");
        txtPassword = new Text(container, SWT.SINGLE | SWT.BORDER); 
        txtPassword.setLayoutData(new GridData(GridData.FILL_HORIZONTAL));
        txtPassword.setText("P4ssw0rD");

        label = new Label(container, SWT.NONE);  
        label.setText("RFC2898 Iterations:");
        txtIterations = new Text(container, SWT.SINGLE | SWT.BORDER); 
        txtIterations.setLayoutData(new GridData(GridData.FILL_HORIZONTAL));
        txtIterations.setText("1000");
         
        label = new Label(container, SWT.NONE);  
        label.setText("Salt:");
        txtSalt = new Text(container, SWT.SINGLE | SWT.BORDER); 
        txtSalt.setLayoutData(new GridData(GridData.FILL_HORIZONTAL));
        txtSalt.setText("00010203040506070809");
         
        label = new Label(container, SWT.NONE);  
        label.setText("Generated Key:");
        txtKey = new Text(container, SWT.SINGLE | SWT.BORDER | SWT.READ_ONLY); 
        txtKey.setLayoutData(new GridData(GridData.FILL_HORIZONTAL));
        txtKey.setText("--");
         
        label = new Label(container, SWT.NONE);  
        label.setText("IV:");
        txtInitVector = new Text(container, SWT.SINGLE | SWT.BORDER); 
        txtInitVector.setLayoutData(new GridData(GridData.FILL_HORIZONTAL));
        txtInitVector.setText("000102030405060708090A0B0C0D0E0F");
         
         
        //         /* Create a group for the results widget */
        grpResults = new Group(sashForm, SWT.NULL);

        gridLayout = new GridLayout();
        gridLayout.numColumns = 1;
        grpResults.setLayout(gridLayout);
        grpResults.setLayoutData(new GridData(GridData.GRAB_HORIZONTAL |
                                              GridData.GRAB_VERTICAL | GridData.HORIZONTAL_ALIGN_FILL |
                                              GridData.VERTICAL_ALIGN_FILL));
        grpResults.setText("Transformation Result:");

        
        // Create a group for the status box
        grpStatus = new Group(tabFolderPage, SWT.NULL);
        grpStatus.setText("Status");
        gridLayout = new GridLayout();
        gridLayout.numColumns = 1;
        grpStatus.setLayout(gridLayout);
        //         grpStatus.setLayoutData(new GridData(GridData.GRAB_HORIZONTAL |
        //                                              //GridData.GRAB_VERTICAL | 
        //                                              GridData.HORIZONTAL_ALIGN_FILL |
        //                                              GridData.VERTICAL_ALIGN_FILL));
        
        GridData data2 = new GridData(GridData.GRAB_HORIZONTAL);
        //                                       GridData.GRAB_VERTICAL |
        //                                       GridData.HORIZONTAL_ALIGN_FILL |
        //                                       GridData.VERTICAL_ALIGN_FILL
        data2.verticalAlignment= GridData.FILL;
        data2.grabExcessVerticalSpace= true;
        data2.horizontalAlignment= GridData.FILL;
        data2.verticalSpan = 1;
        //data2.heightHint = 2;
        grpStatus.setLayoutData(data2);



        Display display = grpInput.getShell().getDisplay();

        pink = new Color(display, 0xFF, 0xE4, 0xC4);
        white = new Color(display, 0xFF, 0xFF, 0xFF);
        
        waitCursor = new Cursor(display, SWT.CURSOR_WAIT);

        tabFolderPage.addDisposeListener(new DisposeListener() {
                public void widgetDisposed(DisposeEvent e) {
                    waitCursor.dispose();
                    pink.dispose();
                    white.dispose();
                }
            });


        //          container= new Composite(grpInput, SWT.NULL);
        //          gridLayout = new GridLayout();  
        //          gridLayout.numColumns = 2;
        //          container.setLayout(gridLayout);
        //          container.setLayoutData(new GridData(GridData.GRAB_HORIZONTAL 
        //                                             | GridData.HORIZONTAL_ALIGN_FILL ));

        label = new Label(grpInput, SWT.NONE);
        label.setText("Plain Text:");

        txtInput = new Text(grpInput,  
                            SWT.MULTI 
                            | SWT.WRAP 
                            | SWT.BORDER
                            | SWT.V_SCROLL); 
        txtInput.setLayoutData(new GridData(GridData.FILL_BOTH
                                            | GridData.VERTICAL_ALIGN_FILL
                                            | GridData.HORIZONTAL_ALIGN_FILL
                                            ));
        txtInput.setText(DefaultText);


        txtResult = new Text(grpResults, SWT.MULTI | SWT.WRAP | SWT.BORDER | SWT.V_SCROLL | SWT.READ_ONLY);
        txtResult.setText("");
        txtResult.setLayoutData(new GridData(GridData.FILL_BOTH));
        txtResult.setEditable(false);
                
        txtStatus = new Text(grpStatus, SWT.MULTI | SWT.WRAP | SWT.BORDER | SWT.V_SCROLL);
        String vendor =  System.getProperty("java.vendor");
        txtStatus.setText("Ready. (" + vendor + ")\r\n");
        txtStatus.setLayoutData(new GridData(GridData.FILL_BOTH));
        txtStatus.setEditable(false);

        if (vendor.startsWith("Sun"))
        {
            listProviders.select(1);
        }
    }   


    
    private boolean setupAesCipher(int mode) {
        int ix= listProviders.getSelectionIndex();
        _providerName = listProviders.getItem(ix) ;
        if (!verifyJceProvider())
        {
            txtStatus.append("JCE Provider '" + _providerName + "' was not found.\r\n");
            return false;
        }
        
        try
        {
            txtStatus.append("provider:" + _providerName + "\r\n");
            _aesCipher = Cipher.getInstance("AES/CBC/PKCS5Padding", _providerName);
            txtStatus.append("cipher\r\n");
            int iterations = Integer.parseInt(txtIterations.getText());
            txtStatus.append("iterations: " + iterations + "\r\n");
            byte[] keyBytes= PBKDF2.deriveKey(txtPassword.getText().getBytes(),
                                              TextUtils.HexStringToByteArray(txtSalt.getText()),
                                              iterations,
                                              _keyLengthInBits/8);

            txtKey.setText(TextUtils.ByteArrayToHexString(keyBytes));
            
            SecretKeySpec keySpec = new SecretKeySpec(keyBytes, "AES");

            byte[] iv = TextUtils.HexStringToByteArray(txtInitVector.getText());
            IvParameterSpec ivSpec = new IvParameterSpec(iv);

            _aesCipher.init(mode, keySpec, ivSpec);
        }
        catch (Exception ex1)
        {
            txtStatus.append("Exception setting up cipher: " + ex1.getMessage() + "\r\n");
            ex1.printStackTrace();
            return false; 
        }
        
        return true;
    }

    
    private void doEncrypt() {
        if (txtInitVector.getText().length() != 32)
        {
            txtInitVector.setBackground(pink);
            txtStatus.append("Please enter a 16-byte IV.\r\n");
            return;
        }

        if (!setupAesCipher(Cipher.ENCRYPT_MODE))
            return;
        
        grpInput.getShell().setCursor(waitCursor);
        
        txtStatus.append("Using provider: " + _providerName + "\r\n");
        
                
        byte[] plainText = txtInput.getText().trim().getBytes();
        long startTime, loadTime;
        startTime = System.currentTimeMillis();
        txtStatus.append("Encrypting...");
        try {
            txtResult.setText("");
            byte[] result= _aesCipher.doFinal(plainText);
            txtResult.setText(Base64.encode(result));
            LastEncrypted= result;
        }
        catch(Exception ex) {
            txtStatus.append("Exception: " + ex.getMessage() + "\n");
        }

        loadTime = System.currentTimeMillis() - startTime;
        txtStatus.append("done (" + loadTime + "ms).\n");
        txtResult.setSelection(0);
        grpInput.getShell().setCursor(null);  // normal cursor
    }

    private void doDecrypt() {
        if (txtInitVector.getText().length() != 32)
        {
            txtInitVector.setBackground(pink);
            txtStatus.append("Please enter a 16-byte IV.\r\n");
            return;
        }

        if (!setupAesCipher(Cipher.DECRYPT_MODE))
            return;
        
        grpInput.getShell().setCursor(waitCursor);
        
        txtStatus.append("Using provider: " + _providerName + "\r\n");



        long startTime, loadTime;
        startTime = System.currentTimeMillis();
        txtStatus.append("Decrypting...");
        try {
            byte[] cipherText = Base64.decode(txtInput.getText().trim());
            txtResult.setText("");
            byte[] decrypted= _aesCipher.doFinal(cipherText);
            String result= new String(decrypted);  // ascii
            txtResult.setText(result);
        }
        catch(Exception ex) {
            txtStatus.append("While Decrypting, Exception: " + ex.toString() + "\n");
            txtResult.setText("(garbled)");
        }

        loadTime = System.currentTimeMillis() - startTime;
        txtStatus.append("done (" + loadTime + "ms).\n");
        txtResult.setSelection(0);
        grpInput.getShell().setCursor(null);
    }

    private void doSave() {
        try {

            byte[] iv = TextUtils.HexStringToByteArray(txtInitVector.getText());
            CipherStore cs= new CipherStore(txtFile.getText());
            CipherData cd= new CipherData();
            cd.InitializationVector = iv;
            cd.CipherText= LastEncrypted;
            cd.Passphrase= txtPassword.getText();
            cd.Salt = TextUtils.HexStringToByteArray(txtSalt.getText());
            cs.Save(cd);
            txtStatus.append(cd.getLength() + " bytes saved in " + txtFile.getText() + " (" + cd.CipherText.length + " crypto bytes).\n");
        }
        catch (Exception ex) {
            txtStatus.append("While saving, Exception: " + ex.toString() + "\n");
        }
    }
    private void doLoad(){
        try {
            CipherStore cs= new CipherStore(txtFile.getText());
            CipherData cd= cs.Load();
            txtInitVector.setText(TextUtils.ByteArrayToHexString(cd.InitializationVector));
            txtSalt.setText(TextUtils.ByteArrayToHexString(cd.Salt));
            txtInput.setText(Base64.encode(cd.CipherText));
            txtPassword.setText(cd.Passphrase);
            txtResult.setText("");
            txtStatus.append(cd.getLength() + " bytes loaded from " + txtFile.getText() + " (" + cd.CipherText.length + " crypto bytes).\n");
        }
        catch (Exception ex) {
            txtStatus.append("While loading, Exception: " + ex.toString() + "\n");
        }
                
    }


              
    Hashtable<String,Boolean> CheckedJce= new Hashtable<String,Boolean>();
    Hashtable<String,Boolean> HaveJce= new Hashtable<String,Boolean>();
          
    private boolean verifyJceProvider() {
                
        // Check if provider is already installed., eg "IBMJCE", "SunJCE", "BC"
        if (!CheckedJce.containsKey(_providerName)) {                   
            if (Security.getProvider(_providerName) == null) {
                // Provider is not installed, try installing it.
                try {
                    Security.addProvider
                        ((Provider)Class.forName(_providerName).newInstance());
                    CheckedJce.put(_providerName, new Boolean(true)); 
                    HaveJce.put(_providerName, new Boolean(true));
                }
                catch (Exception ex) {
                    //System.out.println("Cannot install provider: " + ex.getMessage());
                    txtStatus.append("Cannot install provider: " + ex.getMessage() + "\r\n");
                                        
                    CheckedJce.put(_providerName, new Boolean(true)); 
                    HaveJce.put(_providerName, new Boolean(false));
                }
            }
            else {
                CheckedJce.put(_providerName, new Boolean(true)); 
                HaveJce.put(_providerName, new Boolean(true));
            }
        }
        return ((Boolean) HaveJce.get(_providerName)).booleanValue();
    }

    
    
}