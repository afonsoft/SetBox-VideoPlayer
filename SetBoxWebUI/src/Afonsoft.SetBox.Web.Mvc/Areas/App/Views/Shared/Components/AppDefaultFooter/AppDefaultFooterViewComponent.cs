﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Afonsoft.SetBox.Web.Areas.App.Models.Layout;
using Afonsoft.SetBox.Web.Session;
using Afonsoft.SetBox.Web.Views;

namespace Afonsoft.SetBox.Web.Areas.App.Views.Shared.Components.AppDefaultFooter
{
    public class AppDefaultFooterViewComponent : SetBoxViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppDefaultFooterViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(footerModel);
        }
    }
}
