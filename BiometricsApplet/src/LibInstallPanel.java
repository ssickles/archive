import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.Vector;

import javax.swing.BoxLayout;
import javax.swing.JButton;
import javax.swing.JCheckBox;
import javax.swing.JLabel;
import javax.swing.JPanel;

import com.neurotechnology.Library.NetInstall;
import com.neurotechnology.Library.ScannerFiles;

public class LibInstallPanel extends JPanel implements ActionListener {
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	FingersSampleApplet csa;
	JButton thisbutton;
	JPanel messagePanel;
	JCheckBox[] scanners;
	NetInstall netinstall;
	Vector<String> mainlibs;
	Vector<String> cmmlibs;
	Vector<ScannerFiles> scannersf;

	LibInstallPanel(FingersSampleApplet csa) {
		this.csa = csa;
		messagePanel = new JPanel();
		messagePanel.setLayout(new BoxLayout(messagePanel, BoxLayout.Y_AXIS));
		messagePanel.add(new JLabel("MegaMatcher Native libraries were not found on your computer."));
		messagePanel.add(new JLabel("*"));
		thisbutton = new JButton("Install");
		thisbutton.addActionListener(this);

		JPanel ospanel = new JPanel();
		ospanel.setLayout(new BoxLayout(ospanel, BoxLayout.Y_AXIS));

		System.out.println("Os name - " + System.getProperty("os.name"));
		System.out.println("Os arch - " + System.getProperty("os.arch"));

		try {
			if (System.getProperty("os.name").indexOf("Windows") != -1) {

				ospanel.add(new JLabel("Your operating system was identified as Windows."));
				ospanel.add(new JLabel(
						"If this is correct please choose scanners whitch you are willing to use and press install"));

				netinstall = new NetInstall();
				mainlibs = netinstall.getMainLibrariesWindows();
				scannersf = netinstall.getScannerLibrariesWindows();
				cmmlibs = netinstall.getCmmLibrariesWindows();
				scanners = new JCheckBox[scannersf.size()];
				JPanel scannerpanel = new JPanel();
				scannerpanel.setLayout(new GridLayout(7, 4));
				for (int i = 0; i < scannersf.size(); i++) {
					scanners[i] = new JCheckBox(scannersf.get(i).getName());
					scannerpanel.add(scanners[i]);
				}

				ospanel.add(scannerpanel);
			} else if (System.getProperty("os.name").indexOf("Linux") != -1) {
				ospanel.add(new JLabel("Your operating system was identified as Linux."));
				ospanel.add(new JLabel("If this is correct press install"));

				netinstall = new NetInstall();
				mainlibs = netinstall.getMainLibrariesLinux();
				cmmlibs = netinstall.getCmmLibrariesLinux();
			} else {
				System.out.println("Sorry your operating system is not supported");
				thisbutton.setEnabled(false);
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		messagePanel.add(ospanel);
		messagePanel.add(thisbutton);
		this.add(messagePanel);
	}

	public void actionPerformed(ActionEvent e) {

		final JLabel label = new JLabel("Installing - 0%");
		final JPanel owner = this;

		messagePanel.removeAll();
		messagePanel.add(label);
		this.updateUI();
		(new Thread() {
			public void run() {
				try {
					Vector<ScannerFiles> selectedscanners = new Vector<ScannerFiles>();
					if (scanners != null)
						for (int i = 0; i < scanners.length; i++)
							if (scanners[i].isSelected())
								selectedscanners.add(scannersf.get(i));
					netinstall.installTemp(csa.getFileSource(), mainlibs, selectedscanners, cmmlibs);
				} catch (Exception e) {
					e.printStackTrace();
				}

				if (!netinstall.checkLoadTemp()) {
					System.out.println("Failed to install libraries");
					messagePanel.removeAll();
					messagePanel.add(new JLabel("Failed to load libraries"));
				} else {
					messagePanel.removeAll();
					System.out.println("Libraries installed successfully. Loading application...");
					messagePanel.add(new JLabel("Libraries installed successfully. Loading application..."));
					updateUI();
					csa.activateMW();
				}
				System.out.println("instalation thread closed");
			}
		}).start();
		(new Thread() {
			public void run() {
				while (netinstall.getProgress() != 100) {
					try {
						Thread.sleep(2000);
					} catch (Exception e) {
					}
					label.setText("Installing - " + netinstall.getProgress() + "%");
					owner.updateUI();
				}
			}
		}).start();
	}

}
