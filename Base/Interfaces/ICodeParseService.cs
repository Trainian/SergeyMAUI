namespace Base.Interfaces
{
    public interface ICodeParseService
    {
        public Task<string> FindNameByCode(string code);
    }
}
