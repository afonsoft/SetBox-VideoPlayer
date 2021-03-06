﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SetBoxWebUI.Helpers;
using SetBoxWebUI.Interfaces;
using SetBoxWebUI.Models;
using SetBoxWebUI.Models.Views;
using SetBoxWebUI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SetBoxWebUI.Controllers
{
    [Authorize]
    public class DevicesController : BaseController
    {
        private readonly ILogger<DevicesController> _logger;
        private readonly IRepository<Device, Guid> _devices;
        private readonly FileDeviceRepository fileDeviceRepository;
        /// <summary>
        /// SetBoxController
        /// </summary>
        public DevicesController(ILogger<DevicesController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _devices = new Repository<Device, Guid>(context);
            fileDeviceRepository = new FileDeviceRepository(context);
        }
       
        public IActionResult Index(DeviceViewModel m)
        {
            ViewData["Edit"] = false;
            if (m == null) m = new DeviceViewModel();
            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string command)
        {
            try
            {
                ViewData["Edit"] = true;
                ViewData["Command"] = command;
                var item = await _devices.FirstOrDefaultAsync(x => x.DeviceId.ToString() == id);

                if (item == null)
                    throw new KeyNotFoundException($"DeviceId: {id} not found.");

                DeviceViewModel model = new DeviceViewModel
                {
                    IsEdited = command == "Edit",
                    DeviceId = item.DeviceId,
                    DeviceIdentifier = item.DeviceIdentifier,
                    CreationDateTime = item.CreationDateTime,
                    License = item.License,
                    Platform = item.Platform,
                    Version = item.Version,
                    Name = item.Name,
                    ApkVersion = item.ApkVersion,
                    DeviceName = item.DeviceName,
                    Manufacturer = item.Manufacturer,
                    Model = item.Model,
                    LastAccessDateTime = item.LastAccessDateTime,
                    Session = CriptoHelpers.Base64Encode(item.DeviceId.ToString()),
                    Files = item.Files.OrderBy(x => x.Order).Select(x => x.File).ToList()
                };

                if (item.Configuration != null)
                {
                    model.TransactionTime = item.Configuration.TransactionTime;
                    model.EnablePhoto = item.Configuration.EnablePhoto;
                    model.EnableTransaction = item.Configuration.EnableTransaction;
                    model.EnableVideo = item.Configuration.EnableVideo;
                    model.EnableWebImage = item.Configuration.EnableWebImage;
                    model.EnableWebVideo = item.Configuration.EnableWebVideo;
                    model.ConfigId = item.Configuration.ConfigId;
                    model.GoogleDrivePassword = item.Configuration.GoogleDrivePassword;
                    model.GoogleDriveUrl = item.Configuration.GoogleDriveUrl;
                    model.GoogleDriveUserName = item.Configuration.GoogleDriveUserName;
                }
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                ViewData["Edit"] = false;
                return View("Index", new DeviceViewModel(ex));
            }
        }

        [HttpPost]
        public async Task<bool> UpdateOrderFiles(string id, string order)
        {
            try
            {
                if (!string.IsNullOrEmpty(order) && !string.IsNullOrEmpty(id))
                {
                    string[] orders = order.Replace("[]=", "-").Split("&");
                    if (orders.Length > 0)
                    {
                        var device = await _devices.FirstOrDefaultAsync(x => x.DeviceId.ToString() == id);
                        if (device != null)
                        {
                            for (int idx = 0; idx < orders.Length; idx++) 
                            {
                               await fileDeviceRepository.UpdateOrder(id, orders[idx], idx);
                            }

                            await _devices.UpdateAsync(device);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return true;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(DeviceViewModel m)
        {
            try
            {
                if (m != null)
                {
                    var updItem = await _devices.FirstOrDefaultAsync(x => x.DeviceId == m.DeviceId);

                    if (updItem != null)
                    {
                        updItem.Name = m.Name;

                        if (updItem.Configuration == null)
                        {
                            updItem.Configuration = new Config
                            {
                                CreationDateTime = DateTime.Now
                            };
                        }
                        updItem.Configuration.EnablePhoto = m.EnablePhoto;
                        updItem.Configuration.EnableTransaction = m.EnableTransaction;
                        updItem.Configuration.EnableVideo = m.EnableVideo;
                        updItem.Configuration.EnableWebImage = m.EnableWebImage;
                        updItem.Configuration.EnableWebVideo = m.EnableWebVideo;
                        updItem.Configuration.TransactionTime = m.TransactionTime;
                        updItem.Configuration.GoogleDriveUserName = m.GoogleDriveUserName;
                        updItem.Configuration.GoogleDrivePassword = m.GoogleDrivePassword;
                        updItem.Configuration.GoogleDriveUrl = m.GoogleDriveUrl;

                        await _devices.UpdateAsync(updItem);

                        ViewData["Edit"] = false;
                        return RedirectToAction("Index", new DeviceViewModel("Device update was successful."));
                    }
                    else
                    {
                        ViewData["Edit"] = false;
                        throw new KeyNotFoundException($"DeviceId: {m.DeviceId} not found.");
                    }
                }
                ViewData["Edit"] = false;
                return View("Index", new DeviceViewModel());

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                ViewData["Edit"] = false;
                return View("Index", new DeviceViewModel(ex));
            }
        }
        //Device

        public async Task<string> Key(string identifier, string session)
        {
            var deviceId = CriptoHelpers.Base64Decode(session);
            var devices = await _devices.FirstOrDefaultAsync(f => f.DeviceId.ToString() == deviceId);
            if (devices != null)
            {
                devices.License = CriptoHelpers.MD5HashString(identifier);
                await _devices.UpdateAsync(devices);
                return devices.License;
            }
            return "";
        }

        public async Task<GridPagedOutput<FileCheckSum>> ListFiles(GridPagedInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.SearchPhrase))
                    input.SearchPhrase = "";

                if (string.IsNullOrEmpty(input.Id))
                    throw new KeyNotFoundException($"DeviceId: {input.Id} not found.");

                var devices = await _devices.GetAsync(f => f.DeviceId.ToString() == input.Id);

                if (devices.Count <= 0)
                    throw new KeyNotFoundException($"DeviceId: {input.Id} not found.");

                var files = devices[0].Files.Where(l => (l.FileId.ToString() == input.Id
                                                        || l.DeviceId.ToString() == input.Id)
                                                    && (l.CreationDateTime.ToString("dd/MM/yyyy").Contains(input.SearchPhrase)
                                                        || l.File.Name.Contains(input.SearchPhrase)
                                                        || l.File.Path.Contains(input.SearchPhrase)
                                                        || l.File.Extension.Contains(input.SearchPhrase)))
                                            .Select(x => x.File)
                                            .Skip((input.Current - 1) * input.RowCount)
                                            .Take(input.RowCount)
                                            .ToList();

                var item = new GridPagedOutput<FileCheckSum>(files) { Current = input.Current, RowCount = input.RowCount, Total = devices[0].Files.Count };
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<GridPagedOutput<DeviceLogError>> ListLogError(GridPagedInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.SearchPhrase))
                    input.SearchPhrase = "";

                if (string.IsNullOrEmpty(input.Id))
                    throw new KeyNotFoundException($"DeviceId: {input.Id} not found.");

                var devices = await _devices.GetAsync(f => f.DeviceId.ToString() == input.Id);

                if (devices.Count <= 0)
                    throw new KeyNotFoundException($"DeviceId: {input.Id} not found.");

                var logs = devices[0].Logs.Where(l => l.DeviceLogId.ToString() == input.Id
                                                        || l.CreationDateTime.ToString("dd/MM/yyyy").Contains(input.SearchPhrase)
                                                        || l.IpAcessed.Contains(input.SearchPhrase)
                                                        || l.Message.Contains(input.SearchPhrase))
                                                 .Skip((input.Current - 1) * input.RowCount)
                                                 .Take(input.RowCount)
                                                 .ToList();

                var item = new GridPagedOutput<DeviceLogError>(logs) { Current = input.Current, RowCount = input.RowCount, Total = devices[0].Logs.Count };
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<GridPagedOutput<DeviceLogAccesses>> ListLog(GridPagedInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.SearchPhrase))
                    input.SearchPhrase = "";

                if (string.IsNullOrEmpty(input.Id))
                    throw new KeyNotFoundException($"DeviceId: {input.Id} not found.");

                var devices = await _devices.GetAsync(f => f.DeviceId.ToString() == input.Id);

                if (devices.Count <= 0)
                    throw new KeyNotFoundException($"DeviceId: {input.Id} not found.");

                var logs = devices[0].LogAccesses.Where(l => l.DeviceLogAccessesId.ToString() == input.Id
                                                        || l.CreationDateTime.ToString("dd/MM/yyyy").Contains(input.SearchPhrase)
                                                        || l.IpAcessed.Contains(input.SearchPhrase)
                                                        || l.Message.Contains(input.SearchPhrase))
                                                 .Skip((input.Current - 1) * input.RowCount)
                                                 .Take(input.RowCount)
                                                 .ToList();

                var item = new GridPagedOutput<DeviceLogAccesses>(logs) { Current = input.Current, RowCount = input.RowCount, Total = devices[0].LogAccesses.Count };
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<GridPagedOutput<Device>> List(GridPagedInput input)
        {

            try
            {
                if (string.IsNullOrEmpty(input.SearchPhrase))
                    input.SearchPhrase = "";

                Expression<Func<Device, object>> orderby = o => o.DeviceIdentifier;
                var keys = input.Sort.OrderBy(kvp => kvp.Key).First();

                switch (keys.Key)
                {
                    case "deviceIdentifier":
                        orderby = o => o.DeviceIdentifier;
                        break;
                    case "license":
                        orderby = o => o.License;
                        break;
                    case "platform":
                        orderby = o => o.Platform;
                        break;
                    case "version":
                        orderby = o => o.Version;
                        break;
                    case "name":
                        orderby = o => o.Name;
                        break;
                    case "deviceName":
                        orderby = o => o.DeviceName;
                        break;
                    case "model":
                        orderby = o => o.Model;
                        break;
                    case "manufacturer":
                        orderby = o => o.Manufacturer;
                        break;
                    case "apkVersion":
                        orderby = o => o.ApkVersion;
                        break;
                }

                var itens = new List<Device>();
                var devices = await _devices.GetPagination(f => f.DeviceId.ToString() == input.Id
                                           || f.DeviceIdentifier.Contains(input.SearchPhrase)
                                           || f.License.Contains(input.SearchPhrase)
                                           || f.Platform.Contains(input.SearchPhrase)
                                           || f.Version.Contains(input.SearchPhrase)
                                           || f.Name.Contains(input.SearchPhrase)
                                           || f.CreationDateTime.ToString("dd/MM/yyyy").Contains(input.SearchPhrase),
                                            keys.Value == "asc" ? orderby : null,
                                            keys.Value == "desc" ? orderby : null,
                                         input.Current,
                                         input.RowCount);

                itens.AddRange(devices.Value);
                var item = new GridPagedOutput<Device>(itens) { Current = input.Current, RowCount = input.RowCount, Total = devices.Key };
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<string> Delete(string id)
        {
            try
            {
                var del = await _devices.FirstOrDefaultAsync(x => x.DeviceId.ToString() == id);
                if (del != null)
                {
                   await _devices.DeleteAsync(del);
                    return "Device Deleted!";
                }
                return "Device Not Found!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ex.Message;
            }
        }
    }
}