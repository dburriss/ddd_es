using Domain.Commands;

namespace Domain
{
    public interface IApplicationCommandService
    {
        void Handle(ICommand command);
    }
}