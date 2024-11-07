using CSharpFunctionalExtensions;
using TagService.HelperClasses;

namespace TagService.Entities;

public class Tag : Entity<Guid>
{
    public const int TEXT_MAX_LENGTH = 5000;
    
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public int UsagesCount { get; private set; }
    
    public Tag(Guid id, string name, string description, DateTime createdAt, int usagesCount) : base(id)
    {
        Name = name;
        Description = description;
        CreatedAt = createdAt;
        UsagesCount = usagesCount;
    }

    public static Result<Tag, Error> Cretate(string name, string description, DateTime createdAt, int usagesCount)
    {
        if(string.IsNullOrWhiteSpace(name) || name.Length > TEXT_MAX_LENGTH)
            return Error.Validation("Name cannot be empty");
        
        if(string.IsNullOrWhiteSpace(description) || description.Length > TEXT_MAX_LENGTH)
            return Error.Validation("Description name cannot be empty");
        
        if(usagesCount < 0)
            return Error.Validation("Usages count name cannot be less than zero");
        
        return new Tag(Guid.NewGuid(), name, description, createdAt, usagesCount);
    }

    public UnitResult<Error> Edit(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > TEXT_MAX_LENGTH)
            return Error.Validation("Text is invalid");
        
        if(string.IsNullOrWhiteSpace(description) || description.Length > TEXT_MAX_LENGTH)
            return Error.Validation("Text is invalid");
        
        Name = name;
        Description = description;

        return Result.Success<Error>();
    }

    public void UsagesIncrease() => UsagesCount++;
    
    public void UsagesDecrease() => UsagesCount--;
}