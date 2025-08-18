public interface IPlatformsService
{
    ImportReport LoadFromText(string text);
    IEnumerable<string> Search(string location);
}