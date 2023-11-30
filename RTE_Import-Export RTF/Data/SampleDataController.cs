using Microsoft.AspNetCore.Mvc;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.Text.RegularExpressions;

namespace RTE_Import_Export_RTF.Data
{
	[Route("api/[controller]")]
	[ApiController]
	public class SampleDataController : ControllerBase
	{
		private IWebHostEnvironment hostingEnv;

		public SampleDataController(IWebHostEnvironment env)
		{
			this.hostingEnv = env;
		}


		[HttpPost]
		[Route("Import")]
		public void Import(IList<IFormFile> UploadFiles)
		{
			if (UploadFiles != null)
			{
				var file = UploadFiles[0];

				string HtmlString = string.Empty;

				using (var mStream = new MemoryStream())
				{
					new WordDocument(file.OpenReadStream(), FormatType.Rtf).Save(mStream, FormatType.Html);
					mStream.Position = 0;
					HtmlString = new StreamReader(mStream).ReadToEnd();
				}
				
				string extract = ExtractBodyContent(HtmlString);
				var str = extract.Replace("\r\n", "");
				Response.Headers.Add("rteValue", str);
			}
		}

		public string ExtractBodyContent(string html)
		{
			if (html.Contains("<html") && html.Contains("<body"))
			{
				return html.Remove(0, html.IndexOf("<body>") + 6).Replace("</body></html>", "");
			}
			return html;
		}
	}
}
