using Frenet.Logistic.Domain.Abstractions;
using MediatR;

namespace Frenet.Logistic.Application.Abstractions.Messaging;

/*
 * parâmetro genérico TQuery representa a consulta, que deve implementar a interface IQuery
 * (com TResponse como seu tipo de resposta).
 *
 * O manipulador retorna um Result<TResponse>,
 *  sucesso da operação ou falha.
 *
 * permitindo apenas consultas que seguem o contrato definido um TResponse.
 */
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
    
}