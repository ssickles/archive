import java.awt.BorderLayout;
import java.net.URL;

import javax.swing.JApplet;
import javax.swing.JPanel;

@SuppressWarnings("serial")
public class ComApplet extends JApplet 
{
	JPanel mainpanel;

	public void init() 
	{
		mainpanel = new JPanel();
		this.setContentPane(mainpanel);
		URL codeBase = getCodeBase();
		MainWindow mw = new MainWindow(codeBase);
		mainpanel.add(mw, BorderLayout.CENTER);
		mainpanel.updateUI();
	}
}
