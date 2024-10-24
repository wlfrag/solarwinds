**DGA Logic for b91ce2fa41029f6955bff20079468448**



**[Usage]**

```solarwinds.exe <encrypt|decrypt> <domain>```

**Encrypt**

![image](https://github.com/user-attachments/assets/498650a6-f038-4b2e-bbab-95df79d83e05)

**Decrypt**

![image](https://github.com/user-attachments/assets/a4467477-6a8f-4fe6-af81-bf37b899c99d)









------------------------------------------------
**Details**


![image](https://github.com/user-attachments/assets/6ea358d2-4bbc-4c40-aaac-a11ab55b8fd5)

With a domain name, calls GetOrCreateUserId()
- ReadDeviceInfo() for mac address
- Append Domain Name
- Add Registry key value:  HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\MachineGuid
- computes an MD5 hash of the value derive above and combines it with hash64 using XOR to produce a mixed output
![image](https://github.com/user-attachments/assets/abb4b734-5e73-4b03-8561-d17357452226)

CryptoHelper class:
  		this.guid = Enumerable.ToArray<byte>(userId);
			this.dnStr = CryptoHelper.DecryptShort(domain); //basically base64 encode domain
			this.offset = 0;
			this.nCount = 0;

With a UserID and domain name, calls GetPreviousString():
- calls CreateSecureString(guid,true)
  -  returns a Base64-encoded string representation of a byte array, using a random first byte and performing an XOR operation with the rest of guid
- extract dnssubstring if .dnStr is too long
- returns both values above + GetStatus() -> .appsync-api.eu-west-1.avsvmcloud.com




**Complete 1-1 function port from malicious .dll**


![image](https://github.com/user-attachments/assets/4378311c-cbf3-4b12-964e-5851d33db505)

![image](https://github.com/user-attachments/assets/cfff6a62-9c88-4049-a88b-1b53000cc7f3)
