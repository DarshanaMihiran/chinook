using Microsoft.AspNetCore.Components;

namespace Chinook.Shared.Components
{
    public partial class Error
    {
        [Parameter]
        public string? ErrorMessage { get; set; }

        private void CloseErrorMessage()
        {
            ErrorMessage = "";
        }
    }
}
