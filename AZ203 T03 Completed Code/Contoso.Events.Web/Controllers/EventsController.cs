using Contoso.Events.Data;
using Contoso.Events.Models;
using Contoso.Events.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Contoso.Events.Management.Controllers
{
    [Route("[controller]")]
    public class EventsController : Controller
    {
        [HttpGet]
        [Route("{page:int?}", Name = "EventList")]
        public IActionResult Index([FromServices] EventsContext eventsContext, [FromServices] IOptions<ApplicationSettings> appSettings, int? page)
        {
            int currentPage = page ?? 1;
            int totalRows = eventsContext.Events.Count();
            int pageSize = appSettings.Value.GridPageSize;
            var pagedEvents = eventsContext.Events
            .OrderByDescending(e => e.StartTime)
            .Skip(pageSize * (currentPage - 1))
            .Take(pageSize);

            EventsGridViewModel viewModel = new EventsGridViewModel
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalRows = totalRows,
                Events = pagedEvents
            };

            return View(viewModel);
        }

        [HttpGet]
        [Route("detail/{key}", Name = "EventDetail")]
        public IActionResult Detail([FromServices] EventsContext eventsContext, string key)
        {
            var matchedEvent = eventsContext.Events
            .SingleOrDefault(e => e.EventKey == key);

            EventDetailViewModel viewModel = new EventDetailViewModel
            {
                Event = matchedEvent
            };

            return View(viewModel);
        }
    }
}
