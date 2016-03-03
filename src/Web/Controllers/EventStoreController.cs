using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Domain.Events;
using Domain;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    public class EventStoreController : Controller
    {
        private readonly IEventStore _eventStore;

        public EventStoreController(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public IActionResult Tags()
        {
            var tags = _eventStore.Tags;
            return View(tags);
        }

        public IActionResult TagIds(string tag)
        {
            var ids = _eventStore.AggregateList(tag);
            ViewBag.Title = tag;
            return View(ids);
        }

        public IActionResult Stream(Guid id, string tag)
        {
            IEventStream stream = null;
            stream = _eventStore.Load(id);

            ViewBag.Title = tag;
            return View(stream);
        }
    }
}
