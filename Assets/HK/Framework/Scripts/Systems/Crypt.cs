using System.IO;
using System.Security.Cryptography;

namespace HK.Framework.Systems
{
	/// <summary>
	/// Crypt.
	/// http://magnaga.com/unity/2016/01/11/unity-save/
	/// </summary>
	public class Crypt
	{

		private const string AesIV = @"Yg7FavTgpQl30UbX";
		private const string AesKey = @"1GboYhc8SmU4g0yb";

		public static string Encrypt(string text)
		{

			RijndaelManaged aes = new RijndaelManaged();
			aes.BlockSize = 128;
			aes.KeySize = 128;
			aes.Padding = PaddingMode.Zeros;
			aes.Mode = CipherMode.CBC;
			aes.Key = System.Text.Encoding.UTF8.GetBytes(AesKey);
			aes.IV = System.Text.Encoding.UTF8.GetBytes(AesIV);

			ICryptoTransform encrypt = aes.CreateEncryptor();
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptStream = new CryptoStream(memoryStream, encrypt, CryptoStreamMode.Write);

			byte[] text_bytes = System.Text.Encoding.UTF8.GetBytes(text);

			cryptStream.Write(text_bytes, 0, text_bytes.Length);
			cryptStream.FlushFinalBlock();

			byte[] encrypted = memoryStream.ToArray();

			return (System.Convert.ToBase64String(encrypted));
		}

		public static string Decrypt(string cryptText)
		{
			RijndaelManaged aes = new RijndaelManaged();
			aes.BlockSize = 128;
			aes.KeySize = 128;
			aes.Padding = PaddingMode.Zeros;
			aes.Mode = CipherMode.CBC;
			aes.Key = System.Text.Encoding.UTF8.GetBytes(AesKey);
			aes.IV = System.Text.Encoding.UTF8.GetBytes(AesIV);

			ICryptoTransform decryptor = aes.CreateDecryptor();

			byte[] encrypted = System.Convert.FromBase64String(cryptText);
			byte[] planeText = new byte[encrypted.Length];

			MemoryStream memoryStream = new MemoryStream(encrypted);
			CryptoStream cryptStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

			cryptStream.Read(planeText, 0, planeText.Length);

			return (System.Text.Encoding.UTF8.GetString(planeText));
		}
	}
}
