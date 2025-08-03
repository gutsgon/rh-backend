using Rh_Backend.Repository;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.Services;
using Rh_Backend.Services.Interfaces;

namespace SeuProjeto.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Repositórios
            services.AddScoped<ICargoRepository, CargoRepository>();
            services.AddScoped<IContratoRepository, ContratoRepository>();
            services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
            services.AddScoped<IHistoricoAlteracaoRepository, HistoricoAlteracaoRepository>();
            services.AddScoped<IFeriasRepository, FeriasRepository>();

            // Serviços
            services.AddScoped<ICargoService, CargoService>();
            services.AddScoped<IContratoService, ContratoService>();
            services.AddScoped<IFuncionarioService, FuncionarioService>();
            services.AddScoped<IHistoricoAlteracaoService, HistoricoAlteracaoService>();
            services.AddScoped<IFeriasService, FeriasService>();

            return services;
        }
    }
}
