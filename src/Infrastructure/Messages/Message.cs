using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Messages
{
    public class Message<T> where T : new()
    {
        public T Data { get; set; }
        IDictionary<string, string> Headers { get; set; }
    }
}
