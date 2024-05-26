using Microsoft.AspNetCore.Components;

namespace Website.Components.Controls;

public partial class Waves : ComponentBase
{
    [Parameter]
    public bool Top { get; set; }
}
