// Tab.java
//
// ------------------------------------------------------------------
//
// Tab is an abstract superclass for a tab in a tabbed UI.
// It is the superclass for the two tabs in the AesDemo UI.
//
// This class defines code that is common to each tab.
//
//
// built on host: DINOCH-2
// Created Sat Aug 08 23:11:27 2009
//
// last saved: 
// Time-stamp: <2009-August-09 02:31:15>
// ------------------------------------------------------------------
//
// Copyright (c) 2005-2009 by Dino Chiesa
// All rights reserved!
//
// ------------------------------------------------------------------

package dpchiesa.examples;


import org.eclipse.swt.SWT;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.eclipse.swt.widgets.TabFolder;

abstract class Tab {

        /* Common composite */
        Composite tabFolderPage;

        /* Controlling instance */
        final AesDemo instance;

        /**
         * Creates the Tab within a given instance of ControlExample.
         */
        Tab(AesDemo instance) {
                this.instance = instance;
        }

        // a group that covers the entire tabFolderPage
        void createUberGroup() {
        }

        /**
         * Creates the "Example" widget children of the "Example" group.
         * Subclasses override this method to create the particular
         * example control.
         */
        void createChildWidgets() {
                /* Do nothing */
        }

        void setChildWidgetState() {
                /* Do nothing */
        }

        Composite createTabFolderPage(TabFolder tabFolder) {

                tabFolderPage = new Composite(tabFolder, SWT.NULL);


                GridLayout layout = new GridLayout();
                tabFolderPage.setLayout(layout);
                tabFolderPage.setLayoutData(
                        new GridData(
                                GridData.HORIZONTAL_ALIGN_BEGINNING |
                                GridData.VERTICAL_ALIGN_BEGINNING));
                //gridLayout.numColumns = 2;

                //              createUberGroup();  //  inheritor may call this
                createChildWidgets();

                return tabFolderPage;
        }


        Control[] getChildWidgets() {
                return new Control[0];
        }

        String getTabText() {
                return "";
        }

        public void gotFocus() {
        }

}
