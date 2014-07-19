// AboutTab.java
//
// ------------------------------------------------------------------
//
// A simple "about" tab for the demo app.
// 
// Author: Admin
// built on host: DINOCH-2
// Created Sat Aug 08 23:11:27 2009
//
// last saved: 
// Time-stamp: <2009-August-09 02:24:22>
// ------------------------------------------------------------------
//
// Copyright (c) 2009 by Dino Chiesa
// All rights reserved!
//
// ------------------------------------------------------------------

package dpchiesa.examples;

import java.util.Properties;

import org.eclipse.swt.SWT;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Group;
import org.eclipse.swt.widgets.Label;
import org.eclipse.swt.widgets.Text;

class AboutTab extends Tab {

        Button btnOk;
        Label label1, label2, label3;
        Text txtDemo, txtApp; 

        Group grpApp;
        Group grpDemo;

        AboutTab(AesDemo instance) {
                super(instance);
        }

        void createUberGroup() {
                super.createUberGroup();

                /* Create a group for the text widget */
                grpDemo= new Group(tabFolderPage, SWT.NULL);

                GridLayout gridLayout = new GridLayout();
                gridLayout.numColumns = 1;
                grpDemo.setLayout(gridLayout);
                grpDemo.setLayoutData(
                        new GridData(
                                GridData.GRAB_HORIZONTAL
                                | GridData.GRAB_VERTICAL
                                        | GridData.HORIZONTAL_ALIGN_FILL
                                        | GridData.VERTICAL_ALIGN_FILL));
                grpDemo.setText("About This Demo");

                /* Create a group to specify the endpoint  */
                grpApp = new Group(tabFolderPage, SWT.NULL);

                gridLayout = new GridLayout();
                gridLayout.numColumns = 1;
                grpApp.setLayout(gridLayout);
                grpApp.setLayoutData(
                        new GridData(
                                GridData.GRAB_HORIZONTAL
                                | GridData.GRAB_VERTICAL
                                        | GridData.HORIZONTAL_ALIGN_FILL
                                        | GridData.VERTICAL_ALIGN_FILL));
                
                grpApp.setText("Application Infrastructure");
        }


        void createChildWidgets() {
                createUberGroup();
                int style = SWT.NONE;

                txtDemo= new Text(grpDemo, SWT.MULTI | SWT.WRAP | SWT.BORDER
                                | SWT.V_SCROLL);
                txtDemo.setText("");
                txtDemo.setLayoutData(new GridData(GridData.FILL_BOTH));
                txtDemo.setEditable(false);

                StringBuffer sb1= new StringBuffer();
                sb1.append("AES Interop demo v1.0\n");
                sb1.append("SWT Version\n");
                txtDemo.setText(sb1.toString());
                
                
                txtApp= new Text(grpApp, SWT.MULTI | SWT.WRAP | SWT.BORDER
                                | SWT.V_SCROLL);
                txtApp.setText("");
                txtApp.setLayoutData(new GridData(GridData.FILL_BOTH));
                txtApp.setEditable(false);

                sb1= new StringBuffer();
                sb1.append("Eclipse SWT  (Platform: ").append(SWT.getPlatform()).append(", Version: ");
                sb1.append(SWT.getVersion()).append(")\n");
                // get system properties here
                
                String[] keys= new String[]{"java.runtime.name", "java.vm.version", "java.vm.vendor", "java.vm.info", "os.name", "os.version"};
            Properties p = new Properties(System.getProperties());
            for (int i=0; i < keys.length; i++) {
                sb1.append(keys[i]).append(": ").append(p.getProperty(keys[i])).append("\n");
            }
                txtApp.setText(sb1.toString());




        }

        String getTabText() {
                return "About";
        }

}
