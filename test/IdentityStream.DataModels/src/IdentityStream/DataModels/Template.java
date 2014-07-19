package IdentityStream.DataModels;

import java.io.Serializable;

@SuppressWarnings("serial")
public class Template implements Serializable
{
	protected String _unitCode;
	
	public Template(String UnitCode)
	{
		_unitCode = UnitCode;
	}
	
	public String getUnitCode()
	{
		return _unitCode;
	}
}
