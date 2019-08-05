using Contoso.Events.Data;
using Contoso.Events.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.Events.Worker
{
    public static class ProcessDocuments
    {
        private static RegistrationContext _registrationsContext = _connection.GetCosmosContext();
        private static ConnectionManager _connection = new ConnectionManager();


        [FunctionName("ProcessDocuments")]
        public static async Task Run([BlobTrigger("signinsheets-pending/{name}")],[HttpTrigger(AuthorizationLevel.Anonymous, "get")], [Blob("signinsheets/{name}", FileAccess.Write)],HttpRequest request, TraceWriter log)
        {
            string eventKey = Path.GetFileNameWithoutExtension(name);
            using (MemoryStream stream = await ProcessStorageMessage(eventKey))
            {
               byte[] byteArray = stream.ToArray(); 
               await output.WriteAsync(byteArray, 0, byteArray.Length);
            }
            string message = request.Query["eventkey"];

            log.Info($"Request received to generate sign-in sheet for event: {message}");

            var registrants = await ProcessHttpRequestMessage(message);

            log.Info($"Registrants: {String.Join(", ", registrants)}");

            log.Info($"Request completed for event: {message}");
        }

        private static async Task<List<string>> ProcessHttpRequestMessage(string eventKey)
        {
            using (EventsContext eventsContext = _connection.GetSqlContext())
            await _registrationsContext.ConfigureConnectionAsync();           
            {
                await eventsContext.Database.EnsureCreatedAsync();


                Event eventEntry = await eventsContext.Events.SingleOrDefaultAsync(e => e.EventKey == eventKey);

                List<string> registrants = await _registrationsContext.GetRegistrantsForEvent(eventKey);

                return registrants;
            }
        }

        private static async Task<MemoryStream> ProcessStorageMessage(string eventKey)
        {
            SignInDocumentGenerator documentGenerator = new SignInDocumentGenerator();

            using (EventsContext eventsContext = _connection.GetSqlContext())
            {
                await eventsContext.Database.EnsureCreatedAsync();
                
                Event eventEntry = await eventsContext.Events.SingleOrDefaultAsync(e => e.EventKey == eventKey);

                List<string> registrants = new List<string>();

                return MemoryStream.Null as MemoryStream;
            }
        }
    }
}