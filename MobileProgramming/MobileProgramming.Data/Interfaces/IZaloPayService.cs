﻿namespace MobileProgramming.Data.Interfaces
{
    public interface IZaloPayService
    {
        Task<Dictionary<string, object>> CreateOrderAsync(string amount, string appUser, string description, string app_trans_id);
        bool ValidateMac(string dataStr, string reqMac);
        Dictionary<string, object> DeserializeData(string dataStr);
        Task<Dictionary<string, object>> QueryOrderStatus(string appTransId);
    }
}
