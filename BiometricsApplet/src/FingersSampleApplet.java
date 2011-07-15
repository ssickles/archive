import java.awt.BorderLayout;

import javax.swing.JApplet;
import javax.swing.JPanel;

import com.neurotechnology.Library.NetInstall;

public class FingersSampleApplet extends JApplet {

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	MainWindow mw;
	JPanel mainpanel;

	public void init() {
		System.out.println("init applet\n");
		mainpanel = new JPanel();
		mainpanel.setLayout(new BorderLayout());
		this.setContentPane(mainpanel);

		try
		{
			// 1: trying to load libraries easy way, this work only if client have
			// them in system path
			if (!NetInstall.checkLoadDefault()) {
				// 2: trying to load libraries from user.home directory, this work
				// only if client had installed them before
				NetInstall netinstall = null;
				try {
					netinstall = new NetInstall();
				} catch (Exception e) {
					e.printStackTrace();
				}
				if (!netinstall.checkLoadTemp()) {
					mainpanel.add(new LibInstallPanel(this), BorderLayout.CENTER);
				} else
					activateMW();
			} else
				activateMW();
		}
		catch (Exception e)
		{
			e.printStackTrace();
		}
	}

	// Directory where you hold nerotechnology libraries
	// It is done this way just to ease usage of sample.
	// It is advised to put file source directories in .html file or in
	// configuration file
	// so it would not be dependent on file catalog structure on server
	public String getFileSource() {
		if (System.getProperty("os.name").indexOf("Windows") != -1) {
			if (System.getProperty("os.arch").indexOf("64") != -1)
				return getCodeBase() + "../win64_x64/";
			else
				return getCodeBase() + "../win32_x86/";
		}
		if (System.getProperty("os.name").indexOf("Linux") != -1) {
			if (System.getProperty("os.arch").indexOf("64") != -1)
				return getCodeBase() + "../../lib/linux_x86_64/";
			else
				return getCodeBase() + "../../lib/linux_x86/";
		}
		return null;
	}

	void activateMW() {

		mw = new MainWindow();
		System.out.println("Main Window created\n");
		mainpanel.removeAll();
		mainpanel.add(mw, BorderLayout.CENTER);
		mainpanel.updateUI();
	}

	public void stop() {
		if (mw != null)
			MainWindow.scanMan.uninitialize();
	}
}
