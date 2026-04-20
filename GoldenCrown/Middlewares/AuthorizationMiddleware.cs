using GoldenCrown.Attributes;
using GoldenCrown.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace GoldenCrown.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApplicationDbContext _context;

        public AuthorizationMiddleware(RequestDelegate next, ApplicationDbContext context)
        {
            _next = next;
            _context = context;
        }

        public async Task InvokeAsync (HttpContext context)
        {
            var attribute = context.GetEndpoint()?.Metadata.GetMetadata<MyAuthorizeAttribute>();
            if (attribute == null)
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var session = await _context.Sessions.FirstOrDefaultAsync(x => x.Token == token);
            if (session == null || session.ExpiresAt < DateTime.UtcNow)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            context.Items["UserId"] = session.UserId;
            await _next(context);
        }
    }
}
