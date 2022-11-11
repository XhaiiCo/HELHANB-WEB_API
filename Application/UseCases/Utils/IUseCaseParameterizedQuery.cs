namespace Application.UseCases.Utils;

public interface IUseCaseParameterizedQuery<out TOutput, in TInput>
{
    TOutput Execute(TInput input);
}