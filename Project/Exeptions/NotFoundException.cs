namespace Project.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException() : base("Game not found!")
    {

    }
}
