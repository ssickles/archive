package IdentityStream.Proxies.ApplicationService;

import test.ws.ApplicationServiceStub;

public class Main {
	public static void main(String [] args) throws Exception {
		ApplicationServiceStub stub = new ApplicationServiceStub();
		
		ApplicationServiceStub.GetIdentityByName idByName = new ApplicationServiceStub.GetIdentityByName();
		idByName.setFullName("Administrator");
		ApplicationServiceStub.GetIdentityByNameResponse resp = stub.getIdentityByName(idByName);
		if (resp.getGetIdentityByNameResult() != null) {
			System.out.println(resp.getGetIdentityByNameResult().getUid());
		}
		else {
			System.out.println("Null identityByNameResult");
		}
	}
}
