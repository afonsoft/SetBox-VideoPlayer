﻿using System.Collections.Generic;
using Afonsoft.SetBox.Editions.Dto;
using Afonsoft.SetBox.MultiTenancy.Payments;

namespace Afonsoft.SetBox.Web.Models.Payment
{
    public class ExtendEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}