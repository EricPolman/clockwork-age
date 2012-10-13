using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clockwork_Age_Editor
{
    interface ISelectable
    {
        bool Selected { get; set; }
        bool Selectable { get; set; }

    }
}
