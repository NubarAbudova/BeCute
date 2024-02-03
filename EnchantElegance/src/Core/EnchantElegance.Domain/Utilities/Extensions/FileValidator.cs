using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantElegance.Domain.Utilities.Extensions
{
	//public static class FileValidator
	//{
	//	public static bool ValidateType(this IFormFile file, string type)
	//	{
	//		return file.ContentType.Contains(type);
	//	}
	//	public static bool ValidateSize(this IFormFile file, int kb)
	//	{
	//		return file.Length < kb * 1024;
	//	}

	//	public static async Task<string> CreateFileAsync(this IFormFile file, string root, params string[] folder)
	//	{
	//		string filename = Guid.NewGuid().ToString() + file.FileName;
	//		string path = root;
	//		for (int i = 0; i < folder.Length; i++)
	//		{
	//			path = Path.Combine(path, folder[i]);
	//		}
	//		path = Path.Combine(path, filename);

	//		using (FileStream stream = new FileStream(path, FileMode.Create))
	//		{
	//			await file.CopyToAsync(stream);
	//		}

	//		return (filename);
	//	}
	//	public static void DeleteFile(string filename, string root, params string[] folder)
	//	{
	//		string path = root;
	//		for (int i = 0; i < folder.Length; i++)
	//		{
	//			path = Path.Combine(path, folder[i]);
	//		}
	//		path = Path.Combine(path, filename);

	//		if (File.Exists(path))
	//		{
	//			File.Delete(path);
	//		}

	//	}
	//}
}