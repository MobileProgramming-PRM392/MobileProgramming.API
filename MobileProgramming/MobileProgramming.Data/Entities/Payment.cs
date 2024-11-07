using System;
using System.Collections.Generic;

namespace MobileProgramming.Data.Entities;

public partial class Payment
{
    public string PaymentId { get; set; } = string.Empty;

    public int? OrderId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public virtual Order? Order { get; set; }
}
