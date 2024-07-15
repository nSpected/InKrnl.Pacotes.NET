using System.Runtime.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InKrnl.Pacotes.Configuração;

public static class ExtensõesDeIConfiguration
{
    /// <summary>
    /// Extrai uma seção do appsettings para dentro da classe (Obs: Case-Sensitive ao nome da classe).
    /// </summary>
    /// <param name="configuração"></param>
    /// <typeparam name="TConfiguração"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static TConfiguração ObterConfiguração<TConfiguração>(this IConfiguration configuração) where TConfiguração : class
    {
        return configuração.GetSection(typeof(TConfiguração).Name).Get<TConfiguração>() ??
               throw new SerializationException(
                   $"A seção da configuração [{typeof(TConfiguração).Name}] não está configurada corretamente no appsettings.");
    }

    /// <summary>
    /// Extrai e injeta a seção com o mesmo nome da classe de configuração como Scoped nos serviços da aplicação (Obs: Case-Sensitive ao nome da classe).
    /// </summary>
    /// <param name="serviços"></param>
    /// <param name="configuração"></param>
    /// <typeparam name="TConfiguração"></typeparam>
    public static IServiceCollection InjetarConfiguração<TConfiguração>(
        this IServiceCollection serviços, IConfiguration configuração) where TConfiguração : class =>
        serviços.AddScoped(_ => configuração.ObterConfiguração<TConfiguração>());
}