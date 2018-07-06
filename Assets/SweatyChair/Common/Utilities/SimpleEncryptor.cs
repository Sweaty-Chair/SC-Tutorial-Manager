using UnityEngine;
using System;
using System.Text;

public static class SimpleEncryptor
{

	// Change this for different project
	private const long KEY_ENCRYPT_DB = 156941202438; // A random integer
	
	static private byte[] _securityCode = null;

	static private byte[] securityCode {
		get {
			if (_securityCode == null) {
				_securityCode = new byte[4];
				_securityCode [0] = (byte)(KEY_ENCRYPT_DB & 0x000000ff);
				_securityCode [1] = (byte)((KEY_ENCRYPT_DB & 0x0000ff00) >> 8);
				_securityCode [2] = (byte)((KEY_ENCRYPT_DB & 0x00ff0000) >> 16);
				_securityCode [3] = (byte)((KEY_ENCRYPT_DB & 0xff000000) >> 24);
			}
			return _securityCode;
		}
	}

	public static byte[] Encrypt (string s)
	{
		return Encrypt (Encoding.UTF8.GetBytes (s));
	}

	public static byte[] Encrypt (byte[] bytes)
	{
		byte[] scode = securityCode;
		byte[] outBytes = new byte[bytes.Length];
		
		int i = 0;
		byte b;
		while (i < bytes.Length) {
			for (int j = 0; j < 4; j++) {
				if (i + j >= bytes.Length)
					break;
				b = bytes [i + j];
				b ^= scode [j];
				outBytes [i + j] = b;
			}
			i += 4;
		}
		return outBytes;
	}
	
	public static string Decrypt (byte[] bytes)
	{
		byte[] scode = securityCode;
		StringBuilder outSb = new StringBuilder (bytes.Length);
		
		int i = 0;
		byte b;
		char c;
		while (i < bytes.Length) {
			for (int j = 0; j < 4; j++) {
				if (i + j >= bytes.Length)
					break;
				b = bytes [i + j];
				c = (char)(b ^ scode [j]);
				outSb.Append (c);
			}
			i += 4;
		}
		return outSb.ToString ();
	}

}