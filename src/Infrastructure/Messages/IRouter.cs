using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Messages
{
    public interface IRouter
    {
        void Map<TEvent>(Delegate action);
    }
}
