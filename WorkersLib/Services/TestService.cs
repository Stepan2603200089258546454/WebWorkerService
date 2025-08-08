using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkersLib.Services
{
    public class TestService
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
    }
}
