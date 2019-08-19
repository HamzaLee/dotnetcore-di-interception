namespace DotNetCore.DI.Interception.Sample.Services
{
    public class CustomService : ICustomService
    {
        private readonly ICustomRepository _repository;

        public CustomService(ICustomRepository repository)
        {
            _repository = repository;
        }
        public virtual string GetValue() => nameof(CustomService) + "(" + _repository.GetValue() + ")";
    }
}
