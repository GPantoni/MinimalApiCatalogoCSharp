using ApiCatalogo.Models;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Authorization;

namespace ApiCatalogo.ApiEndpoints;

public static class AutenticacaoEndpoints
{
    public static void MapAutenticacaoEndpoints(this WebApplication app)
    {
        //endpoint para login
        app.MapPost("/login", [AllowAnonymous](UserModel userModel, ITokenService tokenService) =>
            {
                if (userModel == null)
                    return Results.BadRequest("Login Inválido");

                if (userModel.UserName != "glauco" || userModel.Password != "Numsey#123")
                    return Results.BadRequest("Login Inválido");
                
                var tokenString = tokenService.GerarToken(app.Configuration["Jwt:Key"],
                    app.Configuration["Jwt:Issuer"],
                    app.Configuration["Jwt:Audience"], userModel);
                return Results.Ok(new { Token = tokenString });

            }).Produces(StatusCodes.Status400BadRequest).Produces(StatusCodes.Status200OK).WithName("Login")
            .WithTags("Autenticação");
    }
}