using CLDV_POE_Web_Application.Models;
using CLDV_POE_Web_Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CLDV_POE_Web_Application.Controllers
{
	public class HomeController : Controller
	{
		private readonly BlobService _blobService;
		private readonly TableService _tableService;
		private readonly QueueService _queueService;
		private readonly FileService _fileService;

		public HomeController(BlobService blobService, TableService tableService, QueueService queueService, FileService fileService)
		{
			_blobService = blobService;
			_tableService = tableService;
			_queueService = queueService;
			_fileService = fileService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UploadImage(IFormFile file)
		{
			if (file != null)
			{
				using var stream = file.OpenReadStream();
				await _blobService.UploadBlobAsync("st10257863blobservice", file.FileName, stream);
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile)
		{
			if (ModelState.IsValid)
			{
				await _tableService.AddEntityAsync(profile);
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> ProcessOrder(string orderId)
		{
			await _queueService.SendMessageAsync("st10257863queueservice", $"Processing order {orderId}");
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> UploadContract(IFormFile file)
		{
			if (file != null)
			{
				using var stream = file.OpenReadStream();
				await _fileService.UploadFileAsync("st10257863fileservice", file.FileName, stream);
			}
			return RedirectToAction("Index");
		}
	}
}
