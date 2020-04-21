using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attention.App.Services
{
    public class PixabayService : IPixabayService
    {
        private string v;

        public PixabayService(string v)
        {
            this.v = v;
        }
    }
}
