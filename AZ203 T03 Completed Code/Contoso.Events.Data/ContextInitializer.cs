using Contoso.Events.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contoso.Events.Data
{
    public class ContextInitializer
    {
        public async Task InitializeAsync(EventsContext eventsContext)
        {
            await eventsContext.Database.EnsureCreatedAsync();
            if (!await eventsContext.Events.AnyAsync()){
                Event eventItem = new Event();
                eventItem.EventKey = "FY17SepGeneralConference";
                eventItem.StartTime = DateTime.Today;
                eventItem.EndTime = DateTime.Today.AddDays(3d);
                eventItem.Title = "FY17 September Technical Conference";
                eventItem.Description = "Sed in euismod mi.";
                eventItem.RegistrationCount = 1;
                eventsContext.Events.Add(eventItem);
            }
            await eventsContext.SaveChangesAsync();

        }
    }
}