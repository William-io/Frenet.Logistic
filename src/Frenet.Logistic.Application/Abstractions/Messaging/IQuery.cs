using Frenet.Logistic.Domain.Abstractions;
using MediatR;

namespace Frenet.Logistic.Application.Abstractions.Messaging;

/**
 * IRequest é uma interface do MediatR, comunicação entre componentes
   Result<TResponse> é um tipo de retorno que encapsula o resultado da operação, 
  indicando sucesso ou falha atraves o Result.
 */
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
    
}