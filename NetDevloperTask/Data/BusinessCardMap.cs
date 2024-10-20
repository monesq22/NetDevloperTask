using CsvHelper.Configuration;
using NetDevloperTask.Models;

public class BusinessCardMap : ClassMap<BusinessCard>
{
    public BusinessCardMap()
    {
        Map(m => m.Id).Ignore();
        Map(m => m.Name);
        Map(m => m.Gender);
        Map(m => m.DateOfBirth)
            .TypeConverterOption.Format("MM/dd/yyyy");
        Map(m => m.Email);
        Map(m => m.Phone);
        Map(m => m.Address);
        Map(m => m.Photo);
    }
}
