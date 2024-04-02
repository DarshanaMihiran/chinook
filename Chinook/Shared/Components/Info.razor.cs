using Microsoft.AspNetCore.Components;

namespace Chinook.Shared.Components
{
    public partial class Info
    {
        [Parameter]
        public string? InfoMessage { get; set; }

        private void CloseInfoMessage()
        {
            InfoMessage = "";
        }
    }
}
