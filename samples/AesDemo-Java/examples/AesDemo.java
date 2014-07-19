// AesDemo.java
//
// ------------------------------------------------------------------
//
// This is a demonstration application that shows how to do AES Encryption
// in Java, using an RFC2898-compliant password-derived key.  It is fully
// compliant with .NET AES encryption, and you can prove that with the
// companion .NET app.
//
// Author: Admin
// built on host: DINOCH-2
// Created Sat Aug 08 23:11:27 2009
//
// last saved: 
// Time-stamp: <2009-August-09 02:24:08>
// ------------------------------------------------------------------
//
// Copyright (c) 2005-2009 by Dino Chiesa
// All rights reserved!
//
// ------------------------------------------------------------------


package dpchiesa.examples;

import org.eclipse.swt.SWT;
import org.eclipse.swt.layout.FillLayout;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Display;
import org.eclipse.swt.widgets.Shell;
import org.eclipse.swt.widgets.TabFolder;
import org.eclipse.swt.widgets.TabItem;

public class AesDemo {

  private TabFolder tabFolder;
  Tab[] tabs;


  public AesDemo(Composite parent) {
    //initResources();
    tabFolder = new TabFolder(parent, SWT.NULL);
    tabs= new Tab[] {
      new AboutTab(this),
      new InvokeTab(this)};

    tabFolder.addSelectionListener(new org.eclipse.swt.events.SelectionListener() {
        public void widgetSelected(org.eclipse.swt.events.SelectionEvent e) { 
          //System.out.println("widgetSelected: " + e);  // DEBUG
          //System.out.println("tabitem: " + tabFolder.getSelectionIndex());  // DEBUG
          tabs[tabFolder.getSelectionIndex()].gotFocus();  // relay the message
        }

        public void widgetDefaultSelected(org.eclipse.swt.events.SelectionEvent e) { 
          System.out.println("widgetSelected: " + e); 
          System.out.println("tabitem: " + tabFolder.getSelectionIndex()); 
        }
      });
    
    for (int i = 0; i < tabs.length; i++) {
      TabItem item = new TabItem(tabFolder, SWT.NULL);
      item.setText(tabs[i].getTabText());
      item.setControl(tabs[i].createTabFolderPage(tabFolder));
      //item.getControl().setFont(font);
    }
                
    tabFolder.setSelection(1); // activate the invoke tab
  }

  public static void main(String[] args) {
    Display display = new Display();
    Shell shell = new Shell(display);
    //shell.setFont(new org.eclipse.swt.graphics.Font (display, "Tahoma", 12,0)); 
    shell.setLayout(new FillLayout());
    AesDemo instance = new AesDemo(shell);
    //shell.setImage();
    shell.setText("AES Encryption Demo - Eclipse SWT");
    shell.setSize(780, 544); 
    shell.open();
//    org.eclipse.swt.graphics.Font font= shell.getFont();
//    org.eclipse.swt.graphics.FontData[] fd= font.getFontData();
//    for (int i=0; i < fd.length; i++) 
//      System.out.println("font:" + fd[i].getName() + ", " + fd[i].getHeight() + ", " + fd[i].getStyle());
    while (!shell.isDisposed()) {
      if (!display.readAndDispatch())
        display.sleep();
    }
  }

  

  public void dispose() {
    freeResources();
  }
  private void freeResources() {

  }
}
