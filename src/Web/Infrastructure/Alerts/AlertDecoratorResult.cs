using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Web
{
    public class AlertDecoratorResult : ActionResult, IKeepTempDataResult, IActionResult
    {
		public IActionResult InnerResult { get; set; }
		public string AlertClass { get; set; }
		public string Message { get; set; }
        public bool AppendToExistingMessages { get; private set; }

        public AlertDecoratorResult(IActionResult innerResult, string alertClass, string message, bool appendToExistingMessages = false)
		{
			InnerResult = innerResult;
			AlertClass = alertClass;
			Message = message;
            AppendToExistingMessages = appendToExistingMessages;
		}

        public override Task ExecuteResultAsync(ActionContext context)
        {
            PrepareMessage(context);
            return InnerResult.ExecuteResultAsync(context);
        }

        //ActionResult not IActionResult
        //public override void ExecuteResult(ActionContext context)
        //{
        //    PrepareMessage(context);
        //    InnerResult.ExecuteResult(context);
        //}

        private void PrepareMessage(ActionContext context)
        {
            var tempData = context.HttpContext.RequestServices.GetRequiredService<ITempDataDictionary>();
            var alert = new Alert(AlertClass, Message);

            if (AppendToExistingMessages)
                tempData.AddAlert(alert);
            else tempData.SetAlert(alert);
        }

    }
}