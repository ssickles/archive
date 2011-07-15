import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.ObjectOutputStream;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLConnection;

import javax.swing.BoxLayout;
import javax.swing.JButton;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTextArea;

import IdentityStream.DataModels.Template;

@SuppressWarnings("serial")
public class MainWindow extends JPanel implements ActionListener
{
	private JPanel _mainPanel;
	private JButton _butVerify;
	private JTextArea _txtArea;
	private URL _hostUrl;
	private final String _servletPath = "/HelloWorld/hello";
	
	public MainWindow(URL codeBase)
	{
		_hostUrl = codeBase;
		
		_mainPanel = new JPanel();
		_mainPanel.setPreferredSize(new Dimension(790, 200));
		_mainPanel.setLayout(new BoxLayout(_mainPanel, BoxLayout.PAGE_AXIS));

		_butVerify = new JButton("Verify");
		_butVerify.addActionListener(this);
		_mainPanel.add(_butVerify);
		
		_txtArea = new JTextArea();
		JScrollPane scrollPan = new JScrollPane(_txtArea);
		_mainPanel.add(scrollPan);
		this.add(_mainPanel, BorderLayout.CENTER);
	}

	@Override
	public void actionPerformed(ActionEvent ae)
	{
		if (ae.getActionCommand().equals("Verify"))
		{
			performVerify();
		}
	}
	
	private void performVerify()
	{
		log("Performing biometric verification.");
			try 
			{
				String location = getUrl();
				URL servlet = new URL(location);
				URLConnection servletConnection = servlet.openConnection();
				
				servletConnection.setDoInput(true);
				servletConnection.setDoOutput(true);
				
				servletConnection.setUseCaches(false);
				servletConnection.setDefaultUseCaches(false);
				
				servletConnection.setRequestProperty("Content-Type", "application/octet-stream");
				
				sendTemplate(servletConnection, new Template("L1"));
				
				receiveResponse(servletConnection);
			} 
			catch (MalformedURLException e)
			{
				log(e.toString());
			}
			catch (IOException e)
			{
				log(e.toString());
			}
		log("Biometric verification complete.");
	}
	
	private String getUrl()
	{
		String hostName = _hostUrl.getHost();
        int port = _hostUrl.getPort();
		
		if (port == -1)
		{
			port = 80;
		}
		
        log("Web Server host name: " + hostName);
        
        String webServerStr = "http://" + hostName + ":" + port + _servletPath;
        log("Servlet Url full = " + webServerStr);
        return webServerStr;
    }
	
	private void sendTemplate(URLConnection connection, Template template)
	{
		try
		{
			ObjectOutputStream output = new ObjectOutputStream(connection.getOutputStream());
			output.writeObject(template);
			output.flush();
			output.close();
			log("Sent template to servlet for verification.");
		}
		catch (IOException e)
		{
			log(e.toString());
		}
	}
	
	private void receiveResponse(URLConnection connection)
	{
		try 
		{
			BufferedReader input = new BufferedReader(new InputStreamReader(connection.getInputStream()));
			String result;
			while (((result = input.readLine())) != null)
			{
				log("Reading response... " + result);
			}
			input.close();
		}
		catch (IOException e)
		{
			log(e.toString());
		}
	}
	
	private void log(String message)
	{
		_txtArea.append(message + "\n");
	}
}
