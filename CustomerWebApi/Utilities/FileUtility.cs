using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace CustomerWebApi.Utilities
{
	public static class FileUtility
	{
		public static async Task<string> GetContent(string filePath)
		{
			string fileContent = String.Empty;

			if (File.Exists(filePath) == true)
			{
				fileContent = await File.ReadAllTextAsync(filePath);				
			}

			return fileContent;
		}

		public static async void SaveContent(string filePath , string content)
		{
			await File.WriteAllTextAsync(filePath, content);
		}

		public static string GetFilePath(IWebHostEnvironment webHostEnvironment)
		{

			return Path.Combine(webHostEnvironment.ContentRootPath, "CustomerJsonStore.json");
			

		}
	}
}
