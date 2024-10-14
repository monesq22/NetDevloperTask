using CsvHelper.Configuration;
using NetDevloperTask.Models;

public class BusinessCardMap : ClassMap<BusinessCard>
{
    public BusinessCardMap()
    {
        Map(m => m.Id).Ignore();  // Ignore 'Id' field since it's auto-generated
        Map(m => m.Name);
        Map(m => m.Gender);
        Map(m => m.DateOfBirth)
            .TypeConverterOption.Format("MM/dd/yyyy");  // Adjust format based on your CSV file
        Map(m => m.Email);
        Map(m => m.Phone);
        Map(m => m.Address);
        Map(m => m.Photo);
    }
}
