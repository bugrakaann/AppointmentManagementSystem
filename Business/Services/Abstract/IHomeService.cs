using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abstract
{
    public interface IHomeService
    {
        public string GetHttpErrorMessage(int id);
    }
}
