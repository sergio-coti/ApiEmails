var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Configurar o Swagger para autentica��o b�sica
    options.AddSecurityDefinition("basic", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "basic",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Entre com o nome de usu�rio e senha na seguinte estrutura: `username:password`."
    });

    // Configurar os requisitos de seguran�a globais para o Swagger
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FullCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()    // Permitir qualquer origem
              .AllowAnyMethod()    // Permitir qualquer m�todo (GET, POST, PUT, etc.)
              .AllowAnyHeader();   // Permitir qualquer cabe�alho
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Email API v1");
    options.DocumentTitle = "Email API - Documenta��o com Autentica��o B�sica";
});

app.UseAuthorization();

app.UseCors("FullCorsPolicy");

app.MapControllers();

app.Run();
