namespace DotNetCore.DI.Interception.Sample.Services
{
    public class CustomRepository : ICustomRepository
    {
        private readonly ICustomContext _context;

        public CustomRepository(ICustomContext context)
        {
            _context = context;
        }
        public string GetValue()
        {
            return $"{nameof(CustomRepository)}({_context.GetValue()})";
        }
    }
}