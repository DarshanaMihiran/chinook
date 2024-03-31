using Microsoft.AspNetCore.Components;

namespace Chinook.Common
{
    public class BaseComponent : ComponentBase
    {
        protected string? InfoMessage { get; set; }
        protected string? ErrorMessage { get; set; }

        protected void CloseInfoMessage()
        {
            InfoMessage = "";
        }

        protected void CloseErrorMessage()
        {
            ErrorMessage = "";
        }

        protected void HandleError(Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}
