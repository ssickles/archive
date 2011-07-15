import com.neurotechnology.Fingers.ImpressionType;
import com.neurotechnology.Fingers.Position;
import com.neurotechnology.NImages.NImage;
import com.neurotechnology.ScannerMan.FingerPlaced;
import com.neurotechnology.ScannerMan.FingerRemoved;
import com.neurotechnology.ScannerMan.ImageScanned;
import com.neurotechnology.Templates.NFRecord;

public class Extractor implements FingerPlaced, FingerRemoved, ImageScanned {

	MainWindow mw;

	private String currScanner;

	Extractor(MainWindow mw) throws Exception {
		this.mw = mw;
	}

	public void fingerPlacedCallback() {
		mw.print(currScanner + " - finger placed\n");
	}

	public void fingerRemovedCallback() {
		mw.print(currScanner + " - finger removed\n");
	}

	public void imageScannedCallback(NImage image) {
		NFRecord rec = null;
		mw.print("Image captured.\n");
		try {
			rec = mw.extractor.ExtractFromUnpackedImage(image, Position.nfpUnknown, ImpressionType.nfitLiveScanPlain);
		} catch (Exception e) {
			mw.print(e.getMessage() + "\n");
			return;
		}
		mw.print("Extracted template.\n");
		mw.record = rec;
	}

	@Override
	public void setCurrScaner(String scannerName) {
		currScanner = scannerName;
	}
}
