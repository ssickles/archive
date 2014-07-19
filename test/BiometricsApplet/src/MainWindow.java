import javax.swing.*;

import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.GridLayout;
import java.awt.event.*;
import javax.swing.JList;
import java.util.EnumSet;
import javax.swing.JOptionPane;

import com.neurotechnology.Fingers.NFExtractor;
import com.neurotechnology.Fingers.Position;
import com.neurotechnology.NImages.NImage;
import com.neurotechnology.NLicensing.NLicensing;
import com.neurotechnology.NLicensing.LicensingConfig;
import com.neurotechnology.ScannerMan.ImageScanned;
import com.neurotechnology.ScannerMan.ScanManException;
import com.neurotechnology.ScannerMan.Scanner;
import com.neurotechnology.ScannerMan.ScannerMan;
import com.neurotechnology.Templates.NFRecord;

public class MainWindow extends JPanel implements ActionListener {

	private static final long serialVersionUID = 1L;
	public static final String LIBRARY = "JVFWrapper";
	static MainWindow me;
	static ScannerMan scanMan;

	NFExtractor extractor;

	// GDI components
	private JPanel mainPanel;

	private JPanel picturePanel;
	private JPanel bottomPanel;
	private JPanel controllPanel;
	private JPanel dbPanel;
	private JPanel infoPanel;

	private JTextArea txtArea;

	private ImageIcon srcImage;
	private ImageIcon binImage;

	private JLabel srcLabel;
	private JLabel binLabel;

	private JFrame scannersList;
	private JList scannersJList;

	// Global menu components
	private JMenuItem extractorParams;

	private JMenuItem scanersList;

	JTextField currScanner;

	// Application components
	Scanner[] scanners;
	Position fingerPos;

	NFRecord record;
	NImage image;
	ImageScanned currImageScanner;

	// Constructor
	public MainWindow() {
		
		try {
			extractor = new NFExtractor();
		} catch (Exception e) {
			this.print("Unable to load system libraries/n");
		}
		;

		mainPanel = new JPanel();
		picturePanel = new JPanel();
		bottomPanel = new JPanel();
		controllPanel = new JPanel();
		dbPanel = new JPanel();
		infoPanel = new JPanel();

		this.setLayout(new BorderLayout());
		mainPanel.setLayout(new BoxLayout(mainPanel, BoxLayout.PAGE_AXIS));
		mainPanel.setPreferredSize(new Dimension(790, 600));
		picturePanel.setLayout(new GridLayout(1, 2));
		picturePanel.setPreferredSize(new Dimension(790, 400));
		bottomPanel.setPreferredSize(new Dimension(790, 200));

		bottomPanel.setLayout(new BoxLayout(bottomPanel, BoxLayout.PAGE_AXIS));
		controllPanel.setLayout(new GridLayout(1, 2));
		controllPanel.setPreferredSize(new Dimension(790, 40));
		controllPanel.setMaximumSize(new Dimension(790, 40));
		dbPanel.setLayout(new GridLayout(1, 3));
		infoPanel.setLayout(new GridLayout(2, 1));

		currScanner = new JTextField();

		controllPanel.add(infoPanel);
		controllPanel.add(dbPanel);

		try
		{
		srcImage = new ImageIcon(getClass().getResource("/img/lefthand.png"));
		binImage = new ImageIcon(getClass().getResource("/img/righthand.png"));
		}
		catch (Exception e)
		{
			this.print("Unable to load left hand and right hand images\n");
			e.printStackTrace();
		}

		srcLabel = new JLabel(srcImage);
		binLabel = new JLabel(binImage);

		txtArea = new JTextArea();
		JScrollPane scrollPan = new JScrollPane(txtArea);

		picturePanel.add(srcLabel);
		picturePanel.add(binLabel);

		bottomPanel.add(controllPanel);
		bottomPanel.add(scrollPan);

		buildMenu();
		this.add(mainPanel, BorderLayout.CENTER);
		mainPanel.add(picturePanel);
		mainPanel.add(bottomPanel);

		addActionListeners();
		
		Exception ex = null;
		
		try {
			LicensingConfig licConfig = LicensingConfig.getInstance();
			String licensesString = licConfig.getLicenseString(EnumSet.of(LicensingConfig.LicenseType.FingersExtractor));
			if (NLicensing.licenseObtain(licConfig.getServerIP(), licConfig.getServerPort(), licensesString)) {
				try {
					MainWindow.scanMan = new ScannerMan();
				} catch (Exception e) {
				e.printStackTrace();
					ex = e;
				}
				
				if (ex != null) {
					print("ScannerMan initialization failed!\n");
					print(ex.toString() + "\n");
				} else
					print("ScannerMan initialization successful\n");
			} else {
				JOptionPane.showMessageDialog(null, "Unable to obtain the following licenses: " + licensesString,
						"Failed to obtain required licenses", JOptionPane.ERROR_MESSAGE);
			}
		} catch (Exception exc) {
			System.out.println(exc.getMessage());
			exc.printStackTrace(System.err);
			System.exit(1);
		}
		
		extract();
	}

	private void buildMenu()
	{
		JMenuBar mainMenu = new JMenuBar();
		JMenu scanersMenu = new JMenu("Scanners");
		JMenu paramMenu = new JMenu("Parameters");

		extractorParams = new JMenuItem("Extractor");
		paramMenu.insert(extractorParams, 1);

		scanersList = new JMenuItem("List");
		scanersMenu.insert(scanersList, 1);

		mainMenu.add(scanersMenu);
		mainMenu.add(paramMenu);

		this.add(mainMenu, BorderLayout.NORTH);
	}
	
	public void actionPerformed(ActionEvent e) {
		try {
			if (e.getSource() == scanersList)
				scanersList();
			if (e.getSource() == extractorParams)
				new ExtractorParameters(extractor);

		} catch (Exception ex) {
			txtArea.append("Exception occured\n");
			ex.printStackTrace();
		}
	}

	private void extract() {
		Object[] scans = null;
		try {
			scans = scanMan.getScanners();
		} catch (Exception e) {
			this.print(e.getMessage() + "\n");
		}
		;
		Extractor gen = null;
		try {
			gen = new Extractor(this);
			currImageScanner = gen;
		} catch (Exception e) {
			this.print(e.getMessage() + "\n");
			return;
		}
		;
		if (scans.length == 0) {
			txtArea.append("No scaners selected\n");
			return;
		}
		for (int i = 0; i < scans.length; i++) {
			((Scanner) scans[i]).StopCapturing();
			((Scanner) scans[i]).SetFingerPlacedCallback(gen);
			((Scanner) scans[i]).SetFingerRemovedCallback(gen);
			((Scanner) scans[i]).SetImageScannedCallback(gen);
			((Scanner) scans[i]).StartCapturing();
			txtArea.append("Scanner - " + ((Scanner) scans[i]).GetID() + " capturing started\n");
		}
	}

	void scanersList() throws ScanManException {
		scanners = scanMan.getScanners();

		if (scanners == null) {
			this.print("No scanners connected\n");
			return;
		}

		scannersList = new JFrame();
		scannersList.setTitle("Scanners");
		scannersList.setSize(200, 250);

		scannersJList = new JList(scanners);

		scannersList.add(scannersJList, BorderLayout.CENTER);

		scannersList.setVisible(true);
	}

	private void addActionListeners() {
		scanersList.addActionListener(this);
		extractorParams.addActionListener(this);
	}

	void print(String str) {
		txtArea.append(str);
	}
}