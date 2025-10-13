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

// --- Configuração das Chaves e Strings de Conexão ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);


// --- Registro dos Serviços (Injeção de Dependência) ---

// 1. Controllers, Validação e OpenAPI/Scalar
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUpdateDespesaValidator>();
builder.Services.AddOpenApi();

// 2. Banco de Dados e Repositórios
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IDespesaRepository, DespesaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// 3. Serviços de Aplicação
builder.Services.AddScoped<DespesaService>();
builder.Services.AddScoped<TokenService>();      // <-- ADICIONADO (Você precisará dele em breve)
builder.Services.AddScoped<UsuarioService>();    // <-- A LINHA QUE FALTAVA!

// 4. Autenticação JWT
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

// --- Configuração do Pipeline HTTP ---
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// A ordem aqui é CRÍTICA e não deve ser duplicada
app.UseAuthentication(); // 1º - Quem é você?
app.UseAuthorization();  // 2º - O que você pode fazer?

app.MapControllers();

app.Run();