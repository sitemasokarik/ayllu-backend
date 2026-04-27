namespace DcodePe.Catering.Application.External.ApplicationInsightsService
{
    public interface IInsertApplicationInsightsService
    {
        bool Execute(InsertApplicationInsightsModel metric);
    }
}
