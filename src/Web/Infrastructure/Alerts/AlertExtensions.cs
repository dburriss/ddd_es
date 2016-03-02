using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ViewFeatures;
using System.Collections.Generic;
using BetterSession.AspNet.Mvc;

namespace Web
{
    public static class AlertExtensions
	{
		public const string Alerts = "_Alerts";

		public static List<Alert> GetAlerts(this ITempDataDictionary tempData)
		{
            if (tempData == null)
                return new List<Alert>();

			if (!tempData.ContainsKey(Alerts))
			{
                tempData.Set(Alerts, new List<Alert>());
			}

            return tempData.Get<List<Alert>>(Alerts);
		}

        public static void SetAlert(this ITempDataDictionary tempData, Alert alert)
        {
            if(tempData != null && alert != null)
                tempData.Set(Alerts, new List<Alert> { alert });
        }

        public static void AddAlert(this ITempDataDictionary tempData, Alert alert)
        {
            if (!tempData.ContainsKey(Alerts))
            {
                tempData.Set(Alerts, new List<Alert>());
            }
            var alerts = tempData.Get<List<Alert>>(Alerts);
            alerts.Add(alert);
            tempData.Set(Alerts, alerts);
        }

        public static IActionResult WithSuccess(this IActionResult result, string message, bool appendToExistingMessages = false)
		{
			return new AlertDecoratorResult(result, "alert-success", message, appendToExistingMessages);
		}

		public static IActionResult WithInfo(this IActionResult result, string message, bool appendToExistingMessages = false)
		{
			return new AlertDecoratorResult(result, "alert-info", message,appendToExistingMessages);
		}

		public static IActionResult WithWarning(this IActionResult result, string message, bool appendToExistingMessages = false)
		{
			return new AlertDecoratorResult(result, "alert-warning", message, appendToExistingMessages);
		}

		public static IActionResult WithError(this IActionResult result, string message, bool appendToExistingMessages = false)
		{
			return new AlertDecoratorResult(result, "alert-danger", message);
		}

        //public static void SetAsJson<T>(this ITempDataDictionary tempData, string key, T data)
        //{
        //    var sData = JsonConvert.SerializeObject(data);
        //    tempData[key] = sData;
        //}

        //public static T GetFromJson<T>(this ITempDataDictionary tempData, string key)
        //{
        //    if(tempData.ContainsKey(key))
        //    {
        //        var v = tempData[key];

        //        if(v is T)
        //        {
        //            return (T)v;
        //        }

        //        if(v is string && typeof(T) != typeof(string))
        //        {
        //            var obj = JsonConvert.DeserializeObject<T>((string)v);
        //            return obj;
        //        }
                
        //    }

        //    return default(T);
        //}
    }
}