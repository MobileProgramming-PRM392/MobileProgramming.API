using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using MobileProgramming.Data.ExternalServices.Payment.ZaloPay.Setting;
using MobileProgramming.Data.Helper.ZaloPayHelper;
using MobileProgramming.Data.Helper.ZaloPayHelper.Crypto;
using MobileProgramming.Data.Interfaces;
using Newtonsoft.Json;

namespace MobileProgramming.Data.ExternalServices.Payment.ZaloPay
{
    public class ZaloPayService : IZaloPayService
    {
        private readonly ZaloPaySetting _zaloPaySettings;

        public ZaloPayService(IOptions<ZaloPaySetting> zaloPaySettings)
        {
            _zaloPaySettings = zaloPaySettings.Value;
        }


        public async Task<Dictionary<string, object>> CreateOrderAsync(string amount, string description, string app_trans_id)
        {


            Random rnd = new Random();
            var embed_data = new {  };
            var items = new[] { new { } };
            var param = new Dictionary<string, string>();
            //var app_trans_idd = rnd.Next(1000000);

            param.Add("app_id", _zaloPaySettings.Appid);
            param.Add("app_user", "user123");
            param.Add("app_time", Utils.GetTimeStamp().ToString());
            param.Add("expire_duration_seconds", "1200");
            param.Add("amount", amount);
            param.Add("app_trans_id", app_trans_id); // mã giao dich có định dạng yyMMdd_xxxx
            param.Add("embed_data", JsonConvert.SerializeObject(embed_data));
            param.Add("item", JsonConvert.SerializeObject(items));
            param.Add("description", description + app_trans_id);
            param.Add("bank_code", "");

            var data = _zaloPaySettings.Appid + "|" + param["app_trans_id"] + "|" + param["app_user"] + "|" + param["amount"] + "|"
                        + param["app_time"] + "|" + param["embed_data"] + "|" + param["item"];
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _zaloPaySettings.Key1!, data));

            var result = await HttpHelper.PostFormAsync(_zaloPaySettings.CreateOrderUrl!, param);
            return result;
        }

        public bool ValidateMac(string dataStr, string reqMac)
        {
            try
            {

                var mac = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _zaloPaySettings.Key2, dataStr);
                return reqMac.Equals(mac);
            }
            catch
            {
                return false;
            }
        }

        public Dictionary<string, object> DeserializeData(string dataStr)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(dataStr);
        }

        public async Task<Dictionary<string, object>> QueryOrderStatus(string appTransId)
        {
            var param = new Dictionary<string, string>
                {
                    { "app_id", "553" },
                    { "app_trans_id", appTransId}
                };

            var data = $"{_zaloPaySettings.Appid}|{appTransId}|{_zaloPaySettings.Key1}";
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, $"{_zaloPaySettings.Key1}", data));

            return await HttpHelper.PostFormAsync("https://sb-openapi.zalopay.vn/v2/query", param);
        }

        //public async Task<Dictionary<string, object>> Refund(string zpTransId, string amount, string description)
        //{
        //    var timestamp = Utils.GetTimeStamp().ToString();
        //    var rand = new Random();
        //    var uid = timestamp + "" + rand.Next(111, 999).ToString();

        //    Dictionary<string, string> param = new Dictionary<string, string>();
        //    param.Add("app_id", _zaloPaySettings.Appid!);
        //    param.Add("m_refund_id", DateTime.Now.ToString("yyMMdd") + "_" + _zaloPaySettings.Appid! + "_" + uid);
        //    param.Add("zp_trans_id", zpTransId);
        //    param.Add("amount", amount);
        //    param.Add("timestamp", timestamp);
        //    param.Add("description", description);

        //    var data = _zaloPaySettings.Appid! + "|" + param["zp_trans_id"] + "|" + param["amount"] + "|" + param["description"] + "|" + param["timestamp"];
        //    param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _zaloPaySettings.Key1!, data));

        //    var result = await HttpHelper.PostFormAsync(_zaloPaySettings.RefundUrl!, param);
        //    return result;
        //}

    }
}
