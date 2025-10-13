using FinancialExpensesAPI.Application.Services;
using FinancialExpensesAPI.Application.Validators;
using FinancialExpensesAPI.Domain.Interfaces;
using FinancialExpensesAPI.Infrastructure.Data;
using FinancialExpensesAPI.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- Configura��o das Chaves e Strings de Conex�o ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);


// --- Registro dos Servi�os (Inje��o de Depend�ncia) ---

// 1. Controllers, Valida��o e OpenAPI/Scalar
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUpdateDespesaValidator>();
builder.Services.AddOpenApi();

// 2. Banco de Dados e Reposit�rios
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IDespesaRepository, DespesaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// 3. Servi�os de Aplica��o
builder.Services.AddScoped<DespesaService>();
builder.Services.AddScoped<TokenService>();      // <-- ADICIONADO (Voc� precisar� dele em breve)
builder.Services.AddScoped<UsuarioService>();    // <-- A LINHA QUE FALTAVA!

// 4. Autentica��o JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


var app = builder.Build();

// --- Configura��o do Pipeline HTTP ---
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// A ordem aqui � CR�TICA e n�o deve ser duplicada
app.UseAuthentication(); // 1� - Quem � voc�?
app.UseAuthorization();  // 2� - O que voc� pode fazer?

app.MapControllers();

app.Run();