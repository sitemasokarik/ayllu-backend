

namespace DcodePe.Catering.Application.Exeptions
{
    public class ExceptionManager : IExceptionFilter
    {
        private readonly IInsertApplicationInsightsService _insertApplicationInsightsService;

        public ExceptionManager(IInsertApplicationInsightsService insertApplicationInsightsService)
        {
            _insertApplicationInsightsService = insertApplicationInsightsService;

        }
        public void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult(ResponseApiService.Response(
                StatusCodes.Status500InternalServerError, "Error interno del servidor",  context.Exception.Message));


            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
            var metric =new  InsertApplicationInsightsModel(
                ApplicationInsightsConstants.METRIC_TYPE_ERROR,
                context.Exception.Message, 
                context.Exception.ToString());
            _insertApplicationInsightsService.Execute(metric);
        }
    }
}
