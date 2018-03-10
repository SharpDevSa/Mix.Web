using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotOrg.Db.Entities;
using DotOrg.Web.Ozi.PagesSettings.Forms;

namespace DotOrg.Web.Ozi.Services
{
	public interface IFileService
	{
		void SaveFile(Stream stream, UploadFileSettings context, IFileEntity file, string sourceName);
		void DeleteFile(IFileEntity file, UploadFileSettings context);
		string GetThumbUrl(IFileEntity file, params object[] parameters);
		string GetUrl(IFileEntity file);
	}
}
