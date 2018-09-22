﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FlubuCore.Context;
using FlubuCore.Tasks;
using FlubuCore.Tasks.Packaging;
using FlubuCore.WebApi.Controllers.Exceptions;
using FlubuCore.WebApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FlubuCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly ITaskFactory _taskFactory;

        private readonly ITaskSession _taskSession;

        public ReportsController(IHostingEnvironment hostingEnvironment, ITaskFactory taskFactory, ITaskSession taskSession)
        {
            _hostingEnvironment = hostingEnvironment;
            _taskFactory = taskFactory;
            _taskSession = taskSession;
        }

        /// <summary>
        /// Sends reports(compressed in zip file) that are on the flubu web api server.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("download")]
        public IActionResult DownloadReports([FromBody]DownloadReportsRequest request)
        {
            string downloadDirectory = Path.Combine(_hostingEnvironment.ContentRootPath, "Reports");

            if (!Directory.Exists(downloadDirectory))
            {
                Directory.CreateDirectory(downloadDirectory);
            }

            string dirName = "Reports";

            if (!string.IsNullOrEmpty(request.DownloadFromSubDirectory))
            {
                downloadDirectory = Path.Combine(downloadDirectory, request.DownloadFromSubDirectory);
                dirName = request.DownloadFromSubDirectory;
            }

            var zipDirectory = Path.Combine(downloadDirectory, "output");

            var task = _taskFactory.Create<PackageTask>(zipDirectory);

            string zipFilename = string.IsNullOrEmpty(request.DownloadFromSubDirectory)
                ? "Reports.zip"
                : $"{request.DownloadFromSubDirectory}.zip";

            if (Directory.GetFiles(downloadDirectory).Length == 0)
            {
                throw new HttpError(HttpStatusCode.NotFound, "NoReportsFound");
            }

            task.AddDirectoryToPackage(downloadDirectory, dirName, false).ZipPackage(zipFilename, false).Execute(_taskSession);
            string zipPath = Path.Combine(zipDirectory, zipFilename);

            Stream fs = System.IO.File.OpenRead(zipPath);
            return File(fs, "application/zip", zipFilename);
        }

        /// <summary>
        /// Deletes all reports(cleans directory on flubu web api server).
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("download")]
        public IActionResult CleanPackagesDirectory([FromBody]CleanPackagesDirectoryRequest request)
        {
            var downloadDirectory = Path.Combine(_hostingEnvironment.ContentRootPath, "Reports");

            if (!string.IsNullOrWhiteSpace(request.SubDirectoryToDelete))
            {
                downloadDirectory = Path.Combine(downloadDirectory, request.SubDirectoryToDelete);
            }

            try
            {
                if (Directory.Exists(downloadDirectory))
                {
                    Directory.Delete(downloadDirectory, true);
                }
            }
            catch (IOException)
            {
                Thread.Sleep(1000);
                Directory.Delete(downloadDirectory, true);
            }

            Directory.CreateDirectory(downloadDirectory);

            return Ok();
        }
    }
}
