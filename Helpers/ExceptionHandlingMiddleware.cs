using System.Net;
using System.Text.Json;

namespace Events_system.Helpers
{
    public class ExceptionHandlingMiddleware
    {

        private readonly RequestDelegate _next;
        //RD NEXT - "ово је функција која представља све што долази после тебе".
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            //Прима следећи RequestDelegate у конструктору и чува га као _next.
            //„Позови следећи middleware у реду, и врати ми Task који се завршава кад он заврши.”
            _next = next;
        }

        //Ово је главна метода коју ASP.NET Core зове кад долази HTTP захтев.
        //HttpContext садржи податке о захтеву и омогућава слање одговора.

        public async Task InvokeAsync(HttpContext http)
        {
            try
            {
                await _next(http); // Пуштам захтев даље (контролер)
            }
            catch (KeyNotFoundException knf)
            {
                //Овде middleware "хвата" све што пуца у pipeline-у иза њега.
                //То укључује:Грешке у контролеру,Грешке у сервису,Грешке у репозиторијуму,
                //Буквално све иза await _next(...)

                http.Response.StatusCode = (int)HttpStatusCode.NotFound;
                http.Response.ContentType = "application/json";
                var payload = new { message = knf.Message };
                await http.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
            catch (InvalidOperationException ioe)
            {
                http.Response.StatusCode = (int)HttpStatusCode.Conflict;
                http.Response.ContentType = "application/json";
                var payload = new { message = ioe.Message };
                await http.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
            catch (Exception)
            {
                http.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                http.Response.ContentType = "application/json";
                var payload = new { message = "An unexpected error occurred." };
                await http.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
        }

    }
}
