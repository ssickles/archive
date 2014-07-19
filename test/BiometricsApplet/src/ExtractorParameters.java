import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.*;

import com.neurotechnology.Fingers.NFExtractor;
import com.neurotechnology.Fingers.TemplateSize;
import com.neurotechnology.NMatcher.MatcherMode;

public class ExtractorParameters extends JFrame implements ActionListener {
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;

	JButton ok;
	JButton cancel;

	JTextField genThreshold;
	JTextField genMaxRotation;
	JTextField qualThreshold;
	JTextField genPicCount;
	JComboBox templaesize;
	JTextField extractedRidgeCount;
	JComboBox matchMode;
	JCheckBox useQuality;

	NFExtractor extractor;

	ExtractorParameters(NFExtractor extractor) {
		try {
			extractor.updateParameters();
		} catch (Exception e) {
		}
		;
		this.extractor = extractor;
		this.setTitle("Extractor Parameters");
		this.setSize(250, 290);
		JPanel mainPanel = new JPanel();
		mainPanel.setLayout(new GridLayout(11, 2));
		genThreshold = new JTextField(new Integer(extractor.getGeneralizationThreshold()).toString());
		genMaxRotation = new JTextField(new Integer(extractor.getGeneralizationMaximalRotation()).toString());
		qualThreshold = new JTextField(new Integer(extractor.getQualityThreshhold()).toString());
		genPicCount = new JTextField(new Integer(extractor.getGenPicCount()).toString());
		useQuality = new JCheckBox("", extractor.isUsequality());
		matchMode = new JComboBox(MatcherMode.values());
		templaesize = new JComboBox(TemplateSize.values());
		extractedRidgeCount = new JTextField(new Integer(extractor.getExtractedRidgeCounts()).toString());
		templaesize.setSelectedItem(extractor.getTemplateSize());
		matchMode.setSelectedItem(extractor.getMode());
		ok = new JButton("OK");
		cancel = new JButton("Cancel");
		ok.addActionListener(this);
		cancel.addActionListener(this);

		mainPanel.add(new JLabel("Generalization"));
		mainPanel.add(new JLabel());
		mainPanel.add(new JLabel("Threshold"));
		mainPanel.add(genThreshold);
		mainPanel.add(new JLabel("Maximal rotation"));
		mainPanel.add(genMaxRotation);
		mainPanel.add(new JLabel("Use generalization of"));
		mainPanel.add(genPicCount);
		mainPanel.add(new JLabel("Template Size"));
		mainPanel.add(templaesize);
		mainPanel.add(new JLabel("Extracted Ridge Counts"));
		mainPanel.add(extractedRidgeCount);
		mainPanel.add(new JLabel("Quality"));
		mainPanel.add(new JLabel());
		mainPanel.add(new JLabel("Use quality"));
		mainPanel.add(useQuality);
		mainPanel.add(new JLabel("Threshold"));
		mainPanel.add(qualThreshold);
		mainPanel.add(new JLabel("Mode"));
		mainPanel.add(matchMode);
		mainPanel.add(ok);
		mainPanel.add(cancel);

		this.setContentPane(mainPanel);
		this.setVisible(true);
	}

	public void actionPerformed(ActionEvent e) {
		if (e.getSource() == ok) {
			extractor.setGeneralizationMaximalRotation(new Integer(genMaxRotation.getText()));
			extractor.setGeneralizationThreshold(new Integer(genThreshold.getText()));
			extractor.setMode((MatcherMode) matchMode.getSelectedItem());
			extractor.setQualityThreshhold(new Integer(qualThreshold.getText()));
			extractor.setUsequality(useQuality.isEnabled());
			extractor.setGenPicCount(new Integer(genPicCount.getText()));
			extractor.setExtractedRidgeCounts(new Integer(extractedRidgeCount.getText()));
			extractor.setTemplateSize((TemplateSize) templaesize.getSelectedItem());
			this.dispose();
		}
		if (e.getSource() == cancel) {
			this.dispose();
		}
	}

}
