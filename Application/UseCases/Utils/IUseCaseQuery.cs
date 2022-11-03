namespace Application.UseCases.Utils;

public interface IUseCaseQuery<out TOutPut>
{
   TOutPut Execute();
}