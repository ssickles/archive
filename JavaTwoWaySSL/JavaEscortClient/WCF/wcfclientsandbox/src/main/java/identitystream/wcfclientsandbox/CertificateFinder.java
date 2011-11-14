package identitystream.wcfclientsandbox;

import java.security.KeyStore;
import java.security.KeyStoreException;
import java.security.NoSuchAlgorithmException;
import java.security.PrivateKey;
import java.security.UnrecoverableKeyException;
import java.security.cert.Certificate;

public class CertificateFinder {
	
	private KeyStore _Store;
	
	public void loadKeyStore(String keyStoreLocation, String storeType, String password) {
		
    	try {
    		_Store = KeyStore.getInstance(storeType);
    		
            java.io.FileInputStream fis = null;
            try {
                fis = new java.io.FileInputStream(keyStoreLocation);
                _Store.load(fis, password.toCharArray());
            } finally {
                if (fis != null) {
                    fis.close();
                }
            }
		} catch (Exception e) {
			e.printStackTrace();
			_Store = null;
		}
	}
	
	public Certificate getCertificateByAlias(String alias) {
    	if (_Store == null)
    		return null;
    	
		Certificate cert = null;
    	try {
			cert = _Store.getCertificate(alias);
		} catch (KeyStoreException e) {
			e.printStackTrace();
		}
		return cert;
	}
	
	public PrivateKey getPrivateKeyByAlias(String alias) {
    	if (_Store == null)
    		return null;
    	
		try {
			return (PrivateKey)_Store.getKey(alias, "pa$$w0rd".toCharArray());
		} catch (UnrecoverableKeyException e) {
			e.printStackTrace();
		} catch (KeyStoreException e) {
			e.printStackTrace();
		} catch (NoSuchAlgorithmException e) {
			e.printStackTrace();
		}
		return null;
	}
}
